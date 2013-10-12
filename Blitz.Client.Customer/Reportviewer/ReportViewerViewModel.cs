using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;

using Blitz.Common.Customer;

using Naru.WPF.Prism.Region;
using Naru.WPF.Scheduler;

namespace Blitz.Client.Customer.Reportviewer
{
    [UseView(typeof(ReportViewerView))]
    public class ReportViewerViewModel : ReportViewerViewModel<IReportViewerService, GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>
    {
        public ReportViewerViewModel(ILog log, IScheduler scheduler, IViewService viewService, IRegionService regionService, IReportViewerService service, 
            IToolBarService toolBarService, HistoryViewModel historyViewModel) 
            : base(log, scheduler, viewService, regionService, service, toolBarService, historyViewModel)
        {
        }
    }
}