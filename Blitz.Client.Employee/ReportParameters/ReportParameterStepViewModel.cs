﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportParameter;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;

namespace Blitz.Client.Employee.ReportParameters
{
    public class ReportParameterStepViewModel : ReportParameterWizardStepViewModel<ReportParameterContext>
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
                if (value.Equals(_selectedDate)) return;
                _selectedDate = value;
                RaisePropertyChanged(() => SelectedDate);

                Context.SelectedDate = SelectedDate;
            }
        }

        #endregion

        public ReportParameterStepViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog,
                                            IReportParameterStepService service,
                                            BindableCollection<DateTime> datesCollection)
            : base(log, scheduler, standardDialog)
        {
            _service = service;
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
    }
}