using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Regions;

namespace Blitz.Client.Core.MVVM
{
    public class TabControlRegionAdapter : RegionAdapterBase<TabControl>
    {
        public TabControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement view in e.NewItems)
                    {
                        var viewModel = view.DataContext as IViewModel;
                        if (viewModel == null) continue;

                        var tabItem = new TabItem {Header = viewModel.DisplayName, Content = view};
                        regionTarget.Items.Add(tabItem);
                    }
                }

                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    foreach (var view in e.OldItems)
                    {
                        regionTarget.Items.Remove(view);
                    }
                }
            };

            regionTarget.SelectionChanged += (s, e) =>
            {
                foreach (var obj in e.AddedItems)
                {
                    var tabItem = obj as TabItem;
                    if (tabItem == null) continue;

                    var view = tabItem.Content;
                    Activate(view);
                    break;
                }

                foreach (var obj in e.RemovedItems)
                {
                    var tabItem = obj as TabItem;
                    if (tabItem == null) continue;

                    var view = tabItem.Content;
                    DeActivate(view);
                }
            };
        }

        private static void Activate(object item)
        {
            var view = item as FrameworkElement;
            if (view == null) return;

            var viewModel = view.DataContext as IViewModel;
            if (viewModel == null) return;

            viewModel.Activate();
        }

        private static void DeActivate(object item)
        {
            var view = item as FrameworkElement;
            if (view == null) return;

            var viewModel = view.DataContext as IViewModel;
            if (viewModel == null) return;

            viewModel.DeActivate();
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}