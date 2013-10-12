using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using Agatha.Common;
using Agatha.Unity;

using Blitz.Client.Common.DynamicColumnEdit;
using Blitz.Client.Common.DynamicColumnManagement;
using Blitz.Client.Common.ExportToExcel;
using Blitz.Client.Core.Agatha;

using ILogInject.Unity;

using Naru.WPF;
using Naru.WPF.MVVM;

using Blitz.Client.Shell;
using Blitz.Client.Trading;
using Blitz.Common.Agatha;
using Blitz.Common.Core;

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

            // Configure Logging
            Container
                .AddNewExtension<BuildTracking>()
                .AddNewExtension<CommonLoggingLogCreationExtension>()
                .RegisterInstance<ILog4NetConfiguration>(new Log4NetConfiguration("ILogInject.UnityCommonLogging.Blitz.Client"));

            Container
                .ConfigureNaru()
                .ConfigureNaruPrism()
                .RegisterTransient<IRequestTask, RequestTask>()
                .RegisterTransient<IBasicExportToExcel, BasicExportToExcel>()
                .RegisterType<IDynamicColumnManagementService, DynamicColumnManagementService>()
                .RegisterType<IDynamicColumnEditService, DynamicColumnEditService>();

            InitialiseAgatha(Container);

            ConfigureLog4Net(Container);
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

        private static void InitialiseAgatha(IUnityContainer container)
        {
            new ClientConfiguration(new Container(container))
                .AddRequestAndResponseAssembly(typeof (AssemblyHook).Assembly)
                .Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(typeof(Blitz.Common.AssemblyHook).Assembly);
        }

        private static void ConfigureLog4Net(IUnityContainer container)
        {
            var configuration = container.Resolve<ILog4NetConfiguration>();
            if (!Directory.Exists(configuration.LogDirectoryPath))
            {
                Directory.CreateDirectory(configuration.LogDirectoryPath);
            }
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(configuration.LogDirectoryPath, configuration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
        }
    }
}