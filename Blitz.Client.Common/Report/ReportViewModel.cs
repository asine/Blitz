using Common.Logging;

using Naru.WPF.MVVM;

namespace Blitz.Client.Common.Report
{
    public abstract class ReportViewModel : Workspace
    {
        protected readonly IViewService ViewService;

        protected ReportViewModel(ILog log, IViewService viewService, IDispatcherService dispatcherService)
            : base(log, dispatcherService)
        {
            ViewService = viewService;
        }
    }
}