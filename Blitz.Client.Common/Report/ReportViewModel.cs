using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.Report
{
    public abstract class ReportViewModel<TRunnerViewModel, TViewerViewModel> : Workspace
        where TRunnerViewModel : IViewModel, ISupportActivationState
        where TViewerViewModel : IViewModel, ISupportActivationState
    {
        private readonly IViewService _viewService;

        protected ReportViewModel(ILog log, IViewService viewService, IDispatcherService dispatcherService)
            : base(log, dispatcherService)
        {
            _viewService = viewService;
        }

        protected override void OnInitialise()
        {
            _viewService.RegionBuilder<TRunnerViewModel>()
                .WithInitialisation(viewModel => Disposables.Add(this.SyncViewModelActivationStates(viewModel)))
                .Show(RegionNames.REPORT);

            _viewService.RegionBuilder<TViewerViewModel>()
                .WithInitialisation(viewModel => Disposables.Add(this.SyncViewModelDeActivation(viewModel)))
                .Show(RegionNames.REPORT);
        }
    }
}