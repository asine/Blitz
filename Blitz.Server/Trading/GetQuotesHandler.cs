using System.Collections.Generic;

using Blitz.Common.Core;
using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Blotter;
using Blitz.Server.Core;

namespace Blitz.Server.Trading
{
    public class GetQuotesHandler : Handler<GetQuotesRequest, GetQuotesResponse>
    {
        public GetQuotesHandler(ILog log)
            : base(log)
        {
        }

        protected override GetQuotesResponse Execute(GetQuotesRequest request)
        {
            var response = CreateTypedResponse();

            var results = new List<QuoteDto>();
            for (var index = 0; index < 100; index++)
            {
                var item = new QuoteDto();
                item.Id = index;

                results.Add(item);
            }
            response.Results = results;

            return response;
        }
    }
}