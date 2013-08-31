using System;

namespace Blitz.Client.Core.MVVM.ToolBar
{
    public class ToolBarService : IToolBarService
    {
        private readonly Func<ToolBarButtonItem> _toolBarButtonItemFactory;
        public BindableCollection<IToolBarItem> Items { get; private set; }

        public ToolBarService(BindableCollectionFactory bindableCollectionFactory,
                              Func<ToolBarButtonItem> toolBarButtonItemFactory)
        {
            _toolBarButtonItemFactory = toolBarButtonItemFactory;
            Items = bindableCollectionFactory.Get<IToolBarItem>();
        }

        public ToolBarButtonItem CreateToolBarButtonItem()
        {
            return _toolBarButtonItemFactory();
        }
    }
}