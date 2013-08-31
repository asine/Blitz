namespace Blitz.Client.Core.MVVM.Menu
{
    public interface IMenuService
    {
        BindableCollection<IMenuItem> Items { get; }

        MenuButtonItem CreateMenuButtonItem();

        MenuGroupItem CreateMenuGroupItem();

        MenuSeperatorItem CreateMenuSeperatorItem();
    }
}