using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicColumnManagement;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Common.DynamicReportData
{
    public class DynamicReportDataViewModel : Workspace
    {
        public BindableCollection<ExpandoObject> Items { get; private set; }

        public BindableCollection<DynamicColumn> Columns { get; private set; }

        public DynamicReportDataViewModel(ILog log, IDispatcherService dispatcherService, BindableCollectionFactory bindableCollectionFactory,
                                          IToolBarService toolBarService, IViewService viewService, Func<DynamicColumnManagementViewModel> dynamicColumnManagementViewModelFactory)
            : base(log, dispatcherService)
        {
            Items = bindableCollectionFactory.Get<ExpandoObject>();
            Columns = bindableCollectionFactory.Get<DynamicColumn>();

            var columnEditToolBarItem = toolBarService.CreateToolBarButtonItem();
            columnEditToolBarItem.DisplayName = "Column Edit";
            columnEditToolBarItem.Command = new DelegateCommand(() =>
            {
                var viewModel = dynamicColumnManagementViewModelFactory();
                viewModel.Initialise(Columns);
                viewService.ShowModal(viewModel);
            });
            toolBarService.Items.Add(columnEditToolBarItem);

            Disposables.Add(this.SyncToolBarItemWithViewModelActivationState(columnEditToolBarItem));
        }

        public Task Initialise<T>(IEnumerable<T> items)
        {
            Columns.AddRange(DynamicColumnHelper.GetColumns<T>());

            return Items.AddRangeAsync(DynamicColumnHelper.ConvertToExpando(items));
        }
    }
}