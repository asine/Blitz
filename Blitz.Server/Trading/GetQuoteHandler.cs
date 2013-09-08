using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Edit;
using Blitz.Server.Core;

using Common.Logging;

using Raven.Client;

namespace Blitz.Server.Trading
{
    public class GetQuoteHandler : Handler<GetQuoteRequest, GetQuoteResponse>
    {
        private readonly IDocumentStore _documentStore;

        public GetQuoteHandler(ILog log, IDocumentStore documentStore)
            : base(log)
        {
            _documentStore = documentStore;
        }

        protected override GetQuoteResponse Execute(GetQuoteRequest request)
        {
            var response = CreateTypedResponse();

            using (var session = _documentStore.OpenSession())
            {
                var quote = session.Load<QuoteDto>(request.Id);
                response.Result = quote;
            }

            return response;
        }
    }
}