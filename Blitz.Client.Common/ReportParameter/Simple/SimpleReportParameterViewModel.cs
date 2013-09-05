using System;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportParameter.Simple
{
    public class SimpleReportParameterViewModel : Workspace
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

        public SimpleReportParameterViewModel(ILog log, IDispatcherService dispatcherService, BindableCollectionFactory bindableCollectionFactory) 
            : base(log, dispatcherService)
        {
            Dates = bindableCollectionFactory.Get<DateTime>();
        }
    }
}