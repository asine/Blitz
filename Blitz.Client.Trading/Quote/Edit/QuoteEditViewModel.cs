using System;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Common.Core;
using Blitz.Common.Trading.Quote.Edit;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Trading.Quote.Edit
{
    public class QuoteEditViewModel : Workspace
    {
        private readonly ITaskScheduler _taskScheduler;
        private readonly IViewService _viewService;
        private readonly IQuoteEditService _service;

        private Guid? _id;

        public BindableCollection<IToolBarItem> ToolBarItems { get; private set; }

        #region Model

        private QuoteModel _model;

        public QuoteModel Model
        {
            get { return _model; }
            private set
            {
                if (Equals(value, _model)) return;
                _model = value;
                RaisePropertyChanged(() => Model);
            }
        }

        #endregion

        public BindableCollection<LookupValue> Instruments { get; private set; }

        public QuoteEditViewModel(ILog log, IDispatcherService dispatcherService, ITaskScheduler taskScheduler, IViewService viewService, IQuoteEditService service,
            BindableCollectionFactory bindableCollectionFactory, Func<ToolBarButtonItem> toolBarButtonItemFactory) 
            : base(log, dispatcherService)
        {
            _taskScheduler = taskScheduler;
            _viewService = viewService;
            _service = service;

            ToolBarItems = bindableCollectionFactory.Get<IToolBarItem>();

            CreateToolBar(toolBarButtonItemFactory);

            Instruments = bindableCollectionFactory.Get<LookupValue>();
        }

        public void Initialise(Guid id)
        {
            _id = id;
        }

        protected override void OnInitialise()
        {
            if (_id.HasValue)
                LoadQuote();
            else
                NewQuote();
        }

        private void NewQuote()
        {
            BusyAsync("... Loading Quote ...")
                .SelectMany(_ => _service.GetInitialisationData())
                .SelectMany(response => Instruments.AddRange(response.Instruments))
                .SelectMany(() => _service.NewQuote())
                .SelectMany(model => Model = model)
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quote"), _taskScheduler.Default)
                .Finally(Idle, _taskScheduler.Default);
        }

        private void LoadQuote()
        {
            BusyAsync("... Loading Quote ...")
                .SelectMany(() => _service.GetQuote(_id.Value))
                .SelectMany(quote => Model = quote)
                .SelectMany(_ => _service.GetInitialisationData())
                .SelectMany(response => Instruments.AddRange(response.Instruments))
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quote"), _taskScheduler.Default)
                .Finally(Idle, _taskScheduler.Default);
        }

        private void CreateToolBar(Func<ToolBarButtonItem> toolBarButtonItemFactory)
        {
            var saveToolBarItem = toolBarButtonItemFactory();
            saveToolBarItem.DisplayName = "Save";
            saveToolBarItem.Command = new DelegateCommand(Save);
            ToolBarItems.Add(saveToolBarItem);

            var cancelToolBarItem = toolBarButtonItemFactory();
            cancelToolBarItem.DisplayName = "Cancel";
            cancelToolBarItem.Command = new DelegateCommand(Close);
            ToolBarItems.Add(cancelToolBarItem);
        }

        private void Save()
        {
            BusyAsync("... Saving Quote ...")
                .SelectMany(_ => _service.SaveQuote(Model))
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quote"), _taskScheduler.Default)
                .Finally(() =>
                {
                    Idle();
                    Close();
                }, _taskScheduler.Default);
        }
    }
}