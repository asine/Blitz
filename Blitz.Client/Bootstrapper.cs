using System.Windows;
using System.Windows.Controls;

using Blitz.Client.Common.DynamicColumnEdit;
using Blitz.Client.Common.DynamicColumnManagement;
using Blitz.Client.Common.ExportToExcel;

using Naru.Agatha;
using Naru.Log4Net;
using Naru.Unity;
using Naru.WPF;
using Naru.WPF.MVVM;

using Blitz.Client.Shell;
using Blitz.Client.Trading;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

using Naru.WPF.Prism;
using Naru.WPF.Prism.TabControl;

namespace Blitz.Client
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var shellViewModel = Container.Resolve<ShellViewModel>();
            var shellView = ViewService.CreateView(shellViewModel.GetType());
            ViewService.BindViewModel(shellView, shellViewModel);

            return shellView;
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

            Container.RegisterInstance(Container);

            Container
                .ConfigureNaruLog4Net("ILogInject.UnityCommonLogging.Blitz.Client")
                .ConfigureNaruWPF()
                .ConfigureNaruPrism()
                .ConfigureNaruAgathaClient(typeof(Blitz.Common.AssemblyHook).Assembly)
                .RegisterTransient<IRequestTask, RequestTask>()
                .RegisterTransient<IBasicExportToExcel, BasicExportToExcel>()
                .RegisterType<IDynamicColumnManagementService, DynamicColumnManagementService>()
                .RegisterType<IDynamicColumnEditService, DynamicColumnEditService>();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(TabControl), Container.Resolve<TabControlRegionAdapter>());
            return mappings;
        }

        protected override void ConfigureModuleCatalog()
        {
            ((ModuleCatalog)ModuleCatalog).AddModule(typeof(Customer.CustomerModule));
            ((ModuleCatalog)ModuleCatalog).AddModule(typeof(Employee.EmployeeModule));
            ((ModuleCatalog)ModuleCatalog).AddModule(typeof(TradingModule));
        }
    }
}