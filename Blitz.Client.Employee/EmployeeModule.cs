using Blitz.Client.Common;

using Common.Logging;

using Naru.WPF;
using Naru.WPF.MVVM;
using Naru.WPF.MVVM.Menu;
using Blitz.Client.Employee.Report;
using Blitz.Client.Employee.ReportRunner;
using Naru.WPF.ModernUI.Assets.Icons;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

using Naru.WPF.Prism.Region;

namespace Blitz.Client.Employee
{
    public class EmployeeModule : IModule
    {
        private readonly ILog _log;
        private readonly IViewService _viewService;
        private readonly IUnityContainer _container;
        private readonly IMenuService _menuService;
        private readonly IRegionService _regionService;

        public EmployeeModule(ILog log, IViewService viewService, IUnityContainer container, IMenuService menuService, IRegionService regionService)
        {
            _log = log;
            _viewService = viewService;
            _container = container;
            _menuService = menuService;
            _regionService = regionService;
        }

        public void Initialize()
        {
            _container
                .RegisterTransient<IReportRunnerService, ReportRunnerService>();

            CreateMenu();
        }

        private void CreateMenu()
        {
            var employeeMenuItem = _menuService.CreateMenuGroupItem();
            employeeMenuItem.DisplayName = "Employee";

            var newReportMenuItem = _menuService.CreateMenuButtonItem();
            newReportMenuItem.DisplayName = "New";
            newReportMenuItem.ImageName = IconNames.NEW;
            newReportMenuItem.Command = new DelegateCommand(() =>
            {
                _log.Debug("Adding Employee Report to Main region");
                var viewModel = _regionService.RegionBuilder<ReportViewModel>()
                    .WithScope()
                    .WithInitialisation(x => x.SetupHeader("Employee Report"))
                    .Show(RegionNames.MAIN);
                ((ISupportActivationState)viewModel).Activate();
            });
            employeeMenuItem.Items.Add(newReportMenuItem);

            _menuService.Items.Add(employeeMenuItem);
        }
    }
}