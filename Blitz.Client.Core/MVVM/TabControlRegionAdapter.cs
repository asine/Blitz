using System;
using System.Collections.Specialized;
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
            region.Views.CollectionChanged += (s, e) => RegionCollectionChanged(regionTarget, e);

            regionTarget.SelectionChanged += (s, e) => TabControlSelectionChanged(e);
        }

        private static void RegionCollectionChanged(TabControl regionTarget, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (FrameworkElement view in e.NewItems)
                {
                    var viewModel = view.DataContext as IViewModel;
                    if (viewModel == null) continue;

                    var tabItem = new TabItem {Header = viewModel.DisplayName, Content = view};

                    ConnectUpActivation(viewModel, regionTarget, tabItem);
                    ConnectUpClosing(viewModel, regionTarget, tabItem);

                    regionTarget.Items.Add(tabItem);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var view in e.OldItems)
                {
                    TabItem tabItem = null;

                    foreach (TabItem item in regionTarget.Items)
                    {
                        if (item.Content == view)
                            tabItem = item;
                    }

                    if (tabItem != null)
                        regionTarget.Items.Remove(tabItem);
                }
            }
        }

        private static void TabControlSelectionChanged(SelectionChangedEventArgs e)
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
        }

        private static void Activate(object item)
        {
            var view = item as FrameworkElement;
            if (view == null) return;

            var viewModel = view.DataContext as ISupportActivationState;
            if (viewModel == null) return;

            viewModel.Activate();
        }

        private static void DeActivate(object item)
        {
            var view = item as FrameworkElement;
            if (view == null) return;

            var viewModel = view.DataContext as ISupportActivationState;
            if (viewModel == null) return;

            viewModel.DeActivate();
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        private static void ConnectUpClosing(IViewModel viewModel, TabControl tabControl, TabItem tabItem)
        {
            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            // ViewModel is closed
            EventHandler supportClosingOnClosed = null;
            supportClosingOnClosed = (s, e) =>
            {
                tabControl.Items.Remove(tabItem);

                if (supportClosingOnClosed != null)
                {
                    supportClosing.OnClosed -= supportClosingOnClosed;
                }
            };
            supportClosing.OnClosed += supportClosingOnClosed;
        }

        private static void ConnectUpActivation(IViewModel viewModel, TabControl tabControl, TabItem tabItem)
        {
            var supportActivationState = viewModel as ISupportActivationState;
            if (supportActivationState == null) return;

            supportActivationState.OnActivationStateChanged += (s, e) =>
            {
                if (supportActivationState.IsActive)
                    tabControl.SelectedItem = tabItem;
            };
        }
    }
}