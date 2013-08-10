using System.Collections.ObjectModel;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;

namespace Blitz.Client.Common.ReportData.Simple
{
    public class SimpleReportDataViewModel : ViewModelBase
    {
        public ObservableCollection<object> Items { get; private set; }

        public SimpleReportDataViewModel(ILog log)
            : base(log)
        {
            Items = new ObservableCollection<object>();
        }
    }
}