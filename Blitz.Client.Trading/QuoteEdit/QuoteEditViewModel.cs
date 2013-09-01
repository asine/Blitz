using System;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;
using Blitz.Common.Trading.Quote.Edit;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Trading.QuoteEdit
{
    public class QuoteEditViewModel : Workspace
    {
        private readonly IViewService _viewService;
        private readonly IQuoteEditService _service;

        private long _id;

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

        public QuoteEditViewModel(ILog log, IDispatcherService dispatcherService, IViewService viewService, IQuoteEditService service,
            BindableCollectionFactory bindableCollectionFactory, Func<ToolBarButtonItem> toolBarButtonItemFactory) 
            : base(log, dispatcherService)
        {
            _viewService = viewService;
            _service = service;

            ToolBarItems = bindableCollectionFactory.Get<IToolBarItem>();

            CreateToolBar(toolBarButtonItemFactory);

            Instruments = bindableCollectionFactory.Get<LookupValue>();
        }

        public void Initialise(long id)
        {
            _id = id;
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading Quote ...")
                .Then(_ => _service.GetQuote(_id))
                .ThenDo(quote => Model = quote)
                .Then(_ => _service.GetInitialisationData())
                .ThenDo(response => Instruments.AddRange(response.Instruments))
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quote"))
                .Finally(Idle);
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
                .Then(_ => _service.SaveQuote(Model))
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quote"))
                .Finally(() =>
                {
                    Idle();
                    Close();
                });
        }
    }
}