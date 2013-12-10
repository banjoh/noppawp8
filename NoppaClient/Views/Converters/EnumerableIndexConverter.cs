using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NoppaClient.View
{
    public class EnumerableIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int index = -1;
            
            if (parameter is int) 
            {
                index = (int)parameter;
            } 
            else if (parameter is string)
            {
                index = int.Parse((string)parameter);
            }

            if (index < 0)
            {
                return null;
            }

            if (value is IEnumerable)
            {
                var enumerable = value as IEnumerable;
                return enumerable.Cast<object>().ElementAtOrDefault(index);
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
