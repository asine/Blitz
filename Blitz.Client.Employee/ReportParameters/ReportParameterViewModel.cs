﻿using System;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Employee.ReportParameters
{
    public class ReportParameterViewModel : Workspace
    {
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
            }
        }

        #endregion

        public ReportParameterViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService, BindableCollectionFactory bindableCollectionFactory) 
            : base(log, scheduler, viewService)
        {
            Dates = bindableCollectionFactory.Get<DateTime>();
        }
    }
}