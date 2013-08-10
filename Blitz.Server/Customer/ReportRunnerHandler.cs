using System.Linq;

using Agatha.Common;
using Agatha.ServiceLayer;

using Blitz.Common.Customer;

namespace Blitz.Server.Customer
{
    public class ReportRunnerHandler : RequestHandler<ReportRunnerRequest, ReportRunnerResponse>
    {
        public override Response Handle(ReportRunnerRequest request)
        {
            var response = CreateTypedResponse();

            var results = Enumerable.Range(0, 5)
                .Select(x => new ReportDto {Id = x})
                .ToList();

            response.Results = results;

            return response;
        }
    }
}