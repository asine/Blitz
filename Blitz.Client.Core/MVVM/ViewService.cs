using System;
using System.Windows;

using Blitz.Client.Core.MVVM.Dialog;
using Blitz.Common.Core;

using Microsoft.Practices.Unity;

namespace Blitz.Client.Core.MVVM
{
    public class ViewService : IViewService
    {
        private readonly ILog _log;
        private readonly IUnityContainer _container;

        public ViewService(ILog log, IUnityContainer container)
        {
            _log = log;
            _container = container;
        }

        public IRegionBuilder RegionBuilder()
        {
            return _container.Resolve<IRegionBuilder>();
        }

        public IRegionBuilder<TViewModel> RegionBuilder<TViewModel>()
            where TViewModel : IViewModel
        {
            return _container.Resolve<IRegionBuilder<TViewModel>>();
        }

        public IDialogBuilder<Answer> DialogBuilder()
        {
            return _container.Resolve<IDialogBuilder<Answer>>();
        }

        public IDialogBuilder<T> DialogBuilder<T>()
        {
            return _container.Resolve<IDialogBuilder<T>>();
        }

        public void ShowModel(IViewModel viewModel)
        {
            _log.Info("Creating View for ViewModel - {0}", viewModel.GetType().FullName);
            var view = CreateView(viewModel.GetType());

            _log.Info("Binding View and ViewModel - {0}", viewModel.GetType().FullName);
            BindViewModel(view, viewModel);

            var window = view as Window;
            if (window != null)
            {
                ConnectUpClosing(viewModel, window);

                window.ShowDialog();
            }
            else
            {
                window = new Window
                {
                    Content = view,
                    Title = viewModel.DisplayName
                };

                ConnectUpClosing(viewModel, window);

                window.ShowDialog();
            }
        }

        private static void ConnectUpClosing(IViewModel viewModel, Window window)
        {
            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            // ViewModel is closed
            EventHandler supportClosingOnClosed = null;
            supportClosingOnClosed = (s, e) =>
            {
                window.Close();

                if (supportClosingOnClosed != null)
                {
                    supportClosing.Closed -= supportClosingOnClosed;
                }
            };
            supportClosing.Closed += supportClosingOnClosed;

            // Window is closed
            EventHandler windowOnClosed = null;
            windowOnClosed = (s, e) =>
            {
                supportClosing.Close();

                if (windowOnClosed != null)
                {
                    window.Closed -= windowOnClosed;
                }
            };
            window.Closed += windowOnClosed;
        }

        public static FrameworkElement CreateView(Type viewModelType)
        {
            // Work out the view type from the ViewModel type 
            var viewTypeName = viewModelType.FullName.Replace("Model", "");

            // Check to see if there is a UseViewAttribute on the ViewModel
            var useViewAttribute = Attribute.GetCustomAttribute(viewModelType, typeof (UseViewAttribute), true) as UseViewAttribute;
            var viewType = useViewAttribute != null ? useViewAttribute.ViewType : viewModelType.Assembly.GetType(viewTypeName);

            var view = (FrameworkElement)Activator.CreateInstance(viewType);

            return view;
        }

        internal static TViewModel CreateViewModel<TViewModel>(IUnityContainer container)
            where TViewModel : IViewModel
        {
            return container.Resolve<TViewModel>();
        }

        public static void BindViewModel<TViewModel>(FrameworkElement view, TViewModel viewModel)
            where TViewModel : IViewModel
        {
            view.DataContext = viewModel;
        }

        internal static IUnityContainer GetContainer(IUnityContainer container, bool scoped)
        {
            return scoped ? container.CreateChildContainer() : container;
        }
    }
}