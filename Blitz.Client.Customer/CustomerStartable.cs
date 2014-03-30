using System;
using System.Threading.Tasks;

using Blitz.Client.Customer.Report;

using Common.Logging;

using Naru.Core;
using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.Menu;
using Naru.WPF.Assets.Icons;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer
{
    public class CustomerStartable
    {
        private readonly ILog _log;
        private readonly IMenuService _menuService;
        private readonly IEventStream _eventStream;
        private readonly Func<ReportViewModel> _reportViewModelFactory;
        private readonly IDispatcherSchedulerProvider _scheduler;

        public CustomerStartable(ILog log, IMenuService menuService, IEventStream eventStream,
                                 Func<ReportViewModel> reportViewModelFactory, IDispatcherSchedulerProvider scheduler)
        {
            _log = log;
            _menuService = menuService;
            _eventStream = eventStream;
            _reportViewModelFactory = reportViewModelFactory;
            _scheduler = scheduler;
        }

        public void Start()
        {
            CreateMenu();
        }

        private void CreateMenu()
        {
            var customerMenuItem = _menuService.CreateMenuGroupItem();
            customerMenuItem.DisplayName = "Customer";

            var newReportMenuItem = _menuService.CreateMenuButtonItem();
            newReportMenuItem.DisplayName = "New";
            newReportMenuItem.ImageName = IconNames.NEW;
            newReportMenuItem.Command = new DelegateCommand(() => OpenReport());
            customerMenuItem.Items.Add(newReportMenuItem);

            _menuService.Items.Add(customerMenuItem);
        }

        private void OpenReport()
        {
            Task.Factory
                .StartNew(() =>
                          {
                              _log.Debug("Adding Customer Report to Main region");

                              var reportViewModel = _reportViewModelFactory();
                              reportViewModel.SetupHeader(_scheduler, "Customer Report");
                              _eventStream.Push(reportViewModel);

                              return reportViewModel;
                          }, _scheduler.Dispatcher.TPL)
                .Do(reportViewModel => reportViewModel.ActivationStateViewModel.Activate(), _scheduler.Dispatcher.TPL);
        }
    }
}