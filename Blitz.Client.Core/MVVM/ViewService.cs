using System;
using System.Windows;

using Blitz.Client.Core.MVVM.Dialog;
using Blitz.Client.ModernUI.Windows.Controls;
using Blitz.Common.Core;

using Microsoft.Practices.Unity;

namespace Blitz.Client.Core.MVVM
{
    public class ViewService : IViewService
    {
        private readonly ILog _log;
        private readonly IUnityContainer _container;
        private readonly IDispatcherService _dispatcherService;

        public ViewService(ILog log, IUnityContainer container, IDispatcherService dispatcherService)
        {
            _log = log;
            _container = container;
            _dispatcherService = dispatcherService;
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

        public IStandardDialogBuilder StandardDialogBuilder()
        {
            return _container.Resolve<IStandardDialogBuilder>();
        }

        public void ShowModal(IViewModel viewModel)
        {
            _dispatcherService.ExecuteSyncOnUI(() =>
            {
                _log.Info("Creating View for ViewModel - {0}", viewModel.GetType().FullName);
                var view = CreateView(viewModel.GetType());

                _log.Info("Binding View and ViewModel - {0}", viewModel.GetType().FullName);
                BindViewModel(view, viewModel);

                var window = view as Window;
                if (window != null)
                {
                    ConnectUpClosing(viewModel, window);

                    window.Owner = Application.Current.MainWindow;
                    window.ShowDialog();
                }
                else
                {
                    window = new ModernWindow
                    {
                        Content = view,
                        Title = viewModel.DisplayName,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStyle = WindowStyle.ToolWindow,
                        Owner = Application.Current.MainWindow
                    };

                    ConnectUpActivation(viewModel, window);
                    ConnectUpClosing(viewModel, window);

                    window.ShowDialog();
                }
            });
        }

        private static void ConnectUpActivation(IViewModel viewModel, Window window)
        {
            var supportActivationState = viewModel as ISupportActivationState;
            if (supportActivationState == null) return;

            RoutedEventHandler windowOnLoaded = null;
            windowOnLoaded = (s, e) =>
            {
                supportActivationState.Activate();

                if (windowOnLoaded != null)
                {
                    window.Loaded -= windowOnLoaded;
                }
            };
            window.Loaded += windowOnLoaded;
        }

        private void ConnectUpClosing(IViewModel viewModel, Window window)
        {
            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            // ViewModel is closed
            EventHandler supportClosingOnClosed = null;
            supportClosingOnClosed = (s, e) =>
            {
                _dispatcherService.ExecuteSyncOnUI(window.Close);

                if (supportClosingOnClosed != null)
                {
                    supportClosing.OnClosed -= supportClosingOnClosed;
                }
            };
            supportClosing.OnClosed += supportClosingOnClosed;

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