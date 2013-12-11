using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicColumnEdit;
using Blitz.Client.Common.DynamicReportData;

using Common.Logging;

using Naru.WPF.Command;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.DynamicColumnManagement
{
    public class DynamicColumnManagementViewModel : Workspace
    {
        private IDynamicColumnManagementService _service;
        private readonly Func<DynamicColumnEditViewModel> _editViewModelFactory;

        private DelegateCommand _saveCommand;

        public BindableCollection<IToolBarItem> ToolBarItems { get; private set; }

        public BindableCollection<DynamicColumn> Columns { get; private set; }

        #region SelectedColumn

        private DynamicColumn _selectedColumn;

        public DynamicColumn SelectedColumn
        {
            get { return _selectedColumn; }
            set
            {
                if (Equals(value, _selectedColumn)) return;
                _selectedColumn = value;
                RaisePropertyChanged(() => SelectedColumn);

                EditColumn(_selectedColumn);
            }
        }

        #endregion

        #region EditViewModel

        private DynamicColumnEditViewModel _editViewModel;

        public DynamicColumnEditViewModel EditViewModel
        {
            get { return _editViewModel; }
            set
            {
                if (Equals(value, _editViewModel)) return;
                _editViewModel = value;
                RaisePropertyChanged(() => EditViewModel);
            }
        }

        #endregion

        public DynamicColumnManagementViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog, IDynamicColumnManagementService service,
                                                BindableCollection<DynamicColumn> columnsCollection, 
                                                BindableCollection<IToolBarItem> toolBarItemsCollection,
                                                Func<DynamicColumnEditViewModel> editViewModelFactory,
                                                IToolBarService toolBarService)
            : base(log, scheduler, standardDialog)
        {
            _service = service;
            _editViewModelFactory = editViewModelFactory;

            Disposables.Add(service);

            Columns = columnsCollection;

            ToolBarItems = toolBarItemsCollection;

            var saveToolBarItem = toolBarService.CreateToolBarButtonItem();
            saveToolBarItem.DisplayName = "Save";
            _saveCommand = new DelegateCommand(() =>
            {
                ClosingStrategy.Close();
            });
            saveToolBarItem.Command = _saveCommand;
            ToolBarItems.Add(saveToolBarItem);

            var cancelToolBarItem = toolBarService.CreateToolBarButtonItem();
            cancelToolBarItem.DisplayName = "Cancel";
            cancelToolBarItem.Command = ClosingStrategy.CloseCommand;
            ToolBarItems.Add(cancelToolBarItem);
        }

        public Task Initialise(IEnumerable<DynamicColumn> columns)
        {
            return Columns.AddRangeAsync(columns);
        }

        private void EditColumn(DynamicColumn column)
        {
            var viewModel = _editViewModelFactory();
            viewModel.Initialise(column);

            EditViewModel = viewModel;
        }
    }
}