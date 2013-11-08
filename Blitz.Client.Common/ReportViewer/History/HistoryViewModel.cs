using System;

using Common.Logging;

using Naru.Core;
using Naru.WPF.Command;
using Naru.WPF.MVVM;

using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.ReportViewer.History
{
    public class HistoryViewModel : Workspace
    {
        public BindableCollection<HistoryItemViewModel> Items { get; private set; }

        public DelegateCommand<HistoryItemViewModel> OpenCommand { get; private set; }

        public event EventHandler<DataEventArgs<long>> Open;

        public HistoryViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService,
                                BindableCollection<HistoryItemViewModel> itemsCollection) 
            : base(log, scheduler, viewService)
        {
            Items = itemsCollection;

            this.SetupHeader("History");

            OpenCommand = new DelegateCommand<HistoryItemViewModel>(x => Open.SafeInvoke(this, new DataEventArgs<long>(x.Id)));
        }
    }
}