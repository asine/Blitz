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
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Validation;

namespace Blitz.Client.Customer.ReportParameters
{
    public class ReportParameterStepViewModel : ReportParameterWizardStepViewModel<ReportParameterContext>,
                                                ISupportValidationAsync<ReportParameterStepViewModel, ReportParameterStepValidator>
    {
        private readonly IReportParameterStepService _service;
        private readonly IValidationAsync<ReportParameterStepViewModel, ReportParameterStepValidator> _validation;

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

        public ReportParameterStepViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                                            IReportParameterStepService service, 
                                            IValidationAsync<ReportParameterStepViewModel, ReportParameterStepValidator> validation,
                                            BindableCollection<DateTime> datesCollection)
            : base(log, scheduler, standardDialog)
        {
            _service = service;
            
            _validation = validation;
            _validation.Initialise(this);
            _validation.ErrorsChanged
                       .TakeUntil(ClosingStrategy.Closed)
                       .ObserveOn(Scheduler.Dispatcher.RX)
                       .Subscribe(x => ErrorsChanged.SafeInvoke(this, new DataErrorsChangedEventArgs(x)));

            Dates = datesCollection;
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading available dates ...")
                                .Then(() => _service.GetAvailableDatesAsync(), Scheduler.Task.TPL)
                                .Do(x => SelectedDate = x.First(), Scheduler.Dispatcher.TPL)
                                .Then(x => Dates.AddRangeAsync(x), Scheduler.Dispatcher.TPL)
                                .CatchAndHandle(_ => StandardDialog.Error("Error", "Problem available dates"), Scheduler.Task.TPL)
                                .Finally(BusyViewModel.InActive, Scheduler.Task.TPL);
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