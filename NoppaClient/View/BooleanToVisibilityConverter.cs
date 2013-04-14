using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NoppaClient.View
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool invert = parameter is string && (string)parameter == "invert";
            return (value == null || (value is bool) && !(bool)value) != invert ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool invert = parameter is string && (string)parameter == "invert";
            return (value is Visibility && (Visibility)value == Visibility.Visible) != invert;
        }
    }
}
