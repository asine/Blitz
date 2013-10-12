using System;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;

using Microsoft.Practices.Prism.Events;

using Naru.WPF.Prism.Region;
using Naru.WPF.Scheduler;

namespace Blitz.Client.Common.ReportViewer
{
    public abstract class ReportViewerViewModel<TReportViewerService, THistoryRequest, THistoryResponse, TReportRequest, TReportResponse> : Workspace
        where TReportViewerService : IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        private readonly IRegionService _regionService;
        protected readonly TReportViewerService Service;
        private readonly HistoryViewModel _historyViewModel;

        protected readonly IToolBarService ToolBarService;

        protected ReportViewerViewModel(ILog log, IScheduler scheduler, IViewService viewService, IRegionService regionService, 
            TReportViewerService service, IToolBarService toolBarService, HistoryViewModel historyViewModel)
            : base(log, scheduler, viewService)
        {
            if (regionService == null) throw new ArgumentNullException("regionService");
            _regionService = regionService;
            Service = service;
            ToolBarService = toolBarService;
            _historyViewModel = historyViewModel;

            this.SetupHeader("Viewer");

            _historyViewModel.Open += Open;
            Disposables.Add(AnonymousDisposable.Create(() => _historyViewModel.Open -= Open));
        }

        private void Open(object sender, DataEventArgs<long> e)
        {
            BusyViewModel.ActiveAsync("... Opening Historic Report ...")
                .SelectMany(_ => Service.GenerateReportAsync(Service.CreateReportRequest(e.Value)))
                .SelectMany(response => Service.GenerateReportViewModelsAsync(response))
                .SelectMany(dataViewModels =>
                {
                    _regionService.RegionBuilder().Clear(RegionNames.HISTORY_DATA);
                    foreach (var dataViewModel in dataViewModels)
                    {
                        _regionService.RegionBuilder<IViewModel>().Show(RegionNames.HISTORY_DATA, dataViewModel);
                    }
                })
                .LogException(Log)
                .CatchAndHandle(_ => ViewService.StandardDialogBuilder().Error("Error", "Problem loading historic report"), Scheduler.Task)
                .Finally(BusyViewModel.InActive, Scheduler.Task);
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading ...")
                .SelectMany(_ => Service.GetHistoryAsync(Service.CreateHistoryRequest()))
                .SelectMany(response => Service.GenerateHistoryItemViewModelsAsync(response))
                .SelectMany(dataViewModels =>
                    {
                        foreach (var dataViewModel in dataViewModels)
                        {
                            _historyViewModel.Items.Add(dataViewModel);
                        }

                        _regionService.RegionBuilder<HistoryViewModel>().Show(RegionNames.HISTORY_DATA, _historyViewModel);
                        ((ISupportActivationState)_historyViewModel).Activate();
                    })
                .LogException(Log)
                .CatchAndHandle(_ => ViewService.StandardDialogBuilder().Error("Error", "Problem loading History"), Scheduler.Task)
                .Finally(BusyViewModel.InActive, Scheduler.Task);
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