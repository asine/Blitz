﻿using System;
using System.Threading.Tasks;

using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Edit;

using Naru.Agatha;
using Naru.Concurrency.Scheduler;
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
        private readonly Func<QuoteModel> _quoteModelFactory;

        public QuoteEditService(IRequestTask requestTask, ISchedulerProvider scheduler, Func<QuoteModel> quoteModelFactory)
        {
            _requestTask = requestTask;
            _scheduler = scheduler;
            _quoteModelFactory = quoteModelFactory;
        }

        public Task<QuoteModel> NewQuoteAsync()
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             var quoteModel = _quoteModelFactory();
                                             quoteModel.Initialise(Guid.NewGuid());
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
                            var quoteModel = _quoteModelFactory();
                            quoteModel.Initialise(id);
                            quoteModel.Instrument = new LookupValue
                                                    {
                                                        Id = quoteDto.InstrumentId,
                                                        Value = quoteDto.InstrumentName
                                                    };
                            quoteModel.Notes = quoteDto.Notes;
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