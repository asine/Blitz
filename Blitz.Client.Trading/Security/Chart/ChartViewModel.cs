using System;

using Common.Logging;

using Naru.WPF.MVVM;

using Blitz.Common.Trading.Security.Chart;

using Microsoft.Practices.Prism.Commands;

using Naru.WPF.TPL;

namespace Blitz.Client.Trading.Security.Chart
{
    public class ChartViewModel : Workspace
    {
        private readonly IScheduler _scheduler;
        private readonly IViewService _viewService;
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

        public ChartViewModel(ILog log, IScheduler scheduler, IViewService viewService, 
            BindableCollectionFactory bindableCollectionFactory, IChartService service)
            : base(log, scheduler)
        {
            _scheduler = scheduler;
            _viewService = viewService;

            _service = service;
            Disposables.Add(service);

            DisplayName = "Chart";

            Items = bindableCollectionFactory.Get<HistoricalDataDto>();
            GoCommand = new DelegateCommand(GetData,() => !string.IsNullOrEmpty(Ticker));
        }

        private void GetData()
        {
            BusyAsync(string.Format("... Loading {0} ...", _ticker))
                .Do(() => Items.ClearAsync())
                .SelectMany(() => _service.GetDataAsync(_ticker, DateTime.Now.AddMonths(-1), DateTime.Now))
                .SelectMany(data => Items.AddRange(data))
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem getting chart data"), _scheduler.Task)
                .Finally(Idle, _scheduler.Task);
        }
    }
}