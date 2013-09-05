using System.Linq;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Client.ModernUI.Assets.Icons;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Trading.Quote.Blotter
{
    public class QuoteBlotterViewModel : Workspace
    {
        private readonly ITaskScheduler _taskScheduler;
        private readonly IViewService _viewService;
        private readonly IQuoteBlotterService _service;

        public BindableCollection<QuoteBlotterItemViewModel> Items { get; private set; }

        public DelegateCommand<QuoteBlotterItemViewModel> OpenCommand { get; private set; }

        public QuoteBlotterViewModel(ILog log, IDispatcherService dispatcherService, ITaskScheduler taskScheduler, IViewService viewService,
            BindableCollectionFactory bindableCollectionFactory, IQuoteBlotterService service, IToolBarService toolBarService) 
            : base(log, dispatcherService)
        {
            _taskScheduler = taskScheduler;
            _viewService = viewService;
            _service = service;
            DisplayName = "Blotter";

            Items = bindableCollectionFactory.Get<QuoteBlotterItemViewModel>();
            OpenCommand = new DelegateCommand<QuoteBlotterItemViewModel>(quote =>
            {
                _service.EditQuote(quote);

                LoadQuotes();
            });

            CreateToolBar(toolBarService);
        }

        protected override void OnInitialise()
        {
            LoadQuotes();
        }

        private void LoadQuotes()
        {
            BusyAsync("... Loading quotes ...")
                .Then(_ => _service.GetQuotes())
                .ThenDo(quotes =>
                {
                    Items.Clear();
                    var items = quotes.Select(x => new QuoteBlotterItemViewModel
                    {
                        Id = x.Id,
                        Instrument = x.InstrumentName,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn
                    });
                    Items.AddRange(items);
                })
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quotes"), _taskScheduler.Default)
                .Finally(Idle, _taskScheduler.Default);
        }

        private void CreateToolBar(IToolBarService toolBarService)
        {
            var newQuoteToolBarItem = toolBarService.CreateToolBarButtonItem();
            newQuoteToolBarItem.DisplayName = "New";
            newQuoteToolBarItem.ImageName = IconNames.NEW;
            newQuoteToolBarItem.Command = new DelegateCommand(() =>
            {
                _service.NewQuote();

                LoadQuotes();
            });
            toolBarService.Items.Add(newQuoteToolBarItem);
        }
    }
}