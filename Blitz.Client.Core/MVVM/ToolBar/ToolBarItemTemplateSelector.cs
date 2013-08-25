using System.Windows;
using System.Windows.Controls;

namespace Blitz.Client.Core.MVVM.ToolBar
{
    public class ToolBarItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = null;

            var typeName = item.GetType().Name;
            switch (typeName)
            {
                case "ToolBarButtonItem":
                {
                    template = Application.Current.TryFindResource("ToolBarButtonItemTemplate") as DataTemplate;
                    break;
                }
            }

            return template;
        }
    }
}