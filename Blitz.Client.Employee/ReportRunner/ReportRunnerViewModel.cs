using Blitz.Client.Common.ReportRunner;

using Common.Logging;

using Naru.WPF.MVVM;

using Blitz.Client.Employee.ReportParameters;
using Blitz.Common.Customer;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Employee.ReportRunner
{
    [UseView(typeof (ReportRunnerView))]
    public class ReportRunnerViewModel : ReportRunnerViewModel<ReportParameterViewModel, IReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerViewModel(ILog log, IViewService viewService, ISchedulerProvider scheduler,
                                     IToolBarService toolBarService,
                                     ReportParameterViewModel reportParameterViewModel, IReportRunnerService service,
                                     BindableCollection<IViewModel> itemsCollection)
            : base(log, viewService, scheduler, toolBarService, reportParameterViewModel, service, itemsCollection)
        {
        }
    }
}