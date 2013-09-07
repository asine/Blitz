﻿using Blitz.Client.Common.ReportViewer.History;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Events;

namespace Blitz.Client.Common.ReportViewer
{
    public abstract class ReportViewerViewModel<TReportViewerService, THistoryRequest, THistoryResponse, TReportRequest, TReportResponse> : Workspace
        where TReportViewerService : IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        protected readonly TReportViewerService Service;
        private readonly ITaskScheduler _taskScheduler;
        private readonly IViewService _viewService;
        private readonly HistoryViewModel _historyViewModel;

        protected readonly IToolBarService ToolBarService;

        protected ReportViewerViewModel(ILog log, TReportViewerService service, ITaskScheduler taskScheduler, IDispatcherService dispatcherService,
            IViewService viewService, IToolBarService toolBarService, HistoryViewModel historyViewModel)
            : base(log, dispatcherService)
        {
            Service = service;
            _taskScheduler = taskScheduler;
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
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading historic report"), _taskScheduler.Default)
                .Finally(Idle, _taskScheduler.Default);
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

                        _viewService.RegionBuilder<HistoryViewModel>()
                            .Show(RegionNames.HISTORY_DATA, _historyViewModel);
                        ((ISupportActivationState)_historyViewModel).Activate();
                    })
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading History"), _taskScheduler.Default)
                .Finally(Idle, _taskScheduler.Default);
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