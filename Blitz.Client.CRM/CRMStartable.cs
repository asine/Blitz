using System;

using Blitz.Client.CRM.Client.Edit;

using Common.Logging;

using Naru.Core;
using Naru.WPF.Assets.Icons;
using Naru.WPF.Command;
using Naru.WPF.Menu;
using Naru.WPF.ViewModel;

namespace Blitz.Client.CRM
{
    public class CRMStartable
    {
        private readonly ILog _log;
        private readonly IMenuService _menuService;
        private readonly IEventStream _eventStream;
        private readonly Func<ClientEditViewModel> _clientEditViewModelFactory;

        public CRMStartable(ILog log, IMenuService menuService, IEventStream eventStream,
                            Func<ClientEditViewModel> clientEditViewModelFactory)
        {
            _log = log;
            _menuService = menuService;
            _eventStream = eventStream;
            _clientEditViewModelFactory = clientEditViewModelFactory;
        }

        public void Start()
        {
            CreateMenu();
        }

        private void CreateMenu()
        {
            var crmMenuItem = _menuService.CreateMenuGroupItem();
            crmMenuItem.DisplayName = "CRM";
            _menuService.Items.Add(crmMenuItem);

            var newReportMenuItem = _menuService.CreateMenuButtonItem();
            newReportMenuItem.DisplayName = "New Client";
            newReportMenuItem.ImageName = IconNames.NEW;
            newReportMenuItem.Command = new DelegateCommand(
                () =>
                {
                    var clientEditViewModel = _clientEditViewModelFactory();
                    _eventStream.Push(clientEditViewModel);
                    ((ISupportActivationState)clientEditViewModel).Activate();
                });
            crmMenuItem.Items.Add(newReportMenuItem);
        }
    }
}