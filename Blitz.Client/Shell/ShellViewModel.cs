using System;

using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.Dialog;
using Blitz.Client.Core.MVVM.Menu;
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

            var toolBarButtonItem = toolBarService.CreateToolBarButtonItem();
            toolBarButtonItem.DisplayName = "Dialog Test 1";
            toolBarButtonItem.Command = new DelegateCommand(() =>
            {
                var answer = viewService.DialogBuilder()
                    .WithDialogType(DialogType.Information)
                    .WithAnswers(Answer.Ok, Answer.Cancel)
                    .WithTitle("Something else interesting happened")
                    .WithMessage("No really.....")
                    .Show();

                log.Info(string.Format("Dialog selection - {0}", answer));
            });
            ToolBarItems.Add(toolBarButtonItem);

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