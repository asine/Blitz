using Microsoft.Practices.Unity;

namespace Blitz.Server
{
    public static class UnityExtensions
    {
        public static IUnityContainer RegisterSingleton<TFrom, TTo>(this IUnityContainer container)
            where TTo : TFrom
        {
            return container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer RegisterTransient<TFrom, TTo>(this IUnityContainer container)
            where TTo : TFrom
        {
            return container.RegisterType<TFrom, TTo>();
        }

        public static IUnityContainer RegisterSingletonInstance<T>(this IUnityContainer container, T instance)
        {
            return container.RegisterInstance<T>(instance, new ContainerControlledLifetimeManager());
        }
    }
}