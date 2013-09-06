using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

using Moq;

namespace Blitz.Tests.UnityAutoMockContainer
{
    /// <summary>
    /// AutoMocking container that leverages the Unity IOC container and the Moq 
    /// mocking library to automatically mock classes resolved from the container.
    /// </summary>
    public class UnityAutoMockContainer
    {
        /// <summary>
        /// Value used when the internal Unity plugin needs to decide if an instance 
        /// class is to be created as a Mock(of T) or not.
        /// </summary>
        internal const string NAME_FOR_MOCKING = "____FOR____MOCKING____57ebd55f-9831-40c7-9a24-b7d450209ad0";

        private readonly IAutoMockerBackingContainer _container;

        /// <summary>
        /// Same as calling <code>new UnityAutoMockContainer(new MockFactory(MockBehavior.Loose))</code>
        /// </summary>
        public UnityAutoMockContainer()
            : this(new MockFactory(MockBehavior.Loose))
        {
        }

        /// <summary>
        /// Allows you to specify the MockFactory that will be used when creating mocked items.
        /// </summary>
        public UnityAutoMockContainer(MockFactory factory)
        {
            _container = new UnityAutoMockerBackingContainer(factory);
        }

        #region Public interface

        /// <summary>
        /// This is just a pass through to the underlying Unity Container. It will
        /// register the instance with the ContainerControlledLifetimeManager (Singleton)
        /// </summary>
        public UnityAutoMockContainer RegisterInstance<TService>(TService instance)
        {
            _container.RegisterInstance(instance);
            return this;
        }

        /// <summary>
        /// This is just a pass through to the underlying Unity Container. It will
        /// register the type with the ContainerControlledLifetimeManager (Singleton)
        /// </summary>
        public UnityAutoMockContainer Register<TService, TImplementation>()
            where TImplementation : TService
        {
            _container.RegisterType<TService, TImplementation>();
            return this;
        }

        /// <summary>
        /// This will create a Mock(of T) for any Interface or Class requested.
        /// </summary>
        /// <remarks>Note: that the Mock returned will live as a Singleton, so if you setup any expectations on the Mock(of T) then they will life for the lifetime of this container.</remarks>
        /// <typeparam name="T">Interface or Class that to create a Mock(of T) for.</typeparam>
        /// <returns>Mocked instance of the type T.</returns>
        public Mock<T> GetMock<T>()
            where T : class
        {
            return _container.ResolveForMocking<T>().Mock;
        }

        /// <summary>
        /// This will resolve an interface or class from the underlying container.
        /// </summary>
        /// <remarks>
        /// 1. If T is an interface it will return the Mock(of T).Object instance
        /// 2. If T is a class it will just return that class
        ///     - unless the class was first created by using the GetMock(of T) in which case it will return a Mocked instance of the class
        /// </remarks>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        #endregion

        private interface IAutoMockerBackingContainer
        {
            void RegisterInstance<TService>(TService instance);
            void RegisterType<TService, TImplementation>() where TImplementation : TService;
            T Resolve<T>();
            object Resolve(Type type);
            IMocked<T> ResolveForMocking<T>() where T : class;
        }

        private class UnityAutoMockerBackingContainer : IAutoMockerBackingContainer
        {
            private readonly IUnityContainer _unityContainer = new UnityContainer();

            public UnityAutoMockerBackingContainer(MockFactory factory)
            {
                _unityContainer.AddExtension(new MockFactoryContainerExtension(factory, this));
            }

            public void RegisterInstance<TService>(TService instance)
            {
                _unityContainer.RegisterInstance(instance, new ContainerControlledLifetimeManager());
            }

            public void RegisterType<TService, TImplementation>()
                where TImplementation : TService
            {
                _unityContainer.RegisterType<TService, TImplementation>(new ContainerControlledLifetimeManager());
            }

            public T Resolve<T>()
            {
                return _unityContainer.Resolve<T>();
            }

            public object Resolve(Type type)
            {
                return _unityContainer.Resolve(type);
            }

            public IMocked<T> ResolveForMocking<T>()
                where T : class
            {
                return (IMocked<T>) _unityContainer.Resolve<T>(NAME_FOR_MOCKING);
            }

            private class MockFactoryContainerExtension : UnityContainerExtension
            {
                private readonly MockFactory _mockFactory;
                private readonly IAutoMockerBackingContainer _container;

                public MockFactoryContainerExtension(MockFactory mockFactory, IAutoMockerBackingContainer container)
                {
                    _mockFactory = mockFactory;
                    _container = container;
                }

                protected override void Initialize()
                {
                    Context.Strategies.Add(new MockExtensibilityStrategy(_mockFactory, _container),
                        UnityBuildStage.PreCreation);
                }
            }

            private class MockExtensibilityStrategy : BuilderStrategy
            {
                private readonly MockFactory _factory;
                private readonly IAutoMockerBackingContainer _container;
                private readonly MethodInfo _createMethod;
                private readonly Dictionary<Type, Mock> _alreadyCreatedMocks = new Dictionary<Type, Mock>();
                private MethodInfo _createMethodWithParameters;

                public MockExtensibilityStrategy(MockFactory factory, IAutoMockerBackingContainer container)
                {
                    _factory = factory;
                    _container = container;
                    _createMethod = factory.GetType().GetMethod("Create", new Type[] {});
                    Debug.Assert(_createMethod != null);
                }

                public override void PreBuildUp(IBuilderContext context)
                {
                    NamedTypeBuildKey buildKey = (NamedTypeBuildKey) context.BuildKey;
                    bool isToBeAMockedClassInstance = buildKey.Name == NAME_FOR_MOCKING;
                    Type mockServiceType = buildKey.Type;

                    if (!mockServiceType.IsInterface && !isToBeAMockedClassInstance)
                    {
                        if (_alreadyCreatedMocks.ContainsKey(mockServiceType))
                        {
                            var mockedObject = _alreadyCreatedMocks[mockServiceType];
                            SetBuildObjectAndCompleteIt(context, mockedObject);
                        }
                        else
                        {
                            base.PreBuildUp(context);
                        }
                    }
                    else
                    {
                        Mock mockedObject;

                        if (_alreadyCreatedMocks.ContainsKey(mockServiceType))
                        {
                            mockedObject = _alreadyCreatedMocks[mockServiceType];
                        }
                        else
                        {
                            if (isToBeAMockedClassInstance && !mockServiceType.IsInterface)
                            {
                                object[] mockedParametersToInject = GetConstructorParameters(context).ToArray();

                                _createMethodWithParameters = _factory.GetType()
                                    .GetMethod("Create", new[] {typeof (object[])});

                                MethodInfo specificCreateMethod =
                                    _createMethodWithParameters.MakeGenericMethod(new[] {mockServiceType});

                                var x = specificCreateMethod.Invoke(_factory, new object[] {mockedParametersToInject});
                                mockedObject = (Mock) x;
                            }
                            else
                            {
                                MethodInfo specificCreateMethod =
                                    _createMethod.MakeGenericMethod(new[] {mockServiceType});
                                mockedObject = (Mock) specificCreateMethod.Invoke(_factory, null);
                            }

                            _alreadyCreatedMocks.Add(mockServiceType, mockedObject);
                        }


                        SetBuildObjectAndCompleteIt(context, mockedObject);
                    }
                }

                private static void SetBuildObjectAndCompleteIt(IBuilderContext context, Mock mockedObject)
                {
                    context.Existing = mockedObject.Object;
                    context.BuildComplete = true;
                }

                private List<object> GetConstructorParameters(IBuilderContext context)
                {
                    var parameters = new List<object>();
                    var policy = new DefaultUnityConstructorSelectorPolicy();
                    var constructor = policy.SelectConstructor(context, new PolicyList());
                    ConstructorInfo constructorInfo;
                    if (constructor == null)
                    {
                        // Unit constructor selector doesn't seem to want to find abstract class protected constructors
                        // quickly find one here...
                        var buildKey = (NamedTypeBuildKey) context.BuildKey;
                        var largestConstructor = buildKey.Type.GetConstructors(
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Instance)
                            .OrderByDescending(o => o.GetParameters().Length)
                            .FirstOrDefault();

                        constructorInfo = largestConstructor;
                    }
                    else
                    {
                        constructorInfo = constructor.Constructor;
                    }

                    foreach (var parameterInfo in constructorInfo.GetParameters())
                        parameters.Add(_container.Resolve(parameterInfo.ParameterType));

                    return parameters;
                }
            }
        }
    }
}
