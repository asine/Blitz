using Blitz.Client.Common.ReportParameter.Simple;
using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Common.Core;
using Blitz.Common.Customer;

namespace Blitz.Client.Employee.ReportRunner
{
    [UseView(typeof(ReportRunnerView))]
    public class ReportRunnerViewModel : ReportRunnerViewModel<SimpleReportParameterViewModel, ReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerViewModel(ILog log, IViewService viewService, ITaskScheduler taskScheduler, IDispatcherService dispatcherService, IToolBarService toolBarService,
            SimpleReportParameterViewModel reportParameterViewModel, ReportRunnerService reportRunnerService)
            : base(log, viewService, taskScheduler, dispatcherService, toolBarService, reportParameterViewModel, reportRunnerService)
        {
        }
    }
}