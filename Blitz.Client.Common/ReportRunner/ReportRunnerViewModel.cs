using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportParameter;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.MVVM;
using Naru.WPF.ModernUI.Assets.Icons;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.ReportRunner
{
    public abstract class ReportRunnerViewModel<TReportParameterViewModel, TReportRunnerService, TRequest, TResponse> : Workspace
        where TReportParameterViewModel : IReportParameterViewModel
        where TReportRunnerService : IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        protected readonly IToolBarService ToolBarService;

        protected readonly TReportRunnerService Service;

        private DelegateCommand _exportToExcel;
        private TResponse _response;

        public BindableCollection<IViewModel> Items { get; private set; }

        #region ParameterViewModel

        private TReportParameterViewModel _parameterViewModel;

        public TReportParameterViewModel ParameterViewModel
        {
            get { return _parameterViewModel; }
            private set
            {
                _parameterViewModel = value;
                RaisePropertyChanged(() => ParameterViewModel);
            }
        }

        #endregion

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

        protected ReportRunnerViewModel(ILog log, IViewService viewService, ISchedulerProvider scheduler, 
                                        IToolBarService toolBarService, TReportParameterViewModel reportParameterViewModel,
                                        TReportRunnerService service, BindableCollection<IViewModel> itemsCollection)
            : base(log, scheduler, viewService)
        {
            ToolBarService = toolBarService;
            Service = service;
            IsExpanded = true;
            Items = itemsCollection;

            ParameterViewModel = reportParameterViewModel;
            this.SyncViewModelActivationStates(ParameterViewModel);

            this.SetupHeader("Runner", IconNames.EXCEL);

            ParameterViewModel.GenerateReport
                .TakeUntil(Closed)
                .Subscribe(_ => GenerateReport());

            CreateExportToExcelToolBarItem();
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading ...")
                .Then(() => Service.ConfigureParameterViewModelAsync(ParameterViewModel), Scheduler.TPL.Task)
                .LogException(Log)
                .CatchAndHandle(x => ViewService.StandardDialog().Error("Error", "Problem initialising parameters"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }

        private void GenerateReport()
        {
            BusyViewModel.ActiveAsync("... Generating ...")
                .Then(() => Items.ClearAsync(), Scheduler.TPL.Dispatcher)
                .Then(() => Service.GenerateAsync(Service.CreateRequest(ParameterViewModel)), Scheduler.TPL.Task)
                .Do(response => _response = response, Scheduler.TPL.Task)
                .Then(response => Service.GenerateDataViewModelsAsync(response), Scheduler.TPL.Task)
                .Then(dataViewModels =>
                    {
                        foreach (var dataViewModel in dataViewModels)
                        {
                            Items.Add(dataViewModel);

                            var supportActivationState = dataViewModel as ISupportActivationState;
                            if (supportActivationState != null)
                            {
                                this.SyncViewModelActivationStates(supportActivationState);
                            }
                        }
                    }, Scheduler.TPL.Dispatcher)
                .LogException(Log)
                .CatchAndHandle(x => ViewService.StandardDialog().Error("Error", "Problem Generating Report"), Scheduler.TPL.Task)
                .Finally(() =>
                {
                    BusyViewModel.InActive();

                    IsExpanded = false;

                    _exportToExcel.RaiseCanExecuteChanged();
                }, Scheduler.TPL.Task);
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
                .Then(_ => Service.ExportToExcel(_response), Scheduler.TPL.Task)
                .LogException(Log)
                .CatchAndHandle(x => ViewService.StandardDialog().Error("Error", "Problem Exporting to Excel"), Scheduler.TPL.Task)
                .Finally(() =>
                {
                    BusyViewModel.InActive();

                    IsExpanded = false;
                }, Scheduler.TPL.Task);
        }

        protected virtual bool CanExportToExcel()
        {
            return !(_response == null);
        }

        protected override void CleanUp()
        {
            Service.Dispose();
        }
    }
}