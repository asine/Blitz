using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Blitz.Client.ModernUI.Assets.Icons
{
    public class IconNameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var resource = Application.Current.TryFindResource(value);

            var path = resource as Path;
            return path != null ? path.Data : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}