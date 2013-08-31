using Blitz.Client.Common.ReportParameter.Simple;
using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer.ReportRunner
{
    [UseView(typeof(ReportRunnerView))]
    public class ReportRunnerViewModel : ReportRunnerViewModel<SimpleReportParameterViewModel, ReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerViewModel(ILog log, IViewService viewService, IDispatcherService dispatcherService, IToolBarService toolBarService, 
            SimpleReportParameterViewModel reportParameterViewModel, ReportRunnerService reportRunnerService) 
            : base(log, viewService, dispatcherService, toolBarService, reportParameterViewModel, reportRunnerService)
        {
        }
    }
}