using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportViewer
{
    [UseView(typeof(ReportViewerView))]
    public class ReportViewerViewModel<TReportViewerService, TRequest, TResponse> : ViewModelBase
        where TReportViewerService : IReportViewerService<TRequest, TResponse>
    {
        private readonly TReportViewerService _reportViewerService;

        public ObservableCollection<ReportViewerItemViewModel> Items { get; private set; }

        #region IsBusy

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        #endregion

        public ReportViewerViewModel(ILog log, TReportViewerService reportViewerService)
            : base(log)
        {
            _reportViewerService = reportViewerService;
            DisplayName = "Viewer";

            Items = new ObservableCollection<ReportViewerItemViewModel>();
        }

        protected override void OnInitialise()
        {
            IsBusy = true;

            var request = _reportViewerService.CreateRequest();

            var task = from response in _reportViewerService.GetHistory(request)
                       from dataViewModels in _reportViewerService.GenerateDataViewModels(response)
                       select dataViewModels;
            task
                .ContinueWith(x =>
                {
                    var dataViewModels = x.Result;

                    foreach (var dataViewModel in dataViewModels)
                    {
                        Items.Add(dataViewModel);
                    }

                    IsBusy = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());

            task.Start();
        }
    }
}