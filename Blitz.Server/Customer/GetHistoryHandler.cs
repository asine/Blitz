using System.Linq;

using Blitz.Common.Core;
using Blitz.Common.Customer;
using Blitz.Server.Core;

namespace Blitz.Server.Customer
{
    public class GetHistoryHandler : HandlerBase<GetHistoryRequest, GetHistoryResponse>
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

            return response;
        }
    }
}