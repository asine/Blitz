using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

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