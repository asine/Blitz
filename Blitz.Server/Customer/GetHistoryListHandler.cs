using System.Linq;

using Blitz.Common.Core;
using Blitz.Common.Customer;
using Blitz.Server.Core;

namespace Blitz.Server.Customer
{
    public class GetHistoryListHandler : Handler<GetHistoryListRequest, GetHistoryListResponse>
    {
        public GetHistoryListHandler(ILog log) 
            : base(log)
        {
        }

        protected override GetHistoryListResponse Execute(GetHistoryListRequest request)
        {
            var response = CreateTypedResponse();

            var results = Enumerable.Range(0, 5)
                .Select(x => new HistoryDto { Id = x })
                .ToList();

            response.Results = results;

            return response;
        }
    }
}