using System;

using Common.Logging;

using Naru.Core;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
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

        public HistoryViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                                BindableCollection<HistoryItemViewModel> itemsCollection) 
            : base(log, scheduler, standardDialog)
        {
            Items = itemsCollection;

            this.SetupHeader(scheduler, "History");

            OpenCommand = new DelegateCommand<HistoryItemViewModel>(x => Open.SafeInvoke(this, new DataEventArgs<long>(x.Id)));
        }
    }
}