﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicColumnManagement;

using Common.Logging;

using Naru.WPF.Command;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.DynamicReportData
{
    public class DynamicReportDataViewModel : Workspace
    {
        public BindableCollection<ExpandoObject> Items { get; private set; }

        public BindableCollection<DynamicColumn> Columns { get; private set; }

        public DynamicReportDataViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog, IViewService viewService,
                                          BindableCollection<ExpandoObject> itemsCollection,
                                          BindableCollection<DynamicColumn> columnsCollection,
                                          IToolBarService toolBarService, Func<DynamicColumnManagementViewModel> dynamicColumnManagementViewModelFactory)
            : base(log, scheduler, standardDialog)
        {
            Items = itemsCollection;
            Columns = columnsCollection;

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