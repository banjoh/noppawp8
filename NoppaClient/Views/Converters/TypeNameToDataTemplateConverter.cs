using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NoppaClient.View
{
    public class TypeNameToDataTemplateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is IDictionary)
            {
                var dict = parameter as IDictionary;
                var typename = value.GetType().Name; // Just class name
                if (!dict.Contains(typename))
                {
                    return null;
                }
                var template = dict[typename];
                return template is DataTemplate ? template : null;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
