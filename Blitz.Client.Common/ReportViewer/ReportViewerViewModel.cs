using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.Core;
using Naru.TPL;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.ReportViewer
{
    public abstract class ReportViewerViewModel<TReportViewerService, THistoryRequest, THistoryResponse, TReportRequest, TReportResponse> : Workspace
        where TReportViewerService : IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        protected readonly TReportViewerService Service;
        private readonly HistoryViewModel _historyViewModel;

        protected readonly IToolBarService ToolBarService;

        public BindableCollection<IViewModel> Items { get; private set; }

        protected ReportViewerViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog, 
                                        TReportViewerService service, IToolBarService toolBarService,
                                        HistoryViewModel historyViewModel, BindableCollection<IViewModel> itemsCollection)
            : base(log, scheduler, standardDialog)
        {
            Service = service;
            ToolBarService = toolBarService;
            _historyViewModel = historyViewModel;

            Items = itemsCollection;

            this.SetupHeader("Viewer");

            _historyViewModel.Open += Open;
            Disposables.Add(AnonymousDisposable.Create(() => _historyViewModel.Open -= Open));
        }

        private void Open(object sender, DataEventArgs<long> e)
        {
            BusyViewModel.ActiveAsync("... Opening Historic Report ...")
                .Then(() => Items.ClearAsync(), Scheduler.Dispatcher.TPL)
                .Then(() => Service.GenerateReportAsync(Service.CreateReportRequest(e.Value)), Scheduler.Task.TPL)
                .Then(response => Service.GenerateReportViewModelsAsync(response), Scheduler.Task.TPL)
                .Then(dataViewModels => Items.AddRangeAsync(dataViewModels), Scheduler.Dispatcher.TPL)
                .LogException(Log)
                .CatchAndHandle(_ => StandardDialog.Error("Error", "Problem loading historic report"), Scheduler.Task.TPL)
                .Finally(BusyViewModel.InActive, Scheduler.Task.TPL);
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading ...")
                .Then(() => Items.ClearAsync(), Scheduler.Dispatcher.TPL)
                .Then(() => _historyViewModel.Items.ClearAsync(), Scheduler.Dispatcher.TPL)
                .Then(() => Service.GetHistoryAsync(Service.CreateHistoryRequest()), Scheduler.Task.TPL)
                .Then(response => Service.GenerateHistoryItemViewModelsAsync(response), Scheduler.Task.TPL)
                .Then(dataViewModels =>
                    {
                        _historyViewModel.Items.AddRange(dataViewModels);

                        Items.Add(_historyViewModel);
                        ((ISupportActivationState)_historyViewModel).Activate();
                    }, Scheduler.Dispatcher.TPL)
                .LogException(Log)
                .CatchAndHandle(_ => StandardDialog.Error("Error", "Problem loading History"), Scheduler.Task.TPL)
                .Finally(BusyViewModel.InActive, Scheduler.Task.TPL);
        }
    }
}