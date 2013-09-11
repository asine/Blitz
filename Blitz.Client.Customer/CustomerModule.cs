using Blitz.Client.Common;
using Blitz.Client.Customer.Report;
using Blitz.Client.Customer.ReportLayout;
using Blitz.Client.Customer.ReportRunner;
using Blitz.Client.Customer.Reportviewer;

using Common.Logging;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

using Naru.WPF;
using Naru.WPF.ModernUI.Assets.Icons;
using Naru.WPF.MVVM;
using Naru.WPF.MVVM.Menu;

namespace Blitz.Client.Customer
{
    public class CustomerModule : IModule
    {
        private readonly ILog _log;
        private readonly IViewService _viewService;
        private readonly IUnityContainer _container;
        private readonly IMenuService _menuService;

        public CustomerModule(ILog log, IViewService viewService, IUnityContainer container, IMenuService menuService)
        {
            _log = log;
            _viewService = viewService;
            _container = container;
            _menuService = menuService;
        }

        public void Initialize()
        {
            _container
                .RegisterTransient<IReportLayoutService, ReportLayoutService>()
                .RegisterTransient<IReportRunnerService, ReportRunnerService>()
                .RegisterTransient<IReportViewerService, ReportViewerService>();

            CreateMenu();
        }

        private void CreateMenu()
        {
            var customerMenuItem = _menuService.CreateMenuGroupItem();
            customerMenuItem.DisplayName = "Customer";

            var newReportMenuItem = _menuService.CreateMenuButtonItem();
            newReportMenuItem.DisplayName = "New";
            newReportMenuItem.ImageName = IconNames.NEW;
            newReportMenuItem.Command = new DelegateCommand(() =>
            {
                _log.Debug("Adding Customer Report to Main region");
                var viewModel = _viewService.RegionBuilder<ReportViewModel>()
                    .WithScope()
                    .WithInitialisation(x => x.SetDisplayName("Customer Report"))
                    .Show(RegionNames.MAIN);
                ((ISupportActivationState)viewModel).Activate();
            });
            customerMenuItem.Items.Add(newReportMenuItem);

            _menuService.Items.Add(customerMenuItem);
        }
    }
}