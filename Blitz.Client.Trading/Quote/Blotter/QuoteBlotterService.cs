using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Naru.Agatha;
using Naru.TPL;
using Naru.WPF.MVVM;

using Blitz.Client.Trading.Quote.Edit;
using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Blotter;

namespace Blitz.Client.Trading.Quote.Blotter
{
    public interface IQuoteBlotterService
    {
        Task<List<QuoteDto>> GetQuotesAsync();

        Task NewQuoteAsync();

        Task EditQuoteAsync(QuoteBlotterItemViewModel quoteBlotterItemViewModel);
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

        public Task<List<QuoteDto>> GetQuotesAsync()
        {
            return _requestTask
                .Get(new GetQuotesRequest())
                .Select(x => x.Results.ToList());
        }

        public Task NewQuoteAsync()
        {
            var viewModel = _quoteEditViewModelFactory();
            viewModel.Initialise();
            return _viewService.ShowModalAsync(viewModel);
        }

        public Task EditQuoteAsync(QuoteBlotterItemViewModel quoteBlotterItemViewModel)
        {
            var viewModel = _quoteEditViewModelFactory();
            viewModel.Initialise(quoteBlotterItemViewModel.Id);
            return _viewService.ShowModalAsync(viewModel);
        }
    }
}