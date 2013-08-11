namespace Blitz.Client.Core.MVVM.ToolBar
{
    public interface IToolBarItem
    {
        string DisplayName { get; }
        bool IsVisible { get; set; }
    }
}