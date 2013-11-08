using System.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.MVVM;
using Naru.WPF.ModernUI.Assets.Icons;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Trading.Quote.Blotter
{
    public class QuoteBlotterViewModel : Workspace
    {
        private readonly IQuoteBlotterService _service;

        public BindableCollection<QuoteBlotterItemViewModel> Items { get; private set; }

        public DelegateCommand<QuoteBlotterItemViewModel> OpenCommand { get; private set; }

        public QuoteBlotterViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService,
                                     BindableCollection<QuoteBlotterItemViewModel> itemsCollection, IQuoteBlotterService service,
                                     IToolBarService toolBarService)
            : base(log, scheduler, viewService)
        {
            _service = service;

            this.SetupHeader("Blotter");

            Items = itemsCollection;

            OpenCommand = new DelegateCommand<QuoteBlotterItemViewModel>(quote => Open(quote));

            CreateToolBar(toolBarService);
        }

        private object Open(QuoteBlotterItemViewModel quote)
        {
            return _service.EditQuoteAsync(quote)
                .Then(() => BusyViewModel.ActiveAsync("... Refreshing quotes ..."), Scheduler.TPL.Dispatcher)
                .Then(() => RefreshQuotesAsync(), Scheduler.TPL.Task)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem refreshing quotes"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading quotes ...")
                .Then(_ => RefreshQuotesAsync(), Scheduler.TPL.Task)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem loading quotes"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }

        public override bool CanClose()
        {
            return true;
        }

        private Task RefreshQuotesAsync()
        {
            return _service.GetQuotesAsync()
                .Do(() => Items.ClearAsync(), Scheduler.TPL.Dispatcher)
                .Then(quotes =>
                      {
                          var items = quotes.Select(x => new QuoteBlotterItemViewModel
                                                         {
                                                             Id = x.Id,
                                                             Instrument = x.InstrumentName,
                                                             CreatedBy = x.CreatedBy,
                                                             CreatedOn = x.CreatedOn,
                                                             ModifiedBy = x.ModifiedBy,
                                                             ModifiedOn = x.ModifiedOn
                                                         });
                          return Items.AddRangeAsync(items);
                      }, Scheduler.TPL.Dispatcher)
                .LogException(Log);
        }

        private void CreateToolBar(IToolBarService toolBarService)
        {
            var newQuoteToolBarItem = toolBarService.CreateToolBarButtonItem();
            newQuoteToolBarItem.DisplayName = "New";
            newQuoteToolBarItem.ImageName = IconNames.NEW;
            newQuoteToolBarItem.Command = new DelegateCommand(() => NewQuote());
            toolBarService.Items.Add(newQuoteToolBarItem);

            this.SyncToolBarItemWithViewModelActivationState(newQuoteToolBarItem);
        }

        private object NewQuote()
        {
            return _service.NewQuoteAsync()
                .Then(() => BusyViewModel.ActiveAsync("... Refreshing quotes ..."), Scheduler.TPL.Dispatcher)
                .Then(() => RefreshQuotesAsync(), Scheduler.TPL.Task)
                .CatchAndHandle(_ => ViewService.StandardDialog().Error("Error", "Problem refreshing quotes"), Scheduler.TPL.Task)
                .Finally(BusyViewModel.InActive, Scheduler.TPL.Task);
        }
    }
}