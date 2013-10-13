using System;

using Common.Logging;

using Naru.WPF.ModernUI.Presentation;
using Naru.WPF.ModernUI.Windows.Controls;
using Naru.WPF.MVVM;
using Naru.WPF.MVVM.Menu;
using Naru.WPF.MVVM.ToolBar;

using Blitz.Client.Settings.Appearance;

using Naru.WPF.Scheduler;

namespace Blitz.Client.Shell
{
    public class ShellViewModel : Workspace, IWindowViewModel
    {
        public BindableCollection<IMenuItem> MenuItems { get; private set; }

        public BindableCollection<IToolBarItem> ToolBarItems { get; private set; }

        public LinkCollection TitleLinks { get; private set; } 

        public ShellViewModel(ILog log, IScheduler scheduler, IViewService viewService, IToolBarService toolBarService, IMenuService menuService, 
            Func<AppearanceViewModel> appearanceViewModelFactory) 
            : base(log, scheduler, viewService)
        {
            ToolBarItems = toolBarService.Items;
            MenuItems = menuService.Items;

            TitleLinks = new LinkCollection();

            TitleLinks.Add(new Link
            {
                DisplayName = "Appearance",
                Command = new DelegateCommand(() =>
                {
                    var viewModel1 = appearanceViewModelFactory();
                    ViewService.ShowModal(viewModel1);
                })
            });
        }
    }
}