using System;
using System.Collections.ObjectModel;

using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.Dialog;
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
            TitleLinks = new LinkCollection();

            ToolBarItems.Add(new ToolBarButtonItem
            {
                DisplayName = "Dialog Test 1",
                Command = new DelegateCommand(() =>
                {
                    var answer = viewService.DialogBuilder()
                        .WithDialogType(DialogType.Information)
                        .WithAnswers(Answer.Ok, Answer.Cancel)
                        .WithTitle("Something else interesting happened")
                        .WithMessage("No really.....")
                        .Show();

                    log.Info(string.Format("Dialog selection - {0}", answer));
                })
            });

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