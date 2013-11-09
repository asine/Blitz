using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.Core;
using Naru.TPL;
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

        protected ReportViewerViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService, 
                                        TReportViewerService service, IToolBarService toolBarService,
                                        HistoryViewModel historyViewModel, BindableCollection<IViewModel> itemsCollection)
            : base(log, scheduler, viewService)
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
                .Then(() => Items.ClearAsync(), Scheduler.TPL.Dispatcher)
                .Then(() => Service.GenerateReportAsync(Service.CreateReportRequest(e.Value)), Scheduler.TPL.Task)
                .Then(response => Service.GenerateReportViewModelsAsync(response), Scheduler.TPL.Task)
                .Then(dataViewModels => Items.AddRangeAsync(dataViewModels), Scheduler.TPL.Dispatcher)
                .LogException(Log)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem loading historic report"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading ...")
                .Then(() => Items.ClearAsync(), Scheduler.TPL.Dispatcher)
                .Then(() => _historyViewModel.Items.ClearAsync(), Scheduler.TPL.Dispatcher)
                .Then(() => Service.GetHistoryAsync(Service.CreateHistoryRequest()), Scheduler.TPL.Task)
                .Then(response => Service.GenerateHistoryItemViewModelsAsync(response), Scheduler.TPL.Task)
                .Then(dataViewModels =>
                    {
                        _historyViewModel.Items.AddRange(dataViewModels);

                        Items.Add(_historyViewModel);
                        ((ISupportActivationState)_historyViewModel).Activate();
                    }, Scheduler.TPL.Dispatcher)
                .LogException(Log)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem loading History"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }
    }
}