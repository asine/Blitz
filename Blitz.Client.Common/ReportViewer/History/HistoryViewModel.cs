using System;

using Common.Logging;

using Naru.WPF.MVVM;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

using Naru.WPF.Scheduler;

namespace Blitz.Client.Common.ReportViewer.History
{
    public class HistoryViewModel : Workspace
    {
        public BindableCollection<HistoryItemViewModel> Items { get; private set; }

        public DelegateCommand<HistoryItemViewModel> OpenCommand { get; private set; }

        public event EventHandler<DataEventArgs<long>> Open;

        public HistoryViewModel(ILog log, IScheduler scheduler, IViewService viewService, BindableCollectionFactory bindableCollectionFactory) 
            : base(log, scheduler, viewService)
        {
            Items = bindableCollectionFactory.Get<HistoryItemViewModel>();

            this.SetupHeader("History");

            OpenCommand = new DelegateCommand<HistoryItemViewModel>(x => Open.SafeInvoke(this, new DataEventArgs<long>(x.Id)));
        }
    }
}