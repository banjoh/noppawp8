using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NoppaClient.View
{
    public class StringToResourceConverter : IValueConverter
    {
        public ResourceDictionary ResourceDictionary { get; set; }
        public string DefaultKey { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (ResourceDictionary != null)
            {
                var key = value as string;
                if (key != null && ResourceDictionary.Contains(key))
                {
                    return ResourceDictionary[key];
                }
                else if (DefaultKey != null && ResourceDictionary.Contains(DefaultKey))
                {
                    return ResourceDictionary[DefaultKey];
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
