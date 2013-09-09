using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;

using Microsoft.Practices.Prism.Events;

using Naru.WPF.TPL;

namespace Blitz.Client.Common.ReportViewer
{
    public abstract class ReportViewerViewModel<TReportViewerService, THistoryRequest, THistoryResponse, TReportRequest, TReportResponse> : Workspace
        where TReportViewerService : IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        protected readonly TReportViewerService Service;
        private readonly IScheduler _scheduler;
        private readonly IViewService _viewService;
        private readonly HistoryViewModel _historyViewModel;

        protected readonly IToolBarService ToolBarService;

        protected ReportViewerViewModel(ILog log, TReportViewerService service, IScheduler scheduler, 
            IViewService viewService, IToolBarService toolBarService, HistoryViewModel historyViewModel)
            : base(log, scheduler)
        {
            Service = service;
            _scheduler = scheduler;
            _viewService = viewService;
            ToolBarService = toolBarService;
            _historyViewModel = historyViewModel;

            DisplayName = "Viewer";

            _historyViewModel.Open += Open;
            Disposables.Add(AnonymousDisposable.Create(() => _historyViewModel.Open -= Open));
        }

        private void Open(object sender, DataEventArgs<long> e)
        {
            BusyAsync("... Opening Historic Report ...")
                .SelectMany(_ => Service.GenerateReportAsync(Service.CreateReportRequest(e.Value)))
                .SelectMany(response => Service.GenerateReportViewModelsAsync(response))
                .SelectMany(dataViewModels =>
                {
                    _viewService.RegionBuilder().Clear(RegionNames.HISTORY_DATA);
                    foreach (var dataViewModel in dataViewModels)
                    {
                        _viewService.RegionBuilder<IViewModel>().Show(RegionNames.HISTORY_DATA, dataViewModel);
                    }
                })
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading historic report"), _scheduler.Task)
                .Finally(Idle, _scheduler.Task);
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading ...")
                .SelectMany(_ => Service.GetHistoryAsync(Service.CreateHistoryRequest()))
                .SelectMany(response => Service.GenerateHistoryItemViewModelsAsync(response))
                .SelectMany(dataViewModels =>
                    {
                        foreach (var dataViewModel in dataViewModels)
                        {
                            _historyViewModel.Items.Add(dataViewModel);
                        }

                        _viewService.RegionBuilder<HistoryViewModel>().Show(RegionNames.HISTORY_DATA, _historyViewModel);
                        ((ISupportActivationState)_historyViewModel).Activate();
                    })
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading History"), _scheduler.Task)
                .Finally(Idle, _scheduler.Task);
        }

        protected override void OnActivate()
        {
            Service.OnActivate();
        }

        protected override void OnDeActivate()
        {
            Service.OnDeActivate();
        }

        protected override void CleanUp()
        {
            Service.CleanUp();
        }
    }
}