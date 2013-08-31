namespace Blitz.Client.Core.MVVM.ToolBar
{
    public interface IToolBarService
    {
        BindableCollection<IToolBarItem> Items { get; }
        ToolBarButtonItem CreateToolBarButtonItem();
    }
}