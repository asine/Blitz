using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;
using Naru.WPF.ModernUI.Assets.Icons;

using Microsoft.Practices.Prism.Commands;

using Naru.WPF.Prism.Region;
using Naru.WPF.Scheduler;

namespace Blitz.Client.Common.ReportRunner
{
    public abstract class ReportRunnerViewModel<TReportParameterViewModel, TReportRunnerService, TRequest, TResponse> : Workspace
        where TReportParameterViewModel : IViewModel
        where TReportRunnerService : IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        private readonly IRegionService _regionService;
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

        protected ReportRunnerViewModel(ILog log, IViewService viewService, IRegionService regionService, IScheduler scheduler, 
            IToolBarService toolBarService, TReportParameterViewModel reportParameterViewModel, TReportRunnerService service)
            : base(log, scheduler, viewService)
        {
            _regionService = regionService;
            _scheduler = scheduler;
            ToolBarService = toolBarService;
            _reportParameterViewModel = reportParameterViewModel;
            Service = service;
            IsExpanded = true;

            this.SetupHeader("Runner", IconNames.EXCEL);

            GenerateReportCommand = new DelegateCommand(GenerateReport, CanExecuteGenerateReport);

            CreateExportToExcelToolBarItem();
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading ...")
                .SelectMany(() => Service.ConfigureParameterViewModelAsync(_reportParameterViewModel))
                .SelectMany(() => _regionService.RegionBuilder<TReportParameterViewModel>().ShowAsync(RegionNames.REPORT_PARAMETER, _reportParameterViewModel))
                .LogException(Log)
                .CatchAndHandle(x => ViewService.StandardDialogBuilder().Error("Error", "Problem initialising parameters"), _scheduler.Task)
                .Finally(BusyViewModel.InActive, _scheduler.Task);
        }

        private void GenerateReport()
        {
            BusyViewModel.ActiveAsync("... Loading ...")
                .SelectMany(_ => Service.GenerateAsync(Service.CreateRequest(_reportParameterViewModel)))
                .SelectMany(response =>
                {
                    _response = response;
                    return Service.GenerateDataViewModelsAsync(response);
                })
                .SelectMany(dataViewModels =>
                    {
                        _regionService.RegionBuilder().Clear(RegionNames.REPORT_DATA);
                        foreach (var dataViewModel in dataViewModels)
                        {
                            _regionService.RegionBuilder<IViewModel>().Show(RegionNames.REPORT_DATA, dataViewModel);

                            var supportActivationState = dataViewModel as ISupportActivationState;
                            if (supportActivationState != null)
                            {
                                this.SyncViewModelActivationStates(supportActivationState);
                            }
                        }
                    })
                .LogException(Log)
                .CatchAndHandle(x => ViewService.StandardDialogBuilder().Error("Error", "Problem Generating Report"), _scheduler.Task)
                .Finally(() =>
                {
                    BusyViewModel.InActive();

                    IsExpanded = false;

                    _exportToExcel.RaiseCanExecuteChanged();
                }, _scheduler.Task);
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
            BusyViewModel.ActiveAsync("... Exporting to Excel ...")
                .SelectMany(_ => Service.ExportToExcel(_response))
                .LogException(Log)
                .CatchAndHandle(x => ViewService.StandardDialogBuilder().Error("Error", "Problem Exporting to Excel"), _scheduler.Task)
                .Finally(() =>
                {
                    BusyViewModel.InActive();

                    IsExpanded = false;
                }, _scheduler.Task);
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