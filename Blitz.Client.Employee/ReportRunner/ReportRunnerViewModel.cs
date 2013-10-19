using Blitz.Client.Common.ReportRunner;

using Common.Logging;

using Naru.WPF.MVVM;

using Blitz.Client.Employee.ReportParameters;
using Blitz.Common.Customer;

using Naru.WPF.Prism.Region;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;

namespace Blitz.Client.Employee.ReportRunner
{
    [UseView(typeof(ReportRunnerView))]
    public class ReportRunnerViewModel : ReportRunnerViewModel<ReportParameterViewModel, IReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerViewModel(ILog log, IViewService viewService, IRegionService regionService, ISchedulerProvider scheduler, IToolBarService toolBarService,
            ReportParameterViewModel reportParameterViewModel, IReportRunnerService service)
            : base(log, viewService, regionService, scheduler, toolBarService, reportParameterViewModel, service)
        {
        }
    }
}