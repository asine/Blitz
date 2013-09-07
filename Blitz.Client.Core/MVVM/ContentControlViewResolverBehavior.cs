using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Blitz.Client.Core.MVVM
{
    /// <summary>
    /// ContentControl Behavior that will resolve the correct view 
    /// for the ViewModel on the DataContext.
    /// </summary>
    public class ContentControlViewResolverBehavior : Behavior<ContentControl>
    {
        protected override void OnAttached()
        {
            RefreshContent();

            AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.DataContextChanged -= AssociatedObject_DataContextChanged;
        }

        private void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshContent();
        }

        private void RefreshContent()
        {
            AssociatedObject.Content = null;

            var viewModel = AssociatedObject.DataContext as IViewModel;
            if (viewModel == null)
            {
                return;
            }

            var view = ViewService.CreateView(viewModel.GetType());
            ViewService.BindViewModel(view, viewModel);

            AssociatedObject.Content = view;
        }
    }
}