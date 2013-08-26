using Microsoft.Practices.Unity;

namespace Blitz.Client.Core.MVVM
{
    public class BindableCollectionFactory
    {
        private readonly IUnityContainer _container;

        public BindableCollectionFactory(IUnityContainer container)
        {
            _container = container;
        }

        public BindableCollection<T> Get<T>()
        {
            return _container.Resolve<BindableCollection<T>>();
        }
    }
}