﻿using Blitz.Client.Common;
using Blitz.Client.Common.Report;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Customer
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

            ViewService.RegionBuilder<ReportViewerViewModel>()
                .WithInitialisation(viewModel => Disposables.Add(this.SyncViewModelDeActivation(viewModel)))
                .Show(RegionNames.REPORT);
        }
    }
}