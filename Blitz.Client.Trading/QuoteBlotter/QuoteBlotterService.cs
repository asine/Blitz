using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Core.Agatha;
using Blitz.Common.Trading.QuoteBlotter;

namespace Blitz.Client.Trading.QuoteBlotter
{
    public interface IQuoteBlotterService
    {
        Task<List<QuoteDto>> GetQuotes();
    }

    public class QuoteBlotterService : IQuoteBlotterService
    {
        private readonly IRequestTask _requestTask;

        public QuoteBlotterService(IRequestTask requestTask)
        {
            _requestTask = requestTask;
        }

        public Task<List<QuoteDto>> GetQuotes()
        {
            return _requestTask.Get<GetQuotesRequest, GetQuotesResponse, List<QuoteDto>>(new GetQuotesRequest(), x => x.Results.ToList());
        }
    }
}