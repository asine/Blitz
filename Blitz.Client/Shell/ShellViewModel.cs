using System;

using Common.Logging;

using Naru.WPF.ModernUI.Presentation;
using Naru.WPF.ModernUI.Windows.Controls;
using Naru.WPF.MVVM;
using Naru.WPF.MVVM.Menu;
using Naru.WPF.MVVM.ToolBar;

using Blitz.Client.Settings.Appearance;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Shell
{
    public class ShellViewModel : Workspace, IWindowViewModel
    {
        private readonly IViewService _viewService;

        public BindableCollection<IMenuItem> MenuItems { get; private set; }

        public BindableCollection<IToolBarItem> ToolBarItems { get; private set; }

        public LinkCollection TitleLinks { get; private set; } 

        public ShellViewModel(ILog log, IToolBarService toolBarService, IMenuService menuService, IDispatcherService dispatcherService, 
            IViewService viewService, Func<AppearanceViewModel> appearanceViewModelFactory) 
            : base(log, dispatcherService)
        {
            _viewService = viewService;
            ToolBarItems = toolBarService.Items;
            MenuItems = menuService.Items;

            TitleLinks = new LinkCollection();

            TitleLinks.Add(new Link
            {
                DisplayName = "Appearance",
                Command = new DelegateCommand(() =>
                {
                    var viewModel1 = appearanceViewModelFactory();
                    _viewService.ShowModal(viewModel1);
                })
            });
        }
    }
}