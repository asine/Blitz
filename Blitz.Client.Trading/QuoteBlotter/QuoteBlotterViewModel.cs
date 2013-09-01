using System.Linq;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;
using Blitz.Common.Trading.QuoteBlotter;

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
            BindableCollectionFactory bindableCollectionFactory, IQuoteBlotterService service) 
            : base(log, dispatcherService)
        {
            _viewService = viewService;
            _service = service;
            DisplayName = "Blotter";

            Items = bindableCollectionFactory.Get<QuoteBlotterItemViewModel>();
            OpenCommand = new DelegateCommand<QuoteBlotterItemViewModel>(quote =>
            {
                
            });
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading quotes ...")
                .Then(_ => _service.GetQuotes())
                .ThenDo(quotes =>
                {
                    var items = quotes.Select(AutoMapper.Mapper.Map<QuoteDto, QuoteBlotterItemViewModel>);
                    Items.AddRange(items);
                })
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem loading quotes"))
                .Finally(Idle);
        }
    }
}