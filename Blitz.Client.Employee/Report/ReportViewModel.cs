﻿using Blitz.Client.Common;
using Blitz.Client.Common.Report;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Employee.ReportRunner;
using Blitz.Common.Core;

namespace Blitz.Client.Employee.Report
{
    [UseView(typeof(ReportView))]
    public class ReportViewModel : Common.Report.ReportViewModel
    {
        public ReportViewModel(ILog log, IViewService viewService, IDispatcherService dispatcherService)
            : base(log, viewService, dispatcherService)
        {
        }

        protected override void OnInitialise()
        {
            ViewService.RegionBuilder<ReportRunnerViewModel>()
                .WithInitialisation(viewModel => Disposables.Add(this.SyncViewModelActivationStates(viewModel)))
                .Show(RegionNames.REPORT);
        }
    }
}