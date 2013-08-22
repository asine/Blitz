using System.Collections.ObjectModel;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.Dialog;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportViewer
{
    [UseView(typeof(ReportViewerView))]
    public class ReportViewerViewModel<TReportViewerService, TRequest, TResponse> : Workspace
        where TReportViewerService : IReportViewerService<TRequest, TResponse>
    {
        private readonly TReportViewerService _reportViewerService;
        private readonly IViewService _viewService;

        public ObservableCollection<ReportViewerItemViewModel> Items { get; private set; }

        public ReportViewerViewModel(ILog log, TReportViewerService reportViewerService, IDispatcherService dispatcherService, IViewService viewService)
            : base(log, dispatcherService)
        {
            _reportViewerService = reportViewerService;
            _viewService = viewService;
            DisplayName = "Viewer";

            Items = new ObservableCollection<ReportViewerItemViewModel>();
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading ...")
                .Then(_ => _reportViewerService.GetHistory(_reportViewerService.CreateRequest()))
                .Then(response => _reportViewerService.GenerateItemViewModels(response))
                .ThenDo(dataViewModels =>
                    DispatcherService.ExecuteSyncOnUI(() =>
                    {
                        foreach (var dataViewModel in dataViewModels)
                        {
                            Items.Add(dataViewModel);
                        }
                    }))
                .LogException(Log)
                .CatchAndHandle(x =>
                    DispatcherService.ExecuteSyncOnUI(() =>
                        _viewService.DialogBuilder()
                            .WithDialogType(DialogType.Error)
                            .WithAnswers(Answer.Ok)
                            .WithTitle("Error")
                            .WithMessage("Problem loading History")
                            .Show())
                )
                .Finally(Idle);
        }

        protected override void OnActivate()
        {
            _reportViewerService.OnActivate();
        }

        protected override void OnDeActivate()
        {
            _reportViewerService.OnDeActivate();
        }

        protected override void CleanUp()
        {
            _reportViewerService.CleanUp();
        }
    }
}