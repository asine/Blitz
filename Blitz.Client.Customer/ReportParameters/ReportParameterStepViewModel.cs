using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportParameter;

using Common.Logging;

using Naru.Core;
using Naru.TPL;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Validation;

namespace Blitz.Client.Customer.ReportParameters
{
    public class ReportParameterStepViewModel : ReportParameterWizardStepViewModel<ReportParameterContext>,
                                                ISupportValidationAsync<ReportParameterStepViewModel, ReportParameterStepValidator>
    {
        private readonly IReportParameterStepService _service;
        private readonly ValidationAsync<ReportParameterStepViewModel, ReportParameterStepValidator> _validation;

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

                _validation.ValidateProperty(() => SelectedDate);
            }
        }

        #endregion

        public ReportParameterStepViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService,
                                            IReportParameterStepService service, 
                                            ValidationAsync<ReportParameterStepViewModel, ReportParameterStepValidator> validation,
                                            BindableCollection<DateTime> datesCollection)
            : base(log, scheduler, viewService)
        {
            _service = service;
            
            _validation = validation;
            _validation.Initialise(this);
            _validation.ErrorsChanged
                       .TakeUntil(Closed)
                       .ObserveOn(Scheduler.RX.Dispatcher)
                       .Subscribe(x => ErrorsChanged.SafeInvoke(this, new DataErrorsChangedEventArgs(x)));

            Dates = datesCollection;
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel
                .ActiveAsync("... Loading available dates ...")
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

        #region Validation

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _validation.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get { return _validation.HasErrors; }
        }

        #endregion
    }
}