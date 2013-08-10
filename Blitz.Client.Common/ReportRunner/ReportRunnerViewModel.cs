using System.Threading.Tasks;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Common.ReportRunner
{
    [UseView(typeof(ReportRunnerView))]
    public class ReportRunnerViewModel<TReportParameterViewModel, TReportRunnerService, TRequest, TResponse> : ViewModelBase
        where TReportParameterViewModel : IViewModel
        where TReportRunnerService : IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        private readonly IViewService _viewService;
        private readonly TReportParameterViewModel _reportParameterViewModel;
        private readonly TReportRunnerService _reportRunnerService;

        public DelegateCommand GenerateReportCommand { get; private set; }

        #region IsExpanded

        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value.Equals(_isExpanded)) return;
                _isExpanded = value;
                RaisePropertyChanged(() => IsExpanded);
            }
        }

        #endregion

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

        public ReportRunnerViewModel(ILog log, IViewService viewService,
            TReportParameterViewModel reportParameterViewModel,
            TReportRunnerService reportRunnerService)
            : base(log)
        {
            _viewService = viewService;
            _reportParameterViewModel = reportParameterViewModel;
            _reportRunnerService = reportRunnerService;
            IsExpanded = true;

            DisplayName = "Runner";

            GenerateReportCommand = new DelegateCommand(GenerateReport, CanExecuteGenerateReport);
        }

        protected override void OnInitialise()
        {
            _reportRunnerService.ConfigureParameterViewModel(_reportParameterViewModel)
                .ContinueWith(_ => IsBusy = false, TaskScheduler.FromCurrentSynchronizationContext());

            _viewService.AddToRegion(_reportParameterViewModel, RegionNames.REPORT_PARAMETER);
        }

        protected virtual bool CanExecuteGenerateReport()
        {
            return true;
        }

        private void GenerateReport()
        {
            IsBusy = true;

            var request = _reportRunnerService.CreateRequest(_reportParameterViewModel);

            var task = from response in _reportRunnerService.Generate(request)
                       from dataViewModels in _reportRunnerService.GenerateDataViewModels(response)
                       select dataViewModels;

            task
                .ContinueWith(x =>
                {
                    _viewService.ClearRegion(RegionNames.REPORT_DATA);

                    var dataViewModels = x.Result;

                    foreach (var dataViewModel in dataViewModels)
                    {
                        _viewService.AddToRegion(dataViewModel, RegionNames.REPORT_DATA);
                    }

                    IsBusy = false;
                    IsExpanded = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());

            task.Start();
        }
    }
}