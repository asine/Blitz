using System.Linq;

using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Blotter;
using Blitz.Server.Core;

using Common.Logging;

using Raven.Client;

namespace Blitz.Server.Trading
{
    public class GetQuotesHandler : Handler<GetQuotesRequest, GetQuotesResponse>
    {
        private readonly IDocumentStore _documentStore;

        public GetQuotesHandler(ILog log, IDocumentStore documentStore)
            : base(log)
        {
            _documentStore = documentStore;
        }

        protected override GetQuotesResponse Execute(GetQuotesRequest request)
        {
            var response = CreateTypedResponse();

            using (var session = _documentStore.OpenSession())
            {
                var quotes = session.Query<QuoteDto>().ToList();
                response.Results = quotes;
            }

            return response;
        }
    }
}