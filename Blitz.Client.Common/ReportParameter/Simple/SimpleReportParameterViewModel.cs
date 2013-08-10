using System;
using System.Collections.ObjectModel;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;

namespace Blitz.Client.Common.ReportParameter.Simple
{
    public class SimpleReportParameterViewModel : ViewModelBase, IReportParameterViewModel
    {
        public ObservableCollection<DateTime> Dates { get; private set; } 

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

        public SimpleReportParameterViewModel(ILog log) 
            : base(log)
        {
            Dates = new ObservableCollection<DateTime>();
        }
    }
}