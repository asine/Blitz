using System.Windows;
using System.Windows.Controls;

namespace Blitz.Client.Core.MVVM.ToolBar
{
    public class ToolBarItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            switch (item.GetType().Name)
            {
                case "ToolBarButtonItem":
                {
                    return Application.Current.TryFindResource("ToolBarButtonItemTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}