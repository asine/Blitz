using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportRunner
{
    public abstract class ReportRunnerService<TReportParameterViewModel, TRequest, TResponse> 
        : IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        private readonly IToolBarService _toolBarService;
        protected readonly ILog Log;
        private readonly List<IToolBarItem> _toolBarItems;

        protected ReportRunnerService(IToolBarService toolBarService, ILog log)
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

        public abstract Task ConfigureParameterViewModelAsync(TReportParameterViewModel viewModel);

        public abstract TRequest CreateRequest(TReportParameterViewModel reportParameterViewModel);

        public abstract Task<TResponse> GenerateAsync(TRequest request);

        public abstract Task<List<IViewModel>> GenerateDataViewModelsAsync(TResponse response);

        public abstract void ExportToExcel(TResponse response);

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