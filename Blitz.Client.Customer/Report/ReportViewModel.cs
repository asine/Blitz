using Blitz.Client.Common;
using Blitz.Client.Common.Report;

using Common.Logging;

using Naru.WPF.MVVM;
using Blitz.Client.Customer.ReportRunner;
using Blitz.Client.Customer.Reportviewer;

using Naru.WPF.TPL;

namespace Blitz.Client.Customer.Report
{
    [UseView(typeof(ReportView))]
    public class ReportViewModel : Common.Report.ReportViewModel
    {
        public ReportViewModel(ILog log, IViewService viewService, IScheduler scheduler)
            : base(log, viewService, scheduler)
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