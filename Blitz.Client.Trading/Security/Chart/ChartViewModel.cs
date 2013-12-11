using System;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;

using Blitz.Common.Trading.Security.Chart;

using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Trading.Security.Chart
{
    public class ChartViewModel : Workspace
    {
        private readonly IChartService _service;

        public BindableCollection<HistoricalDataDto> Items { get; private set; }

        #region Ticker

        private string _ticker;

        public string Ticker
        {
            get { return _ticker; }
            set
            {
                if (value == _ticker) return;
                _ticker = value;
                RaisePropertyChanged(() => Ticker);

                GoCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public DelegateCommand GoCommand { get; private set; }

        public ChartViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                              BindableCollection<HistoricalDataDto> itemsCollection, IChartService service)
            : base(log, scheduler, standardDialog)
        {
            _service = service;
            Disposables.Add(service);

            this.SetupHeader(Scheduler, "Chart");

            Items = itemsCollection;
            GoCommand = new DelegateCommand(GetData, () => !string.IsNullOrEmpty(Ticker));
        }

        private void GetData()
        {
            BusyViewModel.ActiveAsync(string.Format("... Loading {0} ...", _ticker))
                         .Then(() => Items.ClearAsync(), Scheduler.Dispatcher.TPL)
                         .Then(() => _service.GetDataAsync(_ticker, DateTime.Now.AddMonths(-1), DateTime.Now), Scheduler.Task.TPL)
                         .Then(data => Items.AddRangeAsync(data), Scheduler.Dispatcher.TPL)
                         .LogException(Log)
                         .CatchAndHandle(x => StandardDialog.Error("Error", "Problem getting chart data"), Scheduler.Task.TPL)
                         .Finally(BusyViewModel.InActive, Scheduler.Task.TPL);
        }
    }
}