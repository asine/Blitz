using System.Linq;

using Blitz.Common.Core;
using Blitz.Common.Customer;
using Blitz.Server.Core;

namespace Blitz.Server.Customer
{
    public class ReportRunnerHandler : Handler<ReportRunnerRequest, ReportRunnerResponse>
    {
        public ReportRunnerHandler(ILog log) 
            : base(log)
        {
        }

        protected override ReportRunnerResponse Execute(ReportRunnerRequest request)
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