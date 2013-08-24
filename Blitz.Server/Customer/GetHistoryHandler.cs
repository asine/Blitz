using System;
using System.Linq;

using Blitz.Common.Core;
using Blitz.Common.Customer;
using Blitz.Server.Core;

namespace Blitz.Server.Customer
{
    public class GetHistoryHandler : Handler<GetHistoryRequest, GetHistoryResponse>
    {
        public GetHistoryHandler(ILog log) 
            : base(log)
        {
        }

        protected override GetHistoryResponse Execute(GetHistoryRequest request)
        {
            var response = CreateTypedResponse();

            var results = Enumerable.Range(0, 5)
                .Select(x => new HistoryDto { Id = x })
                .ToList();

            response.Results = results;

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

            return response;
        }
    }
}