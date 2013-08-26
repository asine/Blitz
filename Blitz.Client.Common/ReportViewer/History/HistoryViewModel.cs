using System;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace Blitz.Client.Common.ReportViewer.History
{
    public class HistoryViewModel : Workspace
    {
        public BindableCollection<HistoryItemViewModel> Items { get; private set; }

        public DelegateCommand<HistoryItemViewModel> OpenCommand { get; private set; }

        public event EventHandler<DataEventArgs<long>> Open; 

        public HistoryViewModel(ILog log, IDispatcherService dispatcherService, BindableCollectionFactory bindableCollectionFactory) 
            : base(log, dispatcherService)
        {
            Items = bindableCollectionFactory.Get<HistoryItemViewModel>();

            DisplayName = "History";

            OpenCommand = new DelegateCommand<HistoryItemViewModel>(x => Open.SafeInvoke(this, new DataEventArgs<long>(x.Id)));
        }
    }
}