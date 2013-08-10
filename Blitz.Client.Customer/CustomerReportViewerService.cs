using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Core.Agatha;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer
{
    public class CustomerReportViewerService : IReportViewerService<GetHistoryRequest, GetHistoryResponse>
    {
        private readonly IRequestTask _requestTask;

        public CustomerReportViewerService(IRequestTask requestTask)
        {
            _requestTask = requestTask;
        }

        public GetHistoryRequest CreateRequest()
        {
            return new GetHistoryRequest();
        }

        public Task<GetHistoryResponse> GetHistory(GetHistoryRequest request)
        {
            return _requestTask.GetUnstarted<GetHistoryRequest, GetHistoryResponse>(request);
        }

        public Task<List<ReportViewerItemViewModel>> GenerateItemViewModels(GetHistoryResponse response)
        {
            return new Task<List<ReportViewerItemViewModel>>(
                () => new List<ReportViewerItemViewModel>(response.Results
                    .Select((x, i) =>
                    {
                        var item = new ReportViewerItemViewModel();
                        item.Name = "Instance " + item.Name;

                        return item;
                    })
                    .ToList()));
        }
    }
}