using System;

using Blitz.Client.Employee.Report;

using Common.Logging;

using Naru.Core;
using Naru.WPF.Command;
using Naru.WPF.Menu;
using Naru.WPF.Assets.Icons;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Employee
{
    public class EmployeeStartable
    {
        private readonly ILog _log;
        private readonly IDispatcherSchedulerProvider _scheduler;
        private readonly IMenuService _menuService;
        private readonly IMessageStream _messageStream;
        private readonly Func<ReportViewModel> _reportViewModelFactory;

        public EmployeeStartable(ILog log, IDispatcherSchedulerProvider scheduler, IMenuService menuService, IMessageStream messageStream,
                                Func<ReportViewModel> reportViewModelFactory)
        {
            _log = log;
            _scheduler = scheduler;
            _menuService = menuService;
            _messageStream = messageStream;
            _reportViewModelFactory = reportViewModelFactory;
        }

        public void Start()
        {
            CreateMenu();
        }

        private void CreateMenu()
        {
            var employeeMenuItem = _menuService.CreateMenuGroupItem();
            employeeMenuItem.DisplayName = "Employee";

            var newReportMenuItem = _menuService.CreateMenuButtonItem();
            newReportMenuItem.DisplayName = "New";
            newReportMenuItem.ImageName = IconNames.NEW;
            newReportMenuItem.Command = new DelegateCommand(
                () =>
                {
                    _log.Debug("Adding Employee Report to Main region");

                    var reportViewModel = _reportViewModelFactory();
                    reportViewModel.SetupHeader(_scheduler, "Employee Report");
                    _messageStream.Push(reportViewModel);
                    reportViewModel.ActivationStateViewModel.Activate();
                });
            employeeMenuItem.Items.Add(newReportMenuItem);

            _menuService.Items.Add(employeeMenuItem);
        }
    }
}