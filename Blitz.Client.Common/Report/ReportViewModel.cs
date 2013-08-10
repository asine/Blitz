using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.Report
{
    public abstract class ReportViewModel<TRunnerViewModel, TViewerViewModel> : ViewModelBase
        where TRunnerViewModel : IViewModel
        where TViewerViewModel : IViewModel
    {
        private readonly IViewService _viewService;

        protected ReportViewModel(ILog log, IViewService viewService)
            : base(log)
        {
            _viewService = viewService;
        }

        protected override void OnInitialise()
        {
            _viewService.AddToRegion<TRunnerViewModel>(RegionNames.REPORT);
            _viewService.AddToRegion<TViewerViewModel>(RegionNames.REPORT);
        }
    }
}