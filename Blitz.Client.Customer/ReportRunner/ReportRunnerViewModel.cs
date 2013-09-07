using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Client.Customer.ReportParameters;
using Blitz.Common.Core;
using Blitz.Common.Customer;

using Microsoft.Practices.Prism.Commands;

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
            showLayoutToolBarItem.Command = new DelegateCommand(() =>Service.ShowLayout());
            showLayoutToolBarItem.IsVisible = false;

            ToolBarService.Items.Add(showLayoutToolBarItem);
            this.SyncToolBarItemWithViewModelActivationState(showLayoutToolBarItem);
        }
    }
}