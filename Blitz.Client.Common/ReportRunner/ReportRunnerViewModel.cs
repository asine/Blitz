using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.Dialog;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Common.ReportRunner
{
    [UseView(typeof(ReportRunnerView))]
    public class ReportRunnerViewModel<TReportParameterViewModel, TReportRunnerService, TRequest, TResponse> : Workspace
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

        public ReportRunnerViewModel(ILog log, IViewService viewService, IDispatcherService dispatcherService,
            TReportParameterViewModel reportParameterViewModel,
            TReportRunnerService reportRunnerService)
            : base(log, dispatcherService)
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
            BusyAsync("... Loading ...")
                .Then(() => _reportRunnerService.ConfigureParameterViewModel(_reportParameterViewModel))
                .ThenDo(() =>
                    DispatcherService.ExecuteSyncOnUI(() =>
                        _viewService.RegionBuilder<TReportParameterViewModel>()
                            .Show(RegionNames.REPORT_PARAMETER, _reportParameterViewModel)))
                .LogException(Log)
                .CatchAndHandle(x =>
                    DispatcherService.ExecuteSyncOnUI(
                        () => _viewService.StandardDialogBuilder().Error("Error", "Problem initialising parameters")))
                .Finally(Idle);
        }

        protected virtual bool CanExecuteGenerateReport()
        {
            return true;
        }

        private void GenerateReport()
        {
            BusyAsync("... Loading ...")
                .Then(_ => _reportRunnerService.Generate(_reportRunnerService.CreateRequest(_reportParameterViewModel)))
                .Then(response => _reportRunnerService.GenerateDataViewModels(response))
                .ThenDo(dataViewModels =>
                    DispatcherService.ExecuteSyncOnUI(() =>
                    {
                        _viewService.RegionBuilder().Clear(RegionNames.REPORT_DATA);
                        foreach (var dataViewModel in dataViewModels)
                        {
                            _viewService.RegionBuilder<IViewModel>().Show(RegionNames.REPORT_DATA, dataViewModel);
                        }
                    }))
                .LogException(Log)
                .CatchAndHandle(x =>
                    DispatcherService.ExecuteSyncOnUI(
                        () => _viewService.StandardDialogBuilder().Error("Error", "Problem Generating Report")))
                .Finally(() =>
                {
                    Idle();

                    IsExpanded = false;
                });
        }

        protected override void OnActivate()
        {
            _reportRunnerService.OnActivate();
        }

        protected override void OnDeActivate()
        {
            _reportRunnerService.OnDeActivate();
        }

        protected override void CleanUp()
        {
            _reportRunnerService.CleanUp();
        }
    }
}