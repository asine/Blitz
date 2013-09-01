using System;
using System.Threading;
using System.Threading.Tasks;

using Blitz.Client.Core.Agatha;
using Blitz.Common.Trading.Quote.Edit;

namespace Blitz.Client.Trading.QuoteEdit
{
    public interface IQuoteEditService
    {
        Task<QuoteModel> GetQuote(long id);

        Task<GetInitialisationDataResponse> GetInitialisationData();

        Task SaveQuote(QuoteModel quoteModel);
    }

    public class QuoteEditService : IQuoteEditService
    {
        private readonly IRequestTask _requestTask;

        public QuoteEditService(IRequestTask requestTask)
        {
            _requestTask = requestTask;
        }

        public Task<QuoteModel> GetQuote(long id)
        {
            var request = new GetQuoteRequest{Id = id};
            return _requestTask.Get<GetQuoteRequest, GetQuoteResponse, QuoteModel>(request, x =>
            {
                var quoteModel = new QuoteModel(id);
                AutoMapper.Mapper.Map(x.Result, quoteModel);
                return quoteModel;
            });
        }

        public Task<GetInitialisationDataResponse> GetInitialisationData()
        {
            return _requestTask.Get<GetInitialisationDataRequest, GetInitialisationDataResponse>(new GetInitialisationDataRequest());
        }

        public Task SaveQuote(QuoteModel quoteModel)
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            });
        }
    }
}