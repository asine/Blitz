﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicColumnManagement;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;

using Microsoft.Practices.Prism.Commands;

using Naru.WPF.TPL;

namespace Blitz.Client.Common.DynamicReportData
{
    public class DynamicReportDataViewModel : Workspace
    {
        public BindableCollection<ExpandoObject> Items { get; private set; }

        public BindableCollection<DynamicColumn> Columns { get; private set; }

        public DynamicReportDataViewModel(ILog log, IScheduler scheduler, BindableCollectionFactory bindableCollectionFactory,
                                          IToolBarService toolBarService, IViewService viewService, Func<DynamicColumnManagementViewModel> dynamicColumnManagementViewModelFactory)
            : base(log, scheduler)
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