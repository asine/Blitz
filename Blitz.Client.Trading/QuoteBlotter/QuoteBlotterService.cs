using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Trading.QuoteEdit;
using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Blotter;

namespace Blitz.Client.Trading.QuoteBlotter
{
    public interface IQuoteBlotterService
    {
        Task<List<QuoteDto>> GetQuotes();

        void NewQuote();

        void EditQuote(QuoteBlotterItemViewModel quoteBlotterItemViewModel);
    }

    public class QuoteBlotterService : IQuoteBlotterService
    {
        private readonly IRequestTask _requestTask;
        private readonly IViewService _viewService;
        private readonly Func<QuoteEditViewModel> _quoteEditViewModelFactory;

        public QuoteBlotterService(IRequestTask requestTask, IViewService viewService, Func<QuoteEditViewModel> quoteEditViewModelFactory)
        {
            _requestTask = requestTask;
            _viewService = viewService;
            _quoteEditViewModelFactory = quoteEditViewModelFactory;
        }

        public Task<List<QuoteDto>> GetQuotes()
        {
            return _requestTask.Get<GetQuotesRequest, GetQuotesResponse, List<QuoteDto>>(new GetQuotesRequest(), x => x.Results.ToList());
        }

        public void NewQuote()
        {
            var viewModel = _quoteEditViewModelFactory();
            _viewService.ShowModal(viewModel);
        }

        public void EditQuote(QuoteBlotterItemViewModel quoteBlotterItemViewModel)
        {
            var viewModel = _quoteEditViewModelFactory();
            viewModel.Initialise(quoteBlotterItemViewModel.Id);
            _viewService.ShowModal(viewModel);
        }
    }
}