using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.Report
{
    public abstract class ReportViewModel : Workspace
    {
        protected ReportViewModel(ILog log, IViewService viewService, ISchedulerProvider scheduler)
            : base(log, scheduler, viewService)
        {
        }
    }
}