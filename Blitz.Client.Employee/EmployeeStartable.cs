﻿using System;

using Blitz.Client.Common;
using Blitz.Client.Employee.Report;

using Common.Logging;

using Naru.Core;
using Naru.WPF.Command;
using Naru.WPF.Menu;
using Naru.WPF.ModernUI.Assets.Icons;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Employee
{
    public class EmployeeStartable
    {
        private readonly ILog _log;
        private readonly IMenuService _menuService;
        private readonly IEventStream _eventStream;
        private readonly Func<ReportViewModel> _reportViewModelFactory;

        public EmployeeStartable(ILog log, IMenuService menuService, IEventStream eventStream,
                                Func<ReportViewModel> reportViewModelFactory)
        {
            _log = log;
            _menuService = menuService;
            _eventStream = eventStream;
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
                    reportViewModel.SetupHeader("Employee Report");
                    _eventStream.Push(reportViewModel);
                    ((ISupportActivationState)reportViewModel).Activate();
                });
            employeeMenuItem.Items.Add(newReportMenuItem);

            _menuService.Items.Add(employeeMenuItem);
        }
    }
}