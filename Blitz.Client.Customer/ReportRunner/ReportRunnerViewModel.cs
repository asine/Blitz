using Blitz.Client.Common.ReportRunner;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;

using Blitz.Client.Customer.ReportParameters;
using Blitz.Common.Customer;

using Microsoft.Practices.Prism.Commands;

using Naru.WPF.TPL;

namespace Blitz.Client.Customer.ReportRunner
{
    [UseView(typeof(ReportRunnerView))]
    public class ReportRunnerViewModel : ReportRunnerViewModel<ReportParameterViewModel, IReportRunnerService, ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerViewModel(ILog log, IViewService viewService, ITaskScheduler taskScheduler, IDispatcherService dispatcherService, IToolBarService toolBarService, 
            ReportParameterViewModel reportParameterViewModel, IReportRunnerService service) 
            : base(log, viewService, taskScheduler, dispatcherService, toolBarService, reportParameterViewModel, service)
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