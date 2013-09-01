using System.Linq;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Trading.QuoteBlotter
{
    public class QuoteBlotterViewModel : Workspace
    {
        private readonly IViewService _viewService;
        private readonly IQuoteBlotterService _service;

        public BindableCollection<QuoteBlotterItemViewModel> Items { get; private set; }

        public DelegateCommand<QuoteBlotterItemViewModel> OpenCommand { get; private set; }

        public QuoteBlotterViewModel(ILog log, IDispatcherService dispatcherService, IViewService viewService,
            BindableCollectionFactory bindableCollectionFactory, IQuoteBlotterService service, IToolBarService toolBarService) 
            : base(log, dispatcherService)
        {
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
                        Instrument = x.InstrumentName
                    });
                    Items.AddRange(items);
                })
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quotes"))
                .Finally(Idle);
        }

        private void CreateToolBar(IToolBarService toolBarService)
        {
            var newQuoteToolBarItem = toolBarService.CreateToolBarButtonItem();
            newQuoteToolBarItem.DisplayName = "New";
            newQuoteToolBarItem.Command = new DelegateCommand(() =>
            {
                _service.NewQuote();

                LoadQuotes();
            });
            toolBarService.Items.Add(newQuoteToolBarItem);
        }
    }
}