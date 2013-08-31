using System;

namespace Blitz.Client.Core.MVVM.Menu
{
    public class MenuService : IMenuService
    {
        private readonly Func<MenuButtonItem> _menuButtonItemFactory;
        private readonly Func<MenuGroupItem> _menuGroupItemFactory;
        private readonly Func<MenuSeperatorItem> _menuSeperatorItemFactory;

        public BindableCollection<IMenuItem> Items { get; private set; }

        public MenuService(BindableCollectionFactory bindableCollectionFactory,
                           Func<MenuButtonItem> menuButtonItemFactory,
                           Func<MenuGroupItem> menuGroupItemFactory,
                           Func<MenuSeperatorItem> menuSeperatorItemFactory)
        {
            _menuButtonItemFactory = menuButtonItemFactory;
            _menuGroupItemFactory = menuGroupItemFactory;
            _menuSeperatorItemFactory = menuSeperatorItemFactory;
            Items = bindableCollectionFactory.Get<IMenuItem>();
        }

        public MenuButtonItem CreateMenuButtonItem()
        {
            return _menuButtonItemFactory();
        }

        public MenuGroupItem CreateMenuGroupItem()
        {
            return _menuGroupItemFactory();
        }

        public MenuSeperatorItem CreateMenuSeperatorItem()
        {
            return _menuSeperatorItemFactory();
        }
    }
}