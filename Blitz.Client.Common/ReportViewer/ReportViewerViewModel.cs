using System.Collections.Generic;

using Blitz.Client.Common.ReportViewer.History;
using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Events;

namespace Blitz.Client.Common.ReportViewer
{
    [UseView(typeof(ReportViewerView))]
    public class ReportViewerViewModel<TReportViewerService, THistoryRequest, THistoryResponse, TReportRequest, TReportResponse> : Workspace
        where TReportViewerService : IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        private readonly TReportViewerService _reportViewerService;
        private readonly IViewService _viewService;
        private readonly HistoryViewModel _historyViewModel;

        private readonly IToolBarService _toolBarService;
        private readonly List<IToolBarItem> _toolBarItems;

        public ReportViewerViewModel(ILog log, TReportViewerService reportViewerService, IDispatcherService dispatcherService,
            IViewService viewService, IToolBarService toolBarService, HistoryViewModel historyViewModel)
            : base(log, dispatcherService)
        {
            _reportViewerService = reportViewerService;
            _viewService = viewService;
            _toolBarService = toolBarService;
            _historyViewModel = historyViewModel;

            DisplayName = "Viewer";

            _historyViewModel.Open += Open;
            Disposables.Add(new AnonymousDisposable(() => _historyViewModel.Open -= Open));

            _toolBarItems = new List<IToolBarItem>();
            _toolBarItems.ForEach(toolBarItem => _toolBarService.Items.Add(toolBarItem));
        }

        private void Open(object sender, DataEventArgs<long> e)
        {
            BusyAsync("... Opening Historic Report ...")
                .Then(_ => _reportViewerService.GenerateReport(_reportViewerService.CreateReportRequest(e.Value)))
                .Then(response => _reportViewerService.GenerateReportViewModels(response))
                .ThenDo(dataViewModels =>
                    DispatcherService.ExecuteSyncOnUI(() =>
                    {
                        _viewService.RegionBuilder().Clear(RegionNames.REPORT_DATA);
                        foreach (var dataViewModel in dataViewModels)
                        {
                            _viewService.RegionBuilder<IViewModel>().Show(RegionNames.HISTORY_DATA, dataViewModel);
                        }
                    }))
                .LogException(Log)
                .CatchAndHandle(x =>
                    DispatcherService.ExecuteSyncOnUI(
                        () => _viewService.StandardDialogBuilder().Error("Error", "Problem loading historic report")))
                .Finally(Idle);
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading ...")
                .Then(_ => _reportViewerService.GetHistory(_reportViewerService.CreateHistoryRequest()))
                .Then(response => _reportViewerService.GenerateHistoryItemViewModels(response))
                .ThenDo(dataViewModels =>
                    DispatcherService.ExecuteSyncOnUI(() =>
                    {
                        foreach (var dataViewModel in dataViewModels)
                        {
                            _historyViewModel.Items.Add(dataViewModel);
                        }

                        _viewService.RegionBuilder<HistoryViewModel>()
                            .Show(RegionNames.HISTORY_DATA, _historyViewModel);
                        ((ISupportActivationState)_historyViewModel).Activate();
                    }))
                .LogException(Log)
                .CatchAndHandle(x =>
                    DispatcherService.ExecuteSyncOnUI(
                        () => _viewService.StandardDialogBuilder().Error("Error", "Problem loading History")))
                .Finally(Idle);
        }

        protected override void OnActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = true;
            }

            _reportViewerService.OnActivate();
        }

        protected override void OnDeActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = false;
            }

            _reportViewerService.OnDeActivate();
        }

        protected override void CleanUp()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                _toolBarService.Items.Remove(toolBarItem);
            }

            _reportViewerService.CleanUp();
        }
    }
}