using Common.Logging;

using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.Report
{
    [UseView(typeof(ReportView))]
    public abstract class ReportViewModel : Workspace
    {
        public BindableCollection<IViewModel> Items { get; private set; }

        protected ReportViewModel(ILog log, IStandardDialog standardDialog, ISchedulerProvider scheduler,
                                  BindableCollection<IViewModel> itemsCollection)
            : base(log, scheduler, standardDialog)
        {
            Items = itemsCollection;
        }
    }
}