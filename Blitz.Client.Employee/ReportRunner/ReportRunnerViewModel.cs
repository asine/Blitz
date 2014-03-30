using Blitz.Client.Common.ReportRunner;

using Common.Logging;

using Naru.WPF.Dialog;
using Naru.WPF.MVVM;

using Blitz.Client.Employee.ReportParameters;
using Blitz.Common.Customer;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Employee.ReportRunner
{
    public class ReportRunnerViewModel : ReportRunnerViewModel<ReportParameterViewModel, IReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerViewModel(ILog log, IStandardDialog standardDialog, IDispatcherSchedulerProvider scheduler,
                                     IToolBarService toolBarService,
                                     ReportParameterViewModel reportParameterViewModel, IReportRunnerService service,
                                     BindableCollection<IViewModel> itemsCollection)
            : base(log, standardDialog, scheduler, toolBarService, reportParameterViewModel, service, itemsCollection)
        {
        }
    }
}