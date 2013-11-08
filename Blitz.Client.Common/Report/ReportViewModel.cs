using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.Report
{
    public abstract class ReportViewModel : Workspace
    {
        public BindableCollection<IViewModel> Items { get; private set; } 

        protected ReportViewModel(ILog log, IViewService viewService, ISchedulerProvider scheduler,
                                  BindableCollection<IViewModel> itemsCollection)
            : base(log, scheduler, viewService)
        {
            Items = itemsCollection;
        }
    }
}