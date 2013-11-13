using System;
using System.Threading.Tasks;

using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Edit;

using Naru.Agatha;
using Naru.TPL;
using Naru.WPF.Scheduler;

namespace Blitz.Client.Trading.Quote.Edit
{
    public interface IQuoteEditService
    {
        Task<QuoteModel> NewQuoteAsync();

        Task<QuoteModel> GetQuoteAsync(Guid id);

        Task<GetInitialisationDataResponse> GetInitialisationDataAsync();

        Task SaveQuoteAsync(QuoteModel quoteModel);
    }

    public class QuoteEditService : IQuoteEditService
    {
        private readonly IRequestTask _requestTask;
        private readonly ISchedulerProvider _scheduler;

        public QuoteEditService(IRequestTask requestTask, ISchedulerProvider scheduler)
        {
            _requestTask = requestTask;
            _scheduler = scheduler;
        }

        public Task<QuoteModel> NewQuoteAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                var quoteModel = new QuoteModel(Guid.NewGuid());
                return quoteModel;
            });
        }

        public Task<QuoteModel> GetQuoteAsync(Guid id)
        {
            var request = new GetQuoteRequest{Id = id};
            return _requestTask
                .Get(request)
                .Select(x =>
                {
                    var quoteDto = x.Result;
                    var quoteModel = new QuoteModel(id)
                    {
                        Instrument = new LookupValue
                        {
                            Id = quoteDto.InstrumentId,
                            Value = quoteDto.InstrumentName
                        },
                        Notes = quoteDto.Notes
                    };
                    return quoteModel;
                }, _scheduler.Task.TPL);
        }

        public Task<GetInitialisationDataResponse> GetInitialisationDataAsync()
        {
            return _requestTask.Get(new GetInitialisationDataRequest());
        }

        public Task SaveQuoteAsync(QuoteModel quoteModel)
        {
            var quote = new QuoteDto
            {
                Id = quoteModel.Id,
                InstrumentId = quoteModel.Instrument.Id,
                InstrumentName = quoteModel.Instrument.Value,
                Notes = quoteModel.Notes
            };
            return _requestTask.Get(new SaveQuoteRequest {Quote = quote});
        }
    }
}