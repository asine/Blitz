using System;

using Blitz.Client.Trading.Quote.Blotter;
using Blitz.Client.Trading.Security.Chart;

using Common.Logging;

using Naru.Core;
using Naru.WPF.Command;
using Naru.WPF.Menu;
using Naru.WPF.Assets.Icons;

namespace Blitz.Client.Trading
{
    public class TradingStartable
    {
        private readonly ILog _log;
        private readonly IMenuService _menuService;
        private readonly IEventStream _eventStream;
        private readonly Func<QuoteBlotterViewModel> _quoteBlotterViewModelFactory;
        private readonly Func<ChartViewModel> _chartViewModelFactory;

        public TradingStartable(ILog log, IMenuService menuService, IEventStream eventStream,
                                Func<QuoteBlotterViewModel> quoteBlotterViewModelFactory,
                                Func<ChartViewModel> chartViewModelFactory)
        {
            _log = log;
            _menuService = menuService;
            _eventStream = eventStream;
            _quoteBlotterViewModelFactory = quoteBlotterViewModelFactory;
            _chartViewModelFactory = chartViewModelFactory;
        }

        public void Start()
        {
            CreateMenu();
        }

        private void CreateMenu()
        {
            var tradingMenuItem = _menuService.CreateMenuGroupItem();
            tradingMenuItem.DisplayName = "Trading";
            _menuService.Items.Add(tradingMenuItem);

            var newReportMenuItem = _menuService.CreateMenuButtonItem();
            newReportMenuItem.DisplayName = "New Blotter";
            newReportMenuItem.ImageName = IconNames.NEW;
            newReportMenuItem.Command = new DelegateCommand(
                () =>
                {
                    _log.Debug("Adding Blotter to Main region");

                    var quoteBlotterViewModel = _quoteBlotterViewModelFactory();
                    _eventStream.Push(quoteBlotterViewModel);
                    quoteBlotterViewModel.ActivationStateViewModel.Activate();
                });
            tradingMenuItem.Items.Add(newReportMenuItem);

            var chartMenuItem = _menuService.CreateMenuButtonItem();
            chartMenuItem.DisplayName = "Chart";
            chartMenuItem.ImageName = IconNames.NEW;
            chartMenuItem.Command = new DelegateCommand(
                () =>
                {
                    _log.Debug("Adding Chart to Main region");

                    var chartViewModel = _chartViewModelFactory();
                    _eventStream.Push(chartViewModel);
                    chartViewModel.ActivationStateViewModel.Activate();
                });
            tradingMenuItem.Items.Add(chartMenuItem);
        }
    }
}