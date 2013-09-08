using Blitz.Client.Common;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.Menu;
using Naru.WPF.ModernUI.Assets.Icons;
using Blitz.Client.Trading.Quote.Blotter;
using Blitz.Client.Trading.Quote.Edit;
using Blitz.Client.Trading.Security.Chart;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Blitz.Client.Trading
{
    public class TradingModule : IModule
    {
        private readonly ILog _log;
        private readonly IViewService _viewService;
        private readonly IUnityContainer _container;
        private readonly IMenuService _menuService;

        public TradingModule(ILog log, IViewService viewService, IUnityContainer container, IMenuService menuService)
        {
            _log = log;
            _viewService = viewService;
            _container = container;
            _menuService = menuService;
        }

        public void Initialize()
        {
            _container
                .RegisterTransient<IQuoteBlotterService, QuoteBlotterService>()
                .RegisterTransient<IQuoteEditService, QuoteEditService>()
                .RegisterTransient<IChartService, ChartService>();

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
            newReportMenuItem.Command = new DelegateCommand(() =>
            {
                _log.Debug("Adding Blotter to Main region");
                var viewModel = _viewService.RegionBuilder<QuoteBlotterViewModel>()
                    .WithScope()
                    .Show(RegionNames.MAIN);
                ((ISupportActivationState)viewModel).Activate();
            });
            tradingMenuItem.Items.Add(newReportMenuItem);

            var chartMenuItem = _menuService.CreateMenuButtonItem();
            chartMenuItem.DisplayName = "Chart";
            chartMenuItem.ImageName = IconNames.NEW;
            chartMenuItem.Command = new DelegateCommand(() =>
            {
                _log.Debug("Adding Chart to Main region");
                var viewModel = _viewService.RegionBuilder<ChartViewModel>()
                    .WithScope()
                    .Show(RegionNames.MAIN);
                ((ISupportActivationState)viewModel).Activate();
            });
            tradingMenuItem.Items.Add(chartMenuItem);
        }
    }
}