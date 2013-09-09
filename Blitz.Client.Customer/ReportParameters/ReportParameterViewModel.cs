using System;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.TPL;

namespace Blitz.Client.Customer.ReportParameters
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

        public ReportParameterViewModel(ILog log, IScheduler scheduler, BindableCollectionFactory bindableCollectionFactory)
            : base(log, scheduler)
        {
            Dates = bindableCollectionFactory.Get<DateTime>();
        }
    }
}