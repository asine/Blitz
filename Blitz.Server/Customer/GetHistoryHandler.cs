using System.Linq;

using Agatha.Common;
using Agatha.ServiceLayer;

using Blitz.Common.Customer;

namespace Blitz.Server.Customer
{
    public class GetHistoryHandler : RequestHandler<GetHistoryRequest, GetHistoryResponse>
    {
        public override Response Handle(GetHistoryRequest request)
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