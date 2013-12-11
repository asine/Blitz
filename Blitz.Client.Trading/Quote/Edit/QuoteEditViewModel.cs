using System;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
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

        public QuoteEditViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                                  IQuoteEditService service,
                                  BindableCollection<IToolBarItem> toolBarItemsCollection,
                                  BindableCollection<LookupValue> instrumentsCollection,
                                  Func<ToolBarButtonItem> toolBarButtonItemFactory)
            : base(log, scheduler, standardDialog)
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
                this.SetupHeader(Scheduler, "Create Quote");
            }
            else
            {
                _id = id;

                this.SetupHeader(Scheduler, _id.ToString());
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
                         .Then(() => _service.GetInitialisationDataAsync(), Scheduler.Task.TPL)
                         .Then(response => Instruments.AddRangeAsync(response.Instruments), Scheduler.Dispatcher.TPL)
                         .Then(() => _service.NewQuoteAsync(), Scheduler.Task.TPL)
                         .Do(model => Model = model, Scheduler.Task.TPL)
                         .LogException(Log)
                         .CatchAndHandle(_ => StandardDialog.Error("Error", "Problem loading quote"), Scheduler.Task.TPL)
                         .Finally(BusyViewModel.InActive, Scheduler.Task.TPL);
        }

        private void LoadQuote()
        {
            BusyViewModel.ActiveAsync("... Loading Quote ...")
                         .Then(() => _service.GetQuoteAsync(_id.Value), Scheduler.Task.TPL)
                         .Do(quote => Model = quote, Scheduler.Task.TPL)
                         .Then(_ => _service.GetInitialisationDataAsync(), Scheduler.Task.TPL)
                         .Then(response => Instruments.AddRangeAsync(response.Instruments), Scheduler.Dispatcher.TPL)
                         .LogException(Log)
                         .CatchAndHandle(_ => StandardDialog.Error("Error", "Problem loading quote"), Scheduler.Task.TPL)
                         .Finally(BusyViewModel.InActive, Scheduler.Task.TPL);
        }

        private void CreateToolBar(Func<ToolBarButtonItem> toolBarButtonItemFactory)
        {
            var saveToolBarItem = toolBarButtonItemFactory();
            saveToolBarItem.DisplayName = "Save";
            saveToolBarItem.Command = new DelegateCommand(Save);
            ToolBarItems.Add(saveToolBarItem);

            var cancelToolBarItem = toolBarButtonItemFactory();
            cancelToolBarItem.DisplayName = "Cancel";
            cancelToolBarItem.Command = new DelegateCommand(ClosingStrategy.Close);
            ToolBarItems.Add(cancelToolBarItem);
        }

        private void Save()
        {
            BusyViewModel.ActiveAsync("... Saving Quote ...")
                         .Then(() => _service.SaveQuoteAsync(Model), Scheduler.Task.TPL)
                         .LogException(Log)
                         .CatchAndHandle(_ => StandardDialog.Error("Error", "Problem saving quote"), Scheduler.Task.TPL)
                         .Finally(() =>
                                  {
                                      BusyViewModel.InActive();
                                      ClosingStrategy.Close();
                                  }, Scheduler.Task.TPL);
        }
    }
}