using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportViewer
{
    public abstract class ReportViewerService<TRequest, TResponse> : IReportViewerService<TRequest, TResponse>
    {
        private readonly IToolBarService _toolBarService;
        private readonly ILog _log;
        private readonly List<IToolBarItem> _toolBarItems;

        protected ReportViewerService(IToolBarService toolBarService, ILog log)
        {
            _toolBarService = toolBarService;
            _log = log;
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

        public abstract TRequest CreateRequest();

        public abstract Task<TResponse> GetHistory(TRequest request);

        public abstract Task<List<ReportViewerItemViewModel>> GenerateItemViewModels(TResponse response);

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