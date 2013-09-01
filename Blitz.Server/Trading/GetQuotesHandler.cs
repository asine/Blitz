using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Common.Core;
using Blitz.Common.Trading.QuoteBlotter;
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