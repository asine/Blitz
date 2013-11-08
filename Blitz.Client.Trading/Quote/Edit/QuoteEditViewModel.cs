using System;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.MVVM;

using Blitz.Common.Trading.Quote.Edit;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Trading.Quote.Edit
{
    public class QuoteEditViewModel : Workspace
    {
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

        public QuoteEditViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService,
                                  IQuoteEditService service,
                                  BindableCollection<IToolBarItem> toolBarItemsCollection,
                                  BindableCollection<LookupValue> instrumentsCollection,
                                  Func<ToolBarButtonItem> toolBarButtonItemFactory)
            : base(log, scheduler, viewService)
        {
            _service = service;

            ToolBarItems = toolBarItemsCollection;

            CreateToolBar(toolBarButtonItemFactory);

            Instruments = instrumentsCollection;
        }

        public void Initialise(Guid id = default(Guid))
        {
            if (id == default(Guid))
            {
                this.SetupHeader("Create Quote");
            }
            else
            {
                _id = id;

                this.SetupHeader(_id.ToString());
            }
        }

        protected override Task OnInitialise()
        {
            return Task.Factory.StartNew(() =>
            {
                if (_id.HasValue)
                {
                    LoadQuote();
                }
                else
                {
                    NewQuote();
                }
            });
        }

        private void NewQuote()
        {
            BusyViewModel.ActiveAsync("... Loading Quote ...")
                .Then(_ => _service.GetInitialisationDataAsync(), Scheduler.TPL.Task)
                .Then(response => Instruments.AddRange(response.Instruments), Scheduler.TPL.Dispatcher)
                .Then(() => _service.NewQuoteAsync(), Scheduler.TPL.Task)
                .Do(model => Model = model, Scheduler.TPL.Task)
                .LogException(Log)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem loading quote"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }

        private void LoadQuote()
        {
            BusyViewModel.ActiveAsync("... Loading Quote ...")
                .Then(() => _service.GetQuoteAsync(_id.Value), Scheduler.TPL.Task)
                .Do(quote => Model = quote, Scheduler.TPL.Task)
                .Then(_ => _service.GetInitialisationDataAsync(), Scheduler.TPL.Task)
                .Then(response => Instruments.AddRange(response.Instruments), Scheduler.TPL.Dispatcher)
                .LogException(Log)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem loading quote"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
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
            BusyViewModel.ActiveAsync("... Saving Quote ...")
                .Then(_ => _service.SaveQuoteAsync(Model), Scheduler.TPL.Task)
                .LogException(Log)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem saving quote"), Scheduler.TPL.Task)
                .Finally(() =>
                {
                    BusyViewModel.InActive();
                    Close();
                }, Scheduler.TPL.Task);
        }
    }
}