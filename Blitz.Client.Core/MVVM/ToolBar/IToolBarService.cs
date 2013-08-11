using System.Collections.ObjectModel;

namespace Blitz.Client.Core.MVVM.ToolBar
{
    public interface IToolBarService
    {
        ObservableCollection<IToolBarItem> Items { get; }
    }
}