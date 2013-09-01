using Blitz.Client.Common;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.Menu;
using Blitz.Client.Customer.Report;
using Blitz.Client.Customer.ReportLayout;
using Blitz.Client.ModernUI.Assets.Icons;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

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
                .RegisterTransient<IReportLayoutService, ReportLayoutService>();

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
                _log.Info("Adding Customer Report to Main region");
                var viewModel = _viewService.RegionBuilder<ReportViewModel>()
                    .WithScope()
                    .WithInitialisation(x => x.DisplayName = "Customer Report")
                    .Show(RegionNames.MAIN);
                ((ISupportActivationState)viewModel).Activate();
            });
            customerMenuItem.Items.Add(newReportMenuItem);

            _menuService.Items.Add(customerMenuItem);
        }
    }
}