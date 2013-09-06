using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportData.Dynamic
{
    public class DynamicReportDataViewModel : Workspace
    {
        public BindableCollection<ExpandoObject> Items { get; private set; }

        public BindableCollection<DynamicColumn> Columns { get; private set; }

        public DynamicReportDataViewModel(ILog log, IDispatcherService dispatcherService, BindableCollectionFactory bindableCollectionFactory)
            : base(log, dispatcherService)
        {
            Items = bindableCollectionFactory.Get<ExpandoObject>();
            Columns = bindableCollectionFactory.Get<DynamicColumn>();
        }

        public Task Initialise<T>(IEnumerable<T> items, IEnumerable<DynamicColumn> columns)
        {
            Columns.AddRange(DynamicColumnHelper.GetColumns<T>());

            return Items.AddRangeAsync(DynamicColumnHelper.ConvertToExpando(items));
        }
    }
}