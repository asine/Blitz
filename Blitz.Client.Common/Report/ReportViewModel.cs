using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.Report
{
    public abstract class ReportViewModel<TRunnerViewModel, TViewerViewModel> : Workspace
        where TRunnerViewModel : IViewModel, ISupportActivationState
        where TViewerViewModel : IViewModel, ISupportActivationState
    {
        private readonly IViewService _viewService;

        protected ReportViewModel(ILog log, IViewService viewService)
            : base(log)
        {
            _viewService = viewService;
        }

        protected override void OnInitialise()
        {
            _viewService.AddToRegion<TRunnerViewModel>(RegionNames.REPORT, viewModel => Disposables.Add(this.SyncViewModelActivationStates(viewModel)));

            _viewService.AddToRegion<TViewerViewModel>(RegionNames.REPORT, viewModel => Disposables.Add(this.SyncViewModelActivationStates(viewModel)));
        }
    }
}