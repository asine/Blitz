using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;

using Blitz.Common.Customer;

using Naru.WPF.TPL;

namespace Blitz.Client.Customer.Reportviewer
{
    [UseView(typeof(ReportViewerView))]
    public class ReportViewerViewModel : ReportViewerViewModel<IReportViewerService, GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>
    {
        public ReportViewerViewModel(ILog log, IReportViewerService service, ITaskScheduler taskScheduler, IDispatcherService dispatcherService, IViewService viewService, 
            IToolBarService toolBarService, HistoryViewModel historyViewModel) 
            : base(log, service, taskScheduler, dispatcherService, viewService, toolBarService, historyViewModel)
        {
        }
    }
}