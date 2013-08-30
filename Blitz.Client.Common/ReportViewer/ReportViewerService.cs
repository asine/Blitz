using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer.History;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportViewer
{
    public abstract class ReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
        : IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        private readonly IToolBarService _toolBarService;
        protected readonly ILog Log;
        private readonly List<IToolBarItem> _toolBarItems;

        protected ReportViewerService(IToolBarService toolBarService, ILog log)
        {
            _toolBarService = toolBarService;
            Log = log;
            _toolBarItems = new List<IToolBarItem>();

            _toolBarItems.AddRange(AddToolBarItems());

            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarService.Items.Add(toolBarItem);
            }
        }

        protected virtual IEnumerable<IToolBarItem> AddToolBarItems()
        {
            return Enumerable.Empty<IToolBarItem>();
        }

        public abstract THistoryRequest CreateHistoryRequest();

        public abstract Task<THistoryResponse> GetHistoryAsync(THistoryRequest request);

        public abstract Task<List<HistoryItemViewModel>> GenerateHistoryItemViewModelsAsync(THistoryResponse response);

        public abstract TReportRequest CreateReportRequest(long id);

        public abstract Task<TReportResponse> GenerateReportAsync(TReportRequest request);

        public abstract Task<List<IViewModel>> GenerateReportViewModelsAsync(TReportResponse response);

        public virtual void OnActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = true;
            }
        }

        public virtual void OnDeActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = false;
            }
        }

        public virtual void CleanUp()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                _toolBarService.Items.Remove(toolBarItem);
            }
        }
    }
}