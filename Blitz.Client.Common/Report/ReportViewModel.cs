using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;

namespace Blitz.Client.Common.Report
{
    public abstract class ReportViewModel : Workspace
    {
        protected ReportViewModel(ILog log, IViewService viewService, IScheduler scheduler)
            : base(log, scheduler, viewService)
        {
        }
    }
}