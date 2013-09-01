using System.Threading.Tasks;

using Blitz.Client.Core.Agatha;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutService : IReportLayoutService
    {
        private readonly IRequestTask _requestTask;

        public ReportLayoutService(IRequestTask requestTask)
        {
            _requestTask = requestTask;
        }

        public Task<GetAttributesResponse> GetAttributes()
        {
            return _requestTask.Get<GetAttributesRequest, GetAttributesResponse>(new GetAttributesRequest());
        }
    }
}