using System;
using System.Linq;

using Common.Logging;

using Naru.WPF.MVVM;

using Blitz.Common.Customer;

using Microsoft.Practices.Prism.Commands;

using Naru.WPF.TPL;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutViewModel : Workspace
    {
        private readonly ITaskScheduler _taskScheduler;
        private readonly IReportLayoutService _service;
        private readonly IViewService _viewService;
        private readonly Func<ReportLayoutItemViewModel> _reportLayoutItemViewModelFactory;

        public BindableCollection<ReportLayoutItemViewModel> Available { get; private set; }

        public BindableCollection<ReportLayoutItemViewModel> Rows { get; private set; }

        public BindableCollection<ReportLayoutItemViewModel> Columns { get; private set; }

        public ReportLayoutDropTarget ToRowsDropTarget { get; private set; }

        public ReportLayoutDropTarget ToColumnsDropTarget { get; private set; }

        public ReportLayoutDropTarget ToAvailableDropTarget { get; private set; }

        public DelegateCommand OkCommand { get; private set; }

        public ReportLayoutViewModel(ILog log, IDispatcherService dispatcherService, ITaskScheduler taskScheduler,
            IReportLayoutService service, IViewService viewService, BindableCollectionFactory bindableCollectionFactory, 
            Func<ReportLayoutItemViewModel> reportLayoutItemViewModelFactory)
            : base(log, dispatcherService)
        {
            _taskScheduler = taskScheduler;
            _service = service;
            _viewService = viewService;
            _reportLayoutItemViewModelFactory = reportLayoutItemViewModelFactory;

            DisplayName = "Layout";

            Available = bindableCollectionFactory.Get<ReportLayoutItemViewModel>();
            Rows = bindableCollectionFactory.Get<ReportLayoutItemViewModel>();
            Columns = bindableCollectionFactory.Get<ReportLayoutItemViewModel>();

            ToRowsDropTarget = new ReportLayoutDropTarget(x => x.Type == AttributeType.Dimension, Rows, Available, Columns);
            ToColumnsDropTarget = new ReportLayoutDropTarget(x => x.Type == AttributeType.Measure, Columns, Available, Rows);
            ToAvailableDropTarget = new ReportLayoutDropTarget(x => true, Available, Columns, Rows);

            OkCommand = new DelegateCommand(Close);
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading Attributes ...")
                .SelectMany(_ => _service.GetAttributesAsync())
                .Do(response => Available.AddRangeAsync(response.Dimensions.Select(CreateDimension)))
                .Do(response => Available.AddRangeAsync(response.Measures.Select(CreateMeasure)))
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem initialising attributes"), _taskScheduler.Default)
                .Finally(Idle, _taskScheduler.Default);
        }

        private ReportLayoutItemViewModel CreateDimension(AttributeDto dimension)
        {
            var item = _reportLayoutItemViewModelFactory();
            item.DisplayName = dimension.Name;
            item.Type = AttributeType.Dimension;
            return item;
        }

        private ReportLayoutItemViewModel CreateMeasure(AttributeDto measure)
        {
            var item = _reportLayoutItemViewModelFactory();
            item.DisplayName = measure.Name;
            item.Type = AttributeType.Measure;
            return item;
        }
    }
}