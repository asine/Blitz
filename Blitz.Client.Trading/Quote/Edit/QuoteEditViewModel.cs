using System;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;

using Blitz.Common.Trading.Quote.Edit;

using Microsoft.Practices.Prism.Commands;

using Naru.WPF.TPL;

namespace Blitz.Client.Trading.Quote.Edit
{
    public class QuoteEditViewModel : Workspace
    {
        private readonly IScheduler _scheduler;
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

        public QuoteEditViewModel(ILog log, IScheduler scheduler, IViewService viewService, 
            IQuoteEditService service, BindableCollectionFactory bindableCollectionFactory, Func<ToolBarButtonItem> toolBarButtonItemFactory)
            : base(log, scheduler)
        {
            _scheduler = scheduler;
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
            {
                LoadQuote();
            }
            else
            {
                NewQuote();
            }
        }

        private void NewQuote()
        {
            BusyAsync("... Loading Quote ...")
                .SelectMany(_ => _service.GetInitialisationDataAsync())
                .SelectMany(response => Instruments.AddRange(response.Instruments))
                .SelectMany(() => _service.NewQuoteAsync())
                .Do(model => Model = model)
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quote"), _scheduler.Task)
                .Finally(Idle, _scheduler.Task);
        }

        private void LoadQuote()
        {
            BusyAsync("... Loading Quote ...")
                .SelectMany(() => _service.GetQuoteAsync(_id.Value))
                .Do(quote => Model = quote)
                .SelectMany(_ => _service.GetInitialisationDataAsync())
                .SelectMany(response => Instruments.AddRange(response.Instruments))
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quote"), _scheduler.Task)
                .Finally(Idle, _scheduler.Task);
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
                .SelectMany(_ => _service.SaveQuoteAsync(Model))
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem saving quote"), _scheduler.Task)
                .Finally(() =>
                {
                    Idle();
                    Close();
                }, _scheduler.Task);
        }
    }
}