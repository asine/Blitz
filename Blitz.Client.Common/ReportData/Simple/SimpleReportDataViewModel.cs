using System.Collections.ObjectModel;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportData.Simple
{
    public class SimpleReportDataViewModel : Workspace
    {
        public ObservableCollection<object> Items { get; private set; }

        public SimpleReportDataViewModel(ILog log)
            : base(log)
        {
            Items = new ObservableCollection<object>();
        }
    }
}