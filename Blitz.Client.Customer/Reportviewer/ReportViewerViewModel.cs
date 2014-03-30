using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.WPF.Dialog;
using Naru.WPF.MVVM;

using Blitz.Common.Customer;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer.Reportviewer
{
    public class ReportViewerViewModel :
        ReportViewerViewModel <IReportViewerService, GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>
    {
        public ReportViewerViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog,
                                     IReportViewerService service, IToolBarService toolBarService,
                                     HistoryViewModel historyViewModel, BindableCollection<IViewModel> itemsCollection)
            : base(log, scheduler, standardDialog, service, toolBarService, historyViewModel, itemsCollection)
        {
        }
    }
}