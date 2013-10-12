﻿using System.Threading.Tasks;

using Blitz.Client.Common;
using Blitz.Client.Common.Report;

using Common.Logging;

using Naru.WPF.MVVM;
using Blitz.Client.Employee.ReportRunner;

using Naru.WPF.Prism.Region;
using Naru.WPF.Scheduler;

namespace Blitz.Client.Employee.Report
{
    [UseView(typeof(ReportView))]
    public class ReportViewModel : Common.Report.ReportViewModel
    {
        private readonly IRegionService _regionService;

        public ReportViewModel(ILog log, IViewService viewService, IScheduler scheduler, IRegionService regionService)
            : base(log, viewService, scheduler)
        {
            _regionService = regionService;
        }

        protected override Task OnInitialise()
        {
            return _regionService.RegionBuilder<ReportRunnerViewModel>()
                .WithInitialisation(viewModel => Disposables.Add(this.SyncViewModelActivationStates(viewModel)))
                .ShowAsync(RegionNames.REPORT);
        }
    }
}