using System;
using System.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
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

        public ReportLayoutViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                                     IReportLayoutService service,
                                     BindableCollection<ReportLayoutItemViewModel> availableCollection,
                                     BindableCollection<ReportLayoutItemViewModel> rowsCollection,
                                     BindableCollection<ReportLayoutItemViewModel> columnsCollection,
                                     Func<ReportLayoutItemViewModel> reportLayoutItemViewModelFactory)
            : base(log, scheduler, standardDialog)
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
                .Then(() => _service.GetAttributesAsync(), Scheduler.Task.TPL)
                .Do(response => Available.AddRangeAsync(response.Dimensions.Select(CreateDimension)), Scheduler.Dispatcher.TPL)
                .Do(response => Available.AddRangeAsync(response.Measures.Select(CreateMeasure)), Scheduler.Dispatcher.TPL)
                .LogException(Log)
                .CatchAndHandle(_ => StandardDialog.Error("Error", "Problem initialising attributes"), Scheduler.Task.TPL)
                .Finally(BusyViewModel.InActive, Scheduler.Task.TPL);
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