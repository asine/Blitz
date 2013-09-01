using System;
using System.Threading.Tasks;

using Blitz.Client.Core.Agatha;
using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Edit;

namespace Blitz.Client.Trading.QuoteEdit
{
    public interface IQuoteEditService
    {
        Task<QuoteModel> NewQuote();

        Task<QuoteModel> GetQuote(Guid id);

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

        public Task<QuoteModel> NewQuote()
        {
            return Task.Factory.StartNew(() =>
            {
                var quoteModel = new QuoteModel(Guid.NewGuid());
                return quoteModel;
            });
        }

        public Task<QuoteModel> GetQuote(Guid id)
        {
            var request = new GetQuoteRequest{Id = id};
            return _requestTask.Get<GetQuoteRequest, GetQuoteResponse, QuoteModel>(request, x =>
            {
                var quoteModel = new QuoteModel(id);
                quoteModel.Instrument = new LookupValue
                {
                    Id = x.Result.InstrumentId,
                    Value = x.Result.InstrumentName
                };
                return quoteModel;
            });
        }

        public Task<GetInitialisationDataResponse> GetInitialisationData()
        {
            return _requestTask.Get<GetInitialisationDataRequest, GetInitialisationDataResponse>(new GetInitialisationDataRequest());
        }

        public Task SaveQuote(QuoteModel quoteModel)
        {
            var quote = new QuoteDto
            {
                Id = quoteModel.Id,
                InstrumentId = quoteModel.Instrument.Id,
                InstrumentName = quoteModel.Instrument.Value
            };
            return _requestTask.Get<SaveQuoteRequest, SaveQuoteResponse>(new SaveQuoteRequest {Quote = quote});
        }
    }
}