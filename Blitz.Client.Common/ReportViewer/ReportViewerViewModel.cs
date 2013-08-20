using System.Collections.ObjectModel;

using Blitz.Client.Core;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportViewer
{
    [UseView(typeof(ReportViewerView))]
    public class ReportViewerViewModel<TReportViewerService, TRequest, TResponse> : Workspace
        where TReportViewerService : IReportViewerService<TRequest, TResponse>
    {
        private readonly TReportViewerService _reportViewerService;

        public ObservableCollection<ReportViewerItemViewModel> Items { get; private set; }

        public ReportViewerViewModel(ILog log, TReportViewerService reportViewerService, IDispatcherService dispatcherService)
            : base(log, dispatcherService)
        {
            _reportViewerService = reportViewerService;
            DisplayName = "Viewer";

            Items = new ObservableCollection<ReportViewerItemViewModel>();
        }

        protected override void OnInitialise()
        {
            BusyIndicatorSetAsync("... Loading ...")
                .Then(_ => _reportViewerService.GetHistory(_reportViewerService.CreateRequest()))
                .Then(response => _reportViewerService.GenerateItemViewModels(response))
                .ThenDo(dataViewModels => DispatcherService.ExecuteSyncOnUI(() =>
                {
                    foreach (var dataViewModel in dataViewModels)
                    {
                        Items.Add(dataViewModel);
                    }
                }))
                .LogException(Log)
                .Catch<RequestException>(x => { })
                .Finally(BusyIndicatorClear);
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