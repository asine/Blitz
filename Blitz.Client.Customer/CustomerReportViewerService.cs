using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer
{
    public class CustomerReportViewerService : ReportViewerServiceBase<GetHistoryRequest, GetHistoryResponse>
    {
        private readonly IRequestTask _requestTask;
        private readonly IToolBarService _toolBarService;

        private readonly List<IToolBarItem> _toolBarItems;

        public CustomerReportViewerService(IRequestTask requestTask, IToolBarService toolBarService)
        {
            _requestTask = requestTask;
            _toolBarService = toolBarService;

            _toolBarItems = new List<IToolBarItem>();
            _toolBarItems.Add(new ToolBarButtonItem { DisplayName = "Customer Viewer Test 1", IsVisible = false });

            foreach (var toolBarItem in _toolBarItems)
            {
                _toolBarService.Items.Add(toolBarItem);
            }
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

        public override void OnActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = true;
            }
        }

        public override void OnDeActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = false;
            }
        }

        public override void CleanUp()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                _toolBarService.Items.Remove(toolBarItem);
            }
        }
    }
}