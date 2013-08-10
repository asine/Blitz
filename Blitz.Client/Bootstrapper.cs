using System.Windows;
using System.Windows.Controls;

using Agatha.Common;

using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Customer;
using Blitz.Client.Shell;
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
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IViewService, ViewService>();
            Container.RegisterType<ILog, DebugLogger>();
            Container.RegisterType<IRequestTask, RequestTask>();

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
            new ClientConfiguration(typeof(AssemblyHook).Assembly, new Agatha.Unity.Container(container)).Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(typeof(Blitz.Common.AssemblyHook).Assembly);
        }
    }
}