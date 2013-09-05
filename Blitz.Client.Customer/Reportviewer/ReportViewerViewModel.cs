using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Common.ReportViewer.History;
using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Common.Core;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer.Reportviewer
{
    [UseView(typeof(ReportViewerView))]
    public class ReportViewerViewModel : ReportViewerViewModel<ReportViewerService, GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>
    {
        public ReportViewerViewModel(ILog log, ReportViewerService reportViewerService, ITaskScheduler taskScheduler, IDispatcherService dispatcherService, IViewService viewService, 
            IToolBarService toolBarService, HistoryViewModel historyViewModel) 
            : base(log, reportViewerService, taskScheduler, dispatcherService, viewService, toolBarService, historyViewModel)
        {
        }
    }
}