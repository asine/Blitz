using System.Threading.Tasks;

using Blitz.Client.Common.Report;

using Common.Logging;

using Naru.RX;
using Naru.TPL;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Blitz.Client.Customer.ReportRunner;
using Blitz.Client.Customer.Reportviewer;

using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer.Report
{
    [UseView(typeof(ReportView))]
    public class ReportViewModel : Common.Report.ReportViewModel
    {
        private readonly ReportRunnerViewModel _reportRunnerViewModel;
        private readonly ReportViewerViewModel _reportViewerViewModel;

        public ReportViewModel(ILog log, IStandardDialog standardDialog, ISchedulerProvider scheduler,
                               BindableCollection<IViewModel> itemsCollection,
                               ReportRunnerViewModel reportRunnerViewModel,
                               ReportViewerViewModel reportViewerViewModel)
            : base(log, standardDialog, scheduler, itemsCollection)
        {
            _reportRunnerViewModel = reportRunnerViewModel;
            this.SyncViewModelActivationStates(_reportRunnerViewModel).AddDisposable(Disposables);

            _reportViewerViewModel = reportViewerViewModel;
            this.SyncViewModelActivationStates(_reportViewerViewModel).AddDisposable(Disposables);
        }

        protected override Task OnInitialise()
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             Items.Add(_reportRunnerViewModel);
                                             Items.Add(_reportViewerViewModel);
                                         }, Scheduler.Dispatcher.TPL);
        }
    }
}