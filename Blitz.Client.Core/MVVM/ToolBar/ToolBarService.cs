using System.Collections.ObjectModel;

namespace Blitz.Client.Core.MVVM.ToolBar
{
    public class ToolBarService : IToolBarService
    {
        public ObservableCollection<IToolBarItem> Items { get; private set; }

        public ToolBarService()
        {
            Items = new ObservableCollection<IToolBarItem>();
        }
    }
}