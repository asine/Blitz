using Microsoft.Practices.Unity;

namespace Blitz.Common.Core
{
    public static class UnityExtensions
    {
        public static IUnityContainer RegisterSingleton<TFrom, TTo>(this IUnityContainer container) 
            where TTo : TFrom
        {
            container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());

            return container;
        }

        public static IUnityContainer RegisterTransient<TFrom, TTo>(this IUnityContainer container)
            where TTo : TFrom
        {
            container.RegisterType<TFrom, TTo>();

            return container;
        }
        
        public static IUnityContainer RegisterSingletonInstance<T>(this IUnityContainer container, T instance)
        {
            container.RegisterInstance<T>(instance, new ContainerControlledLifetimeManager());

            return container;
        }
    }
}