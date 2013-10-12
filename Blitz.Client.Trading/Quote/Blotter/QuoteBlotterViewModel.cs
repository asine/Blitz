using System.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.MVVM;
using Naru.WPF.MVVM.ToolBar;
using Naru.WPF.ModernUI.Assets.Icons;

using Microsoft.Practices.Prism.Commands;

using Naru.WPF.Scheduler;

namespace Blitz.Client.Trading.Quote.Blotter
{
    public class QuoteBlotterViewModel : Workspace
    {
        private readonly IScheduler _scheduler;
        private readonly IQuoteBlotterService _service;

        public BindableCollection<QuoteBlotterItemViewModel> Items { get; private set; }

        public DelegateCommand<QuoteBlotterItemViewModel> OpenCommand { get; private set; }

        public QuoteBlotterViewModel(ILog log, IScheduler scheduler, IViewService viewService,
            BindableCollectionFactory bindableCollectionFactory, IQuoteBlotterService service, IToolBarService toolBarService)
            : base(log, scheduler, viewService)
        {
            _scheduler = scheduler;
            _service = service;

            this.SetupHeader("Blotter");

            Items = bindableCollectionFactory.Get<QuoteBlotterItemViewModel>();
            OpenCommand = new DelegateCommand<QuoteBlotterItemViewModel>(quote =>
                _service.EditQuoteAsync(quote)
                    .SelectMany(() => BusyViewModel.ActiveAsync("... Refreshing quotes ..."))
                    .SelectMany(() => RefreshQuotesAsync())
                    .CatchAndHandle(_ => ViewService.StandardDialogBuilder().Error("Error", "Problem refreshing quotes"), _scheduler.Task)
                    .Finally(BusyViewModel.InActive, _scheduler.Task));

            CreateToolBar(toolBarService);
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("... Loading quotes ...")
                .SelectMany(_ => RefreshQuotesAsync())
                .CatchAndHandle(_ => ViewService.StandardDialogBuilder().Error("Error", "Problem loading quotes"), _scheduler.Task)
                .Finally(BusyViewModel.InActive, _scheduler.Task);
        }

        public override bool CanClose()
        {
            return true;
        }

        private Task RefreshQuotesAsync()
        {
            return _service.GetQuotesAsync()
                .Do(() => Items.ClearAsync())
                .SelectMany(quotes =>
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
                })
                .LogException(Log);
        }

        private void CreateToolBar(IToolBarService toolBarService)
        {
            var newQuoteToolBarItem = toolBarService.CreateToolBarButtonItem();
            newQuoteToolBarItem.DisplayName = "New";
            newQuoteToolBarItem.ImageName = IconNames.NEW;
            newQuoteToolBarItem.Command = new DelegateCommand(() =>
                _service.NewQuoteAsync()
                    .SelectMany(() => BusyViewModel.ActiveAsync("... Refreshing quotes ..."))
                    .SelectMany(() => RefreshQuotesAsync())
                    .CatchAndHandle(_ => ViewService.StandardDialogBuilder().Error("Error", "Problem refreshing quotes"), _scheduler.Task)
                    .Finally(BusyViewModel.InActive, _scheduler.Task));
            toolBarService.Items.Add(newQuoteToolBarItem);

            this.SyncToolBarItemWithViewModelActivationState(newQuoteToolBarItem);
        }
    }
}