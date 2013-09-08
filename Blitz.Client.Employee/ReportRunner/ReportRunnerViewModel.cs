using Blitz.Client.Common.ReportRunner;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;

using Blitz.Client.Employee.ReportParameters;
using Blitz.Common.Customer;

using Naru.WPF.TPL;

namespace Blitz.Client.Employee.ReportRunner
{
    [UseView(typeof(ReportRunnerView))]
    public class ReportRunnerViewModel : ReportRunnerViewModel<ReportParameterViewModel, IReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerViewModel(ILog log, IViewService viewService, ITaskScheduler taskScheduler, IDispatcherService dispatcherService, IToolBarService toolBarService,
            ReportParameterViewModel reportParameterViewModel, IReportRunnerService service)
            : base(log, viewService, taskScheduler, dispatcherService, toolBarService, reportParameterViewModel, service)
        {
        }
    }
}