using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.TPL;
using Blitz.Common.Core;
using Blitz.Common.Trading.Security.Chart;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Trading.Security.Chart
{
    public class ChartViewModel : Workspace
    {
        private readonly ITaskScheduler _taskScheduler;
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

        public ChartViewModel(ILog log, IDispatcherService dispatcherService, ITaskScheduler taskScheduler, IViewService viewService, 
            BindableCollectionFactory bindableCollectionFactory, IChartService service)
            : base(log, dispatcherService)
        {
            _taskScheduler = taskScheduler;
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
                .SelectMany(() => Items.Clear())
                .SelectMany(() => _service.GetData(_ticker, DateTime.Now.AddMonths(-1), DateTime.Now))
                .SelectMany(data => Items.AddRange(data))
                .LogException(Log)
                .CatchAndHandle(x => _viewService.StandardDialogBuilder().Error("Error", "Problem initialising parameters"), _taskScheduler.Default)
                .Finally(Idle, _taskScheduler.Default);
        }
    }
}