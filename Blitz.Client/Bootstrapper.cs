using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using Agatha.Common;
using Agatha.Unity;

using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.Dialog;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Customer;
using Blitz.Client.Shell;
using Blitz.Common.Agatha;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace Blitz.Client
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new ShellView {DataContext = Container.Resolve<ShellViewModel>()};
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.Activate();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container
                .RegisterTransient<IViewService, ViewService>()
                .RegisterType(typeof(IDialogBuilder<>), typeof(DialogBuilder<>))
                .RegisterTransient<IRegionBuilder, RegionBuilder>()
                .RegisterType(typeof (IRegionBuilder<>), typeof (RegionBuilder<>))
                .RegisterTransient<ILog, DebugLogger>()
                .RegisterTransient<IRequestTask, RequestTask>()
                .RegisterSingleton<IToolBarService, ToolBarService>()
                .RegisterSingletonInstance<IDispatcherService>(new DispatcherService(Dispatcher.CurrentDispatcher));

            InitialiseAgatha(Container);
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(TabControl), Container.Resolve<TabControlRegionAdapter>());
            return mappings;
        }

        protected override void ConfigureModuleCatalog()
        {
            ((ModuleCatalog)ModuleCatalog).AddModule(typeof(CustomerReportModule));
        }

        private static void InitialiseAgatha(IUnityContainer container)
        {
            new ClientConfiguration(new Container(container))
                .AddRequestAndResponseAssembly(typeof (AssemblyHook).Assembly)
                .Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(typeof(Blitz.Common.AssemblyHook).Assembly);
        }
    }
}