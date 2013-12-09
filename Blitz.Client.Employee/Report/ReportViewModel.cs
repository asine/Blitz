using System.Threading.Tasks;

using Blitz.Client.Common.Report;

using Common.Logging;

using Naru.RX;
using Naru.TPL;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Blitz.Client.Employee.ReportRunner;

using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Employee.Report
{
    [UseView(typeof(ReportView))]
    public class ReportViewModel : Common.Report.ReportViewModel
    {
        private readonly ReportRunnerViewModel _reportRunnerViewModel;

        public ReportViewModel(ILog log, IStandardDialog standardDialog, ISchedulerProvider scheduler,
                               BindableCollection<IViewModel> itemsCollection,
                               ReportRunnerViewModel reportRunnerViewModel)
            : base(log, standardDialog, scheduler, itemsCollection)
        {
            _reportRunnerViewModel = reportRunnerViewModel;
            this.SyncViewModelActivationStates(_reportRunnerViewModel).AddDisposable(Disposables);
        }

        protected override Task OnInitialise()
        {
            return Task.Factory.StartNew(() => Items.Add(_reportRunnerViewModel), Scheduler.Dispatcher.TPL);
        }
    }
}