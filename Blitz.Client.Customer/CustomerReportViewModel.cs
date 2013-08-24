using Blitz.Client.Common.Report;
using Blitz.Client.Common.ReportParameter.Simple;
using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer
{
    [UseView(typeof(ReportView))]
    public class CustomerReportViewModel : ReportViewModel<
        ReportRunnerViewModel<SimpleReportParameterViewModel, CustomerReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>, 
        ReportViewerViewModel<CustomerReportViewerService, GetHistoryRequest, GetHistoryResponse>>
    {
        public CustomerReportViewModel(ILog log, IViewService viewService, IDispatcherService dispatcherService)
            : base(log, viewService, dispatcherService)
        {
        }
    }
}