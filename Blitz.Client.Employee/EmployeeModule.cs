using Blitz.Client.Common;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.Menu;
using Blitz.Client.Employee.Report;
using Blitz.Client.ModernUI.Assets.Icons;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;

namespace Blitz.Client.Employee
{
    public class EmployeeModule : IModule
    {
        private readonly ILog _log;
        private readonly IViewService _viewService;
        private readonly IMenuService _menuService;

        public EmployeeModule(ILog log, IViewService viewService, IMenuService menuService)
        {
            _log = log;
            _viewService = viewService;
            _menuService = menuService;
        }

        public void Initialize()
        {
            CreateMenu();
        }

        private void CreateMenu()
        {
            var employeeMenuItem = _menuService.CreateMenuGroupItem();
            employeeMenuItem.DisplayName = "Employee";

            var newReportMenuItem = _menuService.CreateMenuButtonItem();
            newReportMenuItem.DisplayName = "New";
            newReportMenuItem.ImageName = IconNames.EXCEL;
            newReportMenuItem.Command = new DelegateCommand(() =>
            {
                _log.Info("Adding Employee Report to Main region");
                var viewModel = _viewService.RegionBuilder<ReportViewModel>()
                    .WithScope()
                    .WithInitialisation(x => x.DisplayName = "Employee Report")
                    .Show(RegionNames.MAIN);
                ((ISupportActivationState)viewModel).Activate();
            });
            employeeMenuItem.Items.Add(newReportMenuItem);

            _menuService.Items.Add(employeeMenuItem);
        }
    }
}