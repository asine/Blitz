using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicColumnEdit;
using Blitz.Client.Common.DynamicReportData;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

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

        public DynamicColumnManagementViewModel(ILog log, IDispatcherService dispatcherService, IDynamicColumnManagementService service,
                                                BindableCollectionFactory bindableCollectionFactory, Func<DynamicColumnEditViewModel> editViewModelFactory,
                                                IToolBarService toolBarService)
            : base(log, dispatcherService)
        {
            _service = service;
            _editViewModelFactory = editViewModelFactory;

            Disposables.Add(service);

            Columns = bindableCollectionFactory.Get<DynamicColumn>();

            ToolBarItems = bindableCollectionFactory.Get<IToolBarItem>();

            var saveToolBarItem = toolBarService.CreateToolBarButtonItem();
            saveToolBarItem.DisplayName = "Save";
            _saveCommand = new DelegateCommand(() =>
            {
                
                Close();
            });
            saveToolBarItem.Command = _saveCommand;
            ToolBarItems.Add(saveToolBarItem);

            var cancelToolBarItem = toolBarService.CreateToolBarButtonItem();
            cancelToolBarItem.DisplayName = "Cancel";
            cancelToolBarItem.Command = ClosingCommand;
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