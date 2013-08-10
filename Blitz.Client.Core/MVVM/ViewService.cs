using System;
using System.Windows;

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Blitz.Client.Core.MVVM
{
    public class ViewService : IViewService
    {
        private readonly ILog _log;
        private readonly Func<IRegionManager> _regionManagerFactory;
        private readonly IUnityContainer _container;

        public ViewService(ILog log, Func<IRegionManager> regionManagerFactory, IUnityContainer container)
        {
            _log = log;
            _regionManagerFactory = regionManagerFactory;
            _container = container;
        }

        public void AddToRegion(IViewModel viewModel, string regionName, bool scoped = false)
        {
            _log.Info("Scope = {0}", scoped);
            var container = GetContainer(_container, scoped);

            _log.Info("Creating View for ViewModel - {0}", viewModel.GetType().FullName);
            var view = CreateView(container, viewModel.GetType());

            _log.Info("Binding View and ViewModel - {0}", viewModel.GetType().FullName);
            BindViewModel(view, viewModel);

            _log.Info("Adding View and ViewModel - {0} - to Region - {1}", viewModel.GetType().FullName, regionName);
            var regionManager = AddToRegion(_regionManagerFactory, regionName, view, scoped);

            if (scoped)
                container.RegisterInstance(regionManager);
        }

        public void AddToRegion<TViewModel>(string regionName, bool scoped = false)
            where TViewModel : IViewModel
        {
            _log.Info("Scope = {0}", scoped);
            var container = GetContainer(_container, scoped);

            _log.Info("Creating View for ViewModel - {0}", typeof(TViewModel).FullName);
            var view = CreateView(container, typeof (TViewModel));

            _log.Info("Creating ViewModel - {0}", typeof(TViewModel).FullName);
            var viewModel = CreateViewModel<TViewModel>(container);

            _log.Info("Binding View and ViewModel - {0}", typeof(TViewModel).FullName);
            BindViewModel(view, viewModel);

            _log.Info("Adding View and ViewModel - {0} - to Region - {1}", typeof(TViewModel).FullName, regionName);
            var regionManager = AddToRegion(_regionManagerFactory, regionName, view, scoped);

            if (scoped)
                container.RegisterInstance(regionManager);
        }

        public void AddToRegion<TViewModel>(string regionName, Action<TViewModel> initialiseViewModel, bool scoped = false)
            where TViewModel : IViewModel
        {
            _log.Info("Scope = {0}", scoped);
            var container = GetContainer(_container, scoped);

            _log.Info("Creating View for ViewModel - {0}", typeof(TViewModel).FullName);
            var view = CreateView(container, typeof(TViewModel));

            _log.Info("Creating ViewModel - {0}", typeof(TViewModel).FullName);
            var viewModel = CreateViewModel<TViewModel>(container);

            initialiseViewModel(viewModel);

            _log.Info("Binding View and ViewModel - {0}", typeof(TViewModel).FullName);
            BindViewModel(view, viewModel);

            _log.Info("Adding View and ViewModel - {0} - to Region - {1}", typeof(TViewModel).FullName, regionName);
            var regionManager = AddToRegion(_regionManagerFactory, regionName, view, scoped);

            if (scoped)
                container.RegisterInstance(regionManager);
        }

        private static FrameworkElement CreateView(IUnityContainer container, Type viewModelType)
        {
            // Work out the view type from the ViewModel type 
            var viewTypeName = viewModelType.FullName.Replace("Model", "");

            // Check to see if there is a UseViewAttribute on the ViewModel
            var useViewAttribute = Attribute.GetCustomAttribute(viewModelType, typeof (UseViewAttribute), true) as UseViewAttribute;
            var viewType = useViewAttribute != null ? useViewAttribute.ViewType : viewModelType.Assembly.GetType(viewTypeName);

            var view = (FrameworkElement)container.Resolve(viewType);

            return view;
        }

        private static TViewModel CreateViewModel<TViewModel>(IUnityContainer container)
            where TViewModel : IViewModel
        {
            return container.Resolve<TViewModel>();
        }

        private static void BindViewModel<TViewModel>(FrameworkElement view, TViewModel viewModel)
            where TViewModel : IViewModel
        {
            view.DataContext = viewModel;
        }

        private static IRegionManager AddToRegion(Func<IRegionManager> regionManagerFactory, string regionName, FrameworkElement view, bool scoped = false)
        {
            var regionManager = regionManagerFactory();
            var scopedRegionManager = regionManager.Regions[regionName].Add(view, null, scoped);
            regionManager.Regions[regionName].Activate(view);

            return scopedRegionManager;
        }

        private static IUnityContainer GetContainer(IUnityContainer container, bool scoped)
        {
            return scoped ? container.CreateChildContainer() : container;
        }
    }
}