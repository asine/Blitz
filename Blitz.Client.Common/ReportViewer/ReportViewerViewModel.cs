using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using Blitz.Client.Core;
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

        public ReportViewerViewModel(ILog log, TReportViewerService reportViewerService)
            : base(log)
        {
            _reportViewerService = reportViewerService;
            DisplayName = "Viewer";

            Items = new ObservableCollection<ReportViewerItemViewModel>();
        }

        protected override void OnInitialise()
        {
            BusyIndicatorSet("... Loading ...");

            var task = from response in _reportViewerService.GetHistory(_reportViewerService.CreateRequest())
                       from itemViewModels in _reportViewerService.GenerateItemViewModels(response)
                       select itemViewModels;
            task
                .ContinueWith(x =>
                {
                    var dataViewModels = x.Result;

                    foreach (var dataViewModel in dataViewModels)
                    {
                        Items.Add(dataViewModel);
                    }

                    BusyIndicatorClear();
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

            task
                .ContinueWith(t =>
                {
                    if (t.Exception != null)
                        Log.Error(t.Exception.Flatten().InnerException);

                    BusyIndicatorClear();
                }, TaskContinuationOptions.NotOnRanToCompletion);

            task.Start();
        }
    }
}