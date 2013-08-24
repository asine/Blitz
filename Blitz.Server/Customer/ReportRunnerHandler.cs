using System;
using System.Linq;

using Blitz.Common.Core;
using Blitz.Common.Customer;
using Blitz.Server.Core;

namespace Blitz.Server.Customer
{
    public class ReportRunnerHandler : HandlerBase<ReportRunnerRequest, ReportRunnerResponse>
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

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

            return response;
        }
    }
}