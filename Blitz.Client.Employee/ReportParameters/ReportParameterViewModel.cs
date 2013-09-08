using System;

using Common.Logging;

using Naru.WPF.MVVM;

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

        public ReportParameterViewModel(ILog log, IDispatcherService dispatcherService, BindableCollectionFactory bindableCollectionFactory) 
            : base(log, dispatcherService)
        {
            Dates = bindableCollectionFactory.Get<DateTime>();
        }
    }
}