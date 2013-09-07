using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Client.Employee.ReportParameters;
using Blitz.Common.Core;
using Blitz.Common.Customer;

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