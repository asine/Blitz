using Blitz.Client.Common.ReportRunner;

using Common.Logging;

using Naru.WPF.Command;
using Naru.WPF.MVVM;

using Blitz.Client.Customer.ReportParameters;
using Blitz.Common.Customer;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer.ReportRunner
{
    [UseView(typeof(ReportRunnerView))]
    public class ReportRunnerViewModel : ReportRunnerViewModel<ReportParameterViewModel, IReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerViewModel(ILog log, IViewService viewService, ISchedulerProvider scheduler,
                                     IToolBarService toolBarService,
                                     ReportParameterViewModel reportParameterViewModel, IReportRunnerService service,
                                     BindableCollection<IViewModel> itemsCollection)
            : base(log, viewService, scheduler, toolBarService, reportParameterViewModel, service, itemsCollection)
        {
            CreateShowLayoutToolBarItem();
        }

        private void CreateShowLayoutToolBarItem()
        {
            var showLayoutToolBarItem = ToolBarService.CreateToolBarButtonItem();
            showLayoutToolBarItem.DisplayName = "Layout";
            showLayoutToolBarItem.Command = new DelegateCommand(() => Service.ShowLayoutAsync());
            showLayoutToolBarItem.IsVisible = false;

            ToolBarService.Items.Add(showLayoutToolBarItem);
            this.SyncToolBarItemWithViewModelActivationState(showLayoutToolBarItem);
        }
    }
}