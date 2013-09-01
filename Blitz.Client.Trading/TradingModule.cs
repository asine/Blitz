using Blitz.Client.Common;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.Menu;
using Blitz.Client.ModernUI.Assets.Icons;
using Blitz.Client.Trading.QuoteBlotter;
using Blitz.Client.Trading.QuoteEdit;
using Blitz.Common.Core;
using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Edit;

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
                .RegisterTransient<IQuoteEditService, QuoteEditService>();

            CreateMenu();
        }

        private void CreateMenu()
        {
            var tradingMenuItem = _menuService.CreateMenuGroupItem();
            tradingMenuItem.DisplayName = "Trading";

            var newReportMenuItem = _menuService.CreateMenuButtonItem();
            newReportMenuItem.DisplayName = "New Blotter";
            newReportMenuItem.ImageName = IconNames.NEW;
            newReportMenuItem.Command = new DelegateCommand(() =>
            {
                _log.Info("Adding Blotter to Main region");
                var viewModel = _viewService.RegionBuilder<QuoteBlotterViewModel>()
                    .WithScope()
                    .Show(RegionNames.MAIN);
                ((ISupportActivationState)viewModel).Activate();
            });
            tradingMenuItem.Items.Add(newReportMenuItem);

            _menuService.Items.Add(tradingMenuItem);
        }
    }
}