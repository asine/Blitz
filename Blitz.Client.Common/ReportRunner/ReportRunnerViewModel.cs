using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Client.ModernUI.Assets.Icons;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Common.ReportRunner
{
    public abstract class ReportRunnerViewModel<TReportParameterViewModel, TReportRunnerService, TRequest, TResponse> : Workspace
        where TReportParameterViewModel : IViewModel
        where TReportRunnerService : IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        private readonly IViewService _viewService;
        private readonly ITaskScheduler _taskScheduler;

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

        protected ReportRunnerViewModel(ILog log, IViewService viewService, ITaskScheduler taskScheduler, IDispatcherService dispatcherService, 
            IToolBarService toolBarService, TReportParameterViewModel reportParameterViewModel, TReportRunnerService service)
            : base(log, dispatcherService)
        {
            _viewService = viewService;
            _taskScheduler = taskScheduler;
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
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem initialising parameters"), _taskScheduler.Default)
                .Finally(Idle, _taskScheduler.Default);
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
                        }
                    })
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem Generating Report"), _taskScheduler.Default)
                .Finally(() =>
                {
                    Idle();

                    IsExpanded = false;

                    _exportToExcel.RaiseCanExecuteChanged();
                }, _taskScheduler.Default);
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
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem Exporting to Excel"), _taskScheduler.Default)
                .Finally(() =>
                {
                    Idle();

                    IsExpanded = false;
                }, _taskScheduler.Default);
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