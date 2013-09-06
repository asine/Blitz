using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Moq;

using NUnit.Framework;

namespace Blitz.Tests.UnityAutoMockContainer
{
    public class UnityAutoMockContainerFixture
    {
        protected UnityAutoMockContainer GetAutoMockContainer(MockFactory factory)
        {
            return new UnityAutoMockContainer(factory);
        }

        public static void RunAllTests(Action<string> messageWriter)
        {
            var fixture = new UnityAutoMockContainerFixture();
            RunAllTests(fixture, messageWriter);
        }

        public static void RunAllTests(UnityAutoMockContainerFixture fixture, Action<string> messageWriter)
        {
            messageWriter("Starting Tests...");
            foreach (var assertion in fixture.GetAllAssertions)
            {
                assertion(messageWriter);
            }
            messageWriter("Completed Tests...");
        }

        public IEnumerable<Action<Action<string>>> GetAllAssertions
        {
            get
            {
                var tests = new List<Action<Action<string>>>();

                Func<string, string> putSpacesBetweenPascalCasedWords = (s) =>
                {
                    var r = new Regex("([A-Z]+[a-z]+)");
                    return r.Replace(s, m => (m.Value.Length > 3 ? m.Value : m.Value.ToLower()) + " ");
                };

                var methodInfos = this
                    .GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(w => w.GetCustomAttributes(typeof(TestAttribute), true).Any())
                    .OrderBy(ob => ob.Name);

                foreach (var methodInfo in methodInfos)
                {
                    MethodInfo info = methodInfo;
                    Action<Action<string>> a = messageWriter =>
                    {
                        messageWriter("Testing - " + putSpacesBetweenPascalCasedWords(info.Name));
                        info.Invoke(this, new object[0]);
                    };

                    tests.Add(a);
                }

                foreach (var action in tests)
                    yield return action;
            }
        }

        [Test]
        public void CreatesLooseMocksIfFactoryIsMockBehaviorLoose()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Loose));
            var component = factory.Resolve<TestComponent>();

            component.RunAll();
        }

        [Test]
        public void CanRegisterImplementationAndResolveIt()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Loose));
            factory.Register<ITestComponent, TestComponent>();

            var testComponent = factory.Resolve<ITestComponent>();

            Assert.IsNotNull(testComponent);
            Assert.IsFalse(testComponent is IMocked<ITestComponent>);
        }

        [Test]
        public void ResolveUnregisteredInterfaceReturnsMock()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Loose));

            var service = factory.Resolve<IServiceA>();

            Assert.IsNotNull(service);
            Assert.IsTrue(service is IMocked<IServiceA>);
        }

        [Test]
        public void DefaultConstructorWorksWithAllTests()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Loose));
            var a = false;
            var b = false;

            factory.GetMock<IServiceA>().Setup(x => x.RunA()).Callback(() => a = true);
            factory.GetMock<IServiceB>().Setup(x => x.RunB()).Callback(() => b = true);

            var component = factory.Resolve<TestComponent>();

            component.RunAll();

            Assert.IsTrue(a);
            Assert.IsTrue(b);
        }


        [Test]
        public void ThrowsIfStrictMockWithoutExpectation()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Strict));
            factory.GetMock<IServiceB>().Setup(x => x.RunB());

            var component = factory.Resolve<TestComponent>();
            Assert.ShouldThrow(typeof(MockException), component.RunAll);

        }


        [Test]
        public void StrictWorksWithAllExpectationsMet()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Strict));
            factory.GetMock<IServiceA>().Setup(x => x.RunA());
            factory.GetMock<IServiceB>().Setup(x => x.RunB());

            var component = factory.Resolve<TestComponent>();
            component.RunAll();
        }

        [Test]
        public void GetMockedInstanceOfConcreteClass()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Loose));
            var mockedInstance = factory.GetMock<TestComponent>();

            Assert.IsNotNull(mockedInstance);
            Assert.IsNotNull(mockedInstance.Object.ServiceA);
            Assert.IsNotNull(mockedInstance.Object.ServiceB);
        }

        [Test]
        public void GetMockedInstanceOfConcreteClassWithInterfaceConstructorParameter()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Loose));
            var mockedInstance = factory.GetMock<TestComponent>();
            Assert.IsNotNull(mockedInstance);
        }

        [Test]
        public void WhenMockedInstanceIsRetrievedAnyFutureResolvesOfTheSameConcreteClassShouldReturnedTheMockedInstance()
        {
            var factory = GetAutoMockContainer(new MockFactory(MockBehavior.Loose));
            var mockedInstance = factory.GetMock<TestComponent>();

            var resolvedInstance = factory.Resolve<TestComponent>();

            Assert.IsTrue(Object.ReferenceEquals(resolvedInstance, mockedInstance.Object));
        }

        [Test]
        public void ShouldBeAbleToGetMockedInstanceOfAbstractClass()
        {
            var factory = new UnityAutoMockContainer();
            var mock = factory.GetMock<AbstractTestComponent>();
            Assert.IsNotNull(mock);
        }

        public interface IServiceA
        {
            void RunA();
        }

        public interface IServiceB
        {
            void RunB();
        }

        public class ServiceA : IServiceA
        {
            public ServiceA()
            {
            }

            public ServiceA(int count)
            {
                Count = count;
            }

            public ServiceA(IServiceB b)
            {
                ServiceB = b;
            }

            public IServiceB ServiceB { get; private set; }
            public int Count { get; private set; }

            public string Value { get; set; }

            public void RunA() { }
        }


        public interface ITestComponent
        {
            void RunAll();
            IServiceA ServiceA { get; }
            IServiceB ServiceB { get; }
        }

        public abstract class AbstractTestComponent
        {
            private readonly IServiceA _serviceA;
            private readonly IServiceB _serviceB;

            protected AbstractTestComponent(IServiceA serviceA, IServiceB serviceB)
            {
                _serviceA = serviceA;
                _serviceB = serviceB;
            }

            public abstract void RunAll();
            public IServiceA ServiceA { get { return _serviceA; } }
            public IServiceB ServiceB { get { return _serviceB; } }
        }

        public class TestComponent : ITestComponent
        {
            public TestComponent(IServiceA serviceA, IServiceB serviceB)
            {
                ServiceA = serviceA;
                ServiceB = serviceB;
            }

            public IServiceA ServiceA { get; private set; }
            public IServiceB ServiceB { get; private set; }

            public void RunAll()
            {
                ServiceA.RunA();
                ServiceB.RunB();
            }
        }
    }
}