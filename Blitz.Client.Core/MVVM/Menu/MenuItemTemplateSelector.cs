using System.Windows;
using System.Windows.Controls;

namespace Blitz.Client.Core.MVVM.Menu
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            switch (item.GetType().Name)
            {
                case "MenuGroupItem":
                {
                    return Application.Current.TryFindResource("MenuItemTemplate") as DataTemplate;
                }
                case "MenuButtonItem":
                {
                    return Application.Current.TryFindResource("MenuItemTemplate") as DataTemplate;
                }
                case "MenuSeperatorItem":
                {
                    return Application.Current.TryFindResource("MenuSeperatorItemTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}