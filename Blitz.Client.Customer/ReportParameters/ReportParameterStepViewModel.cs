using System;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportParameter;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Validation;

namespace Blitz.Client.Customer.ReportParameters
{
    public class ReportParameterStepViewModel : ReportParameterWizardStepViewModel<ReportParameterContext>,
                                                ISupportValidation<ReportParameterStepViewModel, ReportParameterStepValidator>
    {
        private readonly IReportParameterStepService _service;

        public BindableCollection<DateTime> Dates { get; private set; }

        #region SelectedDate

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                RaisePropertyChanged(() => SelectedDate);

                Context.SelectedDate = SelectedDate;
            }
        }

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get { return this.Validate<ReportParameterStepValidator, ReportParameterStepViewModel>().GetError(); }
        }

        public string this[string columnName]
        {
            get
            {
                return this.Validate<ReportParameterStepValidator, ReportParameterStepViewModel>().GetErrorForProperty(columnName);
            }
        }

        #endregion

        public ReportParameterStepViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService,
                                            IReportParameterStepService service, 
                                            BindableCollection<DateTime> datesCollection)
            : base(log, scheduler, viewService)
        {
            _service = service;
            Dates = datesCollection;
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading available dates ...")
                .Then(_ => _service.GetAvailableDatesAsync(), Scheduler.TPL.Task)
                .Do(x => SelectedDate = x.First(), Scheduler.TPL.Dispatcher)
                .Then(x => Dates.AddRangeAsync(x), Scheduler.TPL.Dispatcher)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem available dates"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }

        protected override void LoadFromContext(ReportParameterContext context)
        {
            SelectedDate = context.SelectedDate;
        }
    }
}