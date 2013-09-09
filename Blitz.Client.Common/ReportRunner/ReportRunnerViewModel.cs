using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;
using Naru.WPF.ModernUI.Assets.Icons;

using Microsoft.Practices.Prism.Commands;

using Naru.WPF.TPL;

namespace Blitz.Client.Common.ReportRunner
{
    public abstract class ReportRunnerViewModel<TReportParameterViewModel, TReportRunnerService, TRequest, TResponse> : Workspace
        where TReportParameterViewModel : IViewModel
        where TReportRunnerService : IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        private readonly IViewService _viewService;
        private readonly IScheduler _scheduler;

        protected readonly IToolBarService ToolBarService;

        private readonly TReportParameterViewModel _reportParameterViewModel;
        protected readonly TReportRunnerService Service;

        private DelegateCommand _exportToExcel;
        private TResponse _response;

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

        protected ReportRunnerViewModel(ILog log, IViewService viewService, IScheduler scheduler, 
            IToolBarService toolBarService, TReportParameterViewModel reportParameterViewModel, TReportRunnerService service)
            : base(log, scheduler)
        {
            _viewService = viewService;
            _scheduler = scheduler;
            ToolBarService = toolBarService;
            _reportParameterViewModel = reportParameterViewModel;
            Service = service;
            IsExpanded = true;

            DisplayName = "Runner";

            GenerateReportCommand = new DelegateCommand(GenerateReport, CanExecuteGenerateReport);

            CreateExportToExcelToolBarItem();
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading ...")
                .SelectMany(() => Service.ConfigureParameterViewModelAsync(_reportParameterViewModel))
                .SelectMany(() => _viewService.RegionBuilder<TReportParameterViewModel>().ShowAsync(RegionNames.REPORT_PARAMETER, _reportParameterViewModel))
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem initialising parameters"), _scheduler.Default)
                .Finally(Idle, _scheduler.Default);
        }

        private void GenerateReport()
        {
            BusyAsync("... Loading ...")
                .SelectMany(_ => Service.GenerateAsync(Service.CreateRequest(_reportParameterViewModel)))
                .SelectMany(response =>
                {
                    _response = response;
                    return Service.GenerateDataViewModelsAsync(response);
                })
                .SelectMany(dataViewModels =>
                    {
                        _viewService.RegionBuilder().Clear(RegionNames.REPORT_DATA);
                        foreach (var dataViewModel in dataViewModels)
                        {
                            _viewService.RegionBuilder<IViewModel>().Show(RegionNames.REPORT_DATA, dataViewModel);

                            var supportActivationState = dataViewModel as ISupportActivationState;
                            if (supportActivationState != null)
                            {
                                this.SyncViewModelActivationStates(supportActivationState);
                            }
                        }
                    })
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem Generating Report"), _scheduler.Default)
                .Finally(() =>
                {
                    Idle();

                    IsExpanded = false;

                    _exportToExcel.RaiseCanExecuteChanged();
                }, _scheduler.Default);
        }

        protected virtual bool CanExecuteGenerateReport()
        {
            return true;
        }

        private void CreateExportToExcelToolBarItem()
        {
            _exportToExcel = new DelegateCommand(ExportToExcel, CanExportToExcel);

            var exportToExcelToolBarItem = ToolBarService.CreateToolBarButtonItem();
            exportToExcelToolBarItem.DisplayName = "Excel Export";
            exportToExcelToolBarItem.Command = _exportToExcel;
            exportToExcelToolBarItem.IsVisible = false;
            exportToExcelToolBarItem.ImageName = IconNames.EXCEL;

            this.SyncToolBarItemWithViewModelActivationState(exportToExcelToolBarItem);
        }

        private void ExportToExcel()
        {
            BusyAsync("... Exporting to Excel ...")
                .SelectMany(_ => Service.ExportToExcel(_response))
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem Exporting to Excel"), _scheduler.Default)
                .Finally(() =>
                {
                    Idle();

                    IsExpanded = false;
                }, _scheduler.Default);
        }

        protected virtual bool CanExportToExcel()
        {
            return !(_response == null);
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