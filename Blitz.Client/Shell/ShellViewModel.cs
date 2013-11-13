using System;
using System.Reactive.Linq;

using Common.Logging;

using Naru.Core;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
using Naru.WPF.Menu;
using Naru.WPF.ModernUI.Presentation;
using Naru.WPF.ModernUI.Windows.Controls;
using Naru.WPF.MVVM;

using Blitz.Client.Settings.Appearance;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Shell
{
    public class ShellViewModel : Workspace, IWindowViewModel
    {
        public BindableCollection<IMenuItem> MenuItems { get; private set; }

        public BindableCollection<IToolBarItem> ToolBarItems { get; private set; }

        public LinkCollection TitleLinks { get; private set; }

        public BindableCollection<IViewModel> Items { get; private set; }

        public ShellViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog, IViewService viewService,
                              IToolBarService toolBarService, IMenuService menuService, IEventStream eventStream,
                              Func<AppearanceViewModel> appearanceViewModelFactory,
                              BindableCollection<IViewModel> itemsCollection)
            : base(log, scheduler, standardDialog)
        {
            ToolBarItems = toolBarService.Items;
            MenuItems = menuService.Items;
            Items = itemsCollection;

            TitleLinks = new LinkCollection();

            TitleLinks.Add(new Link
                           {
                               DisplayName = "Appearance",
                               Command = new DelegateCommand(() =>
                                                             {
                                                                 var viewModel1 = appearanceViewModelFactory();
                                                                 viewService.ShowModal(viewModel1);
                                                             })
                           });

            eventStream.Of<IViewModel>()
                .ObserveOn(Scheduler.Dispatcher.RX)
                .Subscribe(x => Items.Add(x));
        }
    }
}