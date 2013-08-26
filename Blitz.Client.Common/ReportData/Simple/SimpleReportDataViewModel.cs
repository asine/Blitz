using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportData.Simple
{
    public class SimpleReportDataViewModel : Workspace
    {
        public BindableCollection<object> Items { get; private set; }

        public SimpleReportDataViewModel(ILog log, IDispatcherService dispatcherService, BindableCollectionFactory bindableCollectionFactory)
            : base(log, dispatcherService)
        {
            Items = bindableCollectionFactory.Get<object>();
        }
    }
}