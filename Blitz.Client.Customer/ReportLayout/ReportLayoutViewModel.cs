using System;
using System.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.MVVM;

using Blitz.Common.Customer;

using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutViewModel : Workspace
    {
        private readonly IReportLayoutService _service;
        private readonly Func<ReportLayoutItemViewModel> _reportLayoutItemViewModelFactory;

        public BindableCollection<ReportLayoutItemViewModel> Available { get; private set; }

        public BindableCollection<ReportLayoutItemViewModel> Rows { get; private set; }

        public BindableCollection<ReportLayoutItemViewModel> Columns { get; private set; }

        public ReportLayoutDropTarget ToRowsDropTarget { get; private set; }

        public ReportLayoutDropTarget ToColumnsDropTarget { get; private set; }

        public ReportLayoutDropTarget ToAvailableDropTarget { get; private set; }

        public DelegateCommand OkCommand { get; private set; }

        public ReportLayoutViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService,
                                     IReportLayoutService service,
                                     BindableCollection<ReportLayoutItemViewModel> availableCollection,
                                     BindableCollection<ReportLayoutItemViewModel> rowsCollection,
                                     BindableCollection<ReportLayoutItemViewModel> columnsCollection,
                                     Func<ReportLayoutItemViewModel> reportLayoutItemViewModelFactory)
            : base(log, scheduler, viewService)
        {
            _service = service;
            _reportLayoutItemViewModelFactory = reportLayoutItemViewModelFactory;

            this.SetupHeader("Layout");

            Available = availableCollection;
            Rows = rowsCollection;
            Columns = columnsCollection;

            ToRowsDropTarget = new ReportLayoutDropTarget(x => x.Type == AttributeType.Dimension,
                                                          Rows, Available, Columns);

            ToColumnsDropTarget = new ReportLayoutDropTarget(x => x.Type == AttributeType.Measure,
                                                             Columns, Available, Rows);

            ToAvailableDropTarget = new ReportLayoutDropTarget(x => true,
                                                               Available, Columns, Rows);

            OkCommand = new DelegateCommand(Close);
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading Attributes ...")
                .Then(_ => _service.GetAttributesAsync(), Scheduler.TPL.Task)
                .Do(response => Available.AddRangeAsync(response.Dimensions.Select(CreateDimension)), Scheduler.TPL.Dispatcher)
                .Do(response => Available.AddRangeAsync(response.Measures.Select(CreateMeasure)), Scheduler.TPL.Dispatcher)
                .LogException(Log)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem initialising attributes"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }

        private ReportLayoutItemViewModel CreateDimension(AttributeDto dimension)
        {
            var item = _reportLayoutItemViewModelFactory();
            item.Name = dimension.Name;
            item.Type = AttributeType.Dimension;
            return item;
        }

        private ReportLayoutItemViewModel CreateMeasure(AttributeDto measure)
        {
            var item = _reportLayoutItemViewModelFactory();
            item.Name = measure.Name;
            item.Type = AttributeType.Measure;
            return item;
        }
    }
}