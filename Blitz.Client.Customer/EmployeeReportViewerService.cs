using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer
{
    public class EmployeeReportViewerService : ReportViewerService<GetHistoryRequest, GetHistoryResponse>
    {
        private readonly IRequestTask _requestTask;

        public EmployeeReportViewerService(IToolBarService toolBarService, ILog log, IRequestTask requestTask) 
            : base(toolBarService, log)
        {
            _requestTask = requestTask;
        }

        public override GetHistoryRequest CreateRequest()
        {
            return new GetHistoryRequest();
        }

        public override Task<GetHistoryResponse> GetHistory(GetHistoryRequest request)
        {
            return _requestTask.Get<GetHistoryRequest, GetHistoryResponse>(request);
        }

        public override Task<List<ReportViewerItemViewModel>> GenerateItemViewModels(GetHistoryResponse response)
        {
            return Task.Factory.StartNew(
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