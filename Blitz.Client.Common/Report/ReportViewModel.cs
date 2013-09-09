using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.TPL;

namespace Blitz.Client.Common.Report
{
    public abstract class ReportViewModel : Workspace
    {
        protected readonly IViewService ViewService;

        protected ReportViewModel(ILog log, IViewService viewService, IScheduler scheduler)
            : base(log, scheduler)
        {
            ViewService = viewService;
        }
    }
}