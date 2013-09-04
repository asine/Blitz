using System;

using Blitz.Common.Core;
using Blitz.Common.Trading.Quote.Edit;
using Blitz.Server.Core;

using Raven.Client;

namespace Blitz.Server.Trading
{
    public class SaveQuoteHandler : Handler<SaveQuoteRequest, SaveQuoteResponse>
    {
        private readonly IDocumentStore _documentStore;

        public SaveQuoteHandler(ILog log, IDocumentStore documentStore)
            : base(log)
        {
            _documentStore = documentStore;
        }

        protected override SaveQuoteResponse Execute(SaveQuoteRequest request)
        {
            var response = CreateTypedResponse();

            var quote = request.Quote;
            if (!quote.CreatedOn.HasValue)
            {
                quote.CreatedOn = DateTime.Now;
                quote.CreatedBy = "xjpeter";
            }

            using (var session = _documentStore.OpenSession())
            {
                session.Store(quote);
                session.SaveChanges();
            }

            return response;
        }
    }
}