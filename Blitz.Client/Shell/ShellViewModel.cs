using System.Collections.ObjectModel;

using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Shell
{
    public class ShellViewModel : Workspace
    {
        public ObservableCollection<IToolBarItem> ToolBarItems { get; private set; }

        public ShellViewModel(ILog log, IToolBarService toolBarService, IDispatcherService dispatcherService) 
            : base(log, dispatcherService)
        {
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
        }
    }
}