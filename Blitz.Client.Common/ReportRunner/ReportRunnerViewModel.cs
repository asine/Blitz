using System.Collections.Generic;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
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

        private readonly IToolBarService _toolBarService;
        private readonly List<IToolBarItem> _toolBarItems;

        private readonly TReportParameterViewModel _reportParameterViewModel;
        private readonly TReportRunnerService _reportRunnerService;

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

        protected ReportRunnerViewModel(ILog log, IViewService viewService, IDispatcherService dispatcherService, IToolBarService toolBarService,
            TReportParameterViewModel reportParameterViewModel,
            TReportRunnerService reportRunnerService)
            : base(log, dispatcherService)
        {
            _viewService = viewService;
            _toolBarService = toolBarService;
            _reportParameterViewModel = reportParameterViewModel;
            _reportRunnerService = reportRunnerService;
            IsExpanded = true;

            DisplayName = "Runner";

            GenerateReportCommand = new DelegateCommand(GenerateReport, CanExecuteGenerateReport);

            _toolBarItems = new List<IToolBarItem> {CreateExportToExcelToolBarItem()};
            _toolBarItems.ForEach(toolBarItem => _toolBarService.Items.Add(toolBarItem));
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading ...")
                .Then(() => _reportRunnerService.ConfigureParameterViewModelAsync(_reportParameterViewModel))
                .ThenDo(() => _viewService.RegionBuilder<TReportParameterViewModel>()
                    .Show(RegionNames.REPORT_PARAMETER, _reportParameterViewModel))
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem initialising parameters"))
                .Finally(Idle);
        }

        private void GenerateReport()
        {
            BusyAsync("... Loading ...")
                .Then(_ => _reportRunnerService.GenerateAsync(_reportRunnerService.CreateRequest(_reportParameterViewModel)))
                .Then(response =>
                {
                    _response = response;
                    return _reportRunnerService.GenerateDataViewModelsAsync(response);
                })
                .ThenDo(dataViewModels =>
                    {
                        _viewService.RegionBuilder().Clear(RegionNames.REPORT_DATA);
                        foreach (var dataViewModel in dataViewModels)
                        {
                            _viewService.RegionBuilder<IViewModel>().Show(RegionNames.REPORT_DATA, dataViewModel);
                        }
                    })
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem Generating Report"))
                .Finally(() =>
                {
                    Idle();

                    IsExpanded = false;

                    _exportToExcel.RaiseCanExecuteChanged();
                });
        }

        protected virtual bool CanExecuteGenerateReport()
        {
            return true;
        }

        private ToolBarButtonItem CreateExportToExcelToolBarItem()
        {
            _exportToExcel = new DelegateCommand(ExportToExcel, CanExportToExcel);

            var exportToExcelToolBarItem = _toolBarService.CreateToolBarButtonItem();
            exportToExcelToolBarItem.DisplayName = "Excel Export";
            exportToExcelToolBarItem.Command = _exportToExcel;
            exportToExcelToolBarItem.IsVisible = false;
            exportToExcelToolBarItem.ImageName = IconNames.EXCEL;
            return exportToExcelToolBarItem;
        }

        private void ExportToExcel()
        {
            BusyAsync("... Exporting to Excel ...")
                .ThenDo(_ => _reportRunnerService.ExportToExcel(_response))
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem Exporting to Excel"))
                .Finally(() =>
                {
                    Idle();

                    IsExpanded = false;
                });
        }

        protected virtual bool CanExportToExcel()
        {
            return !(_response == null);
        }

        protected override void OnActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = true;
            }

            _reportRunnerService.OnActivate();
        }

        protected override void OnDeActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = false;
            }

            _reportRunnerService.OnDeActivate();
        }

        protected override void CleanUp()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                _toolBarService.Items.Remove(toolBarItem);
            }

            _reportRunnerService.CleanUp();
        }
    }
}