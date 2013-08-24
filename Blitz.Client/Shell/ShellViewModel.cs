using System;
using System.Collections.ObjectModel;

using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.ModernUI.Presentation;
using Blitz.Client.ModernUI.Windows.Controls;
using Blitz.Client.Settings.Appearance;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Shell
{
    public class ShellViewModel : Workspace, IWindowViewModel
    {
        private readonly IViewService _viewService;

        public ObservableCollection<IToolBarItem> ToolBarItems { get; private set; }

        public LinkCollection TitleLinks { get; private set; } 

        public ShellViewModel(ILog log, IToolBarService toolBarService, IDispatcherService dispatcherService, IViewService viewService,
                              Func<AppearanceViewModel> appearanceViewModelFactory) 
            : base(log, dispatcherService)
        {
            _viewService = viewService;
            ToolBarItems = toolBarService.Items;

            var toolBarItem1 = new ToolBarButtonItem { DisplayName = "Test1"};
            ToolBarItems.Add(toolBarItem1);
            ToolBarItems.Add(new ToolBarButtonItem
            {
                DisplayName = "Test2",
                Command = new DelegateCommand(() =>
                {
                    toolBarItem1.IsVisible = !toolBarItem1.IsVisible;
                })
            });
            ToolBarItems.Add(new ToolBarButtonItem { DisplayName = "Test3" });

            TitleLinks = new LinkCollection();
            var appearanceLink = new Link
            {
                DisplayName = "Appearance",
                Command = new DelegateCommand(() =>
                {
                    var viewModel = appearanceViewModelFactory();
                    _viewService.ShowModel(viewModel);
                })
            };

            TitleLinks.Add(appearanceLink);
        }
    }
}