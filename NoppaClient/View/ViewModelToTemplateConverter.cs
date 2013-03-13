using NoppaClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NoppaClient.View
{
    public class ViewModelToTemplateConverter : IValueConverter
    {
        public DataTemplate OverviewTemplate { get; set; }
        public DataTemplate LecturesTemplate { get; set; }
        public DataTemplate ExercisesTemplate { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is OverviewViewModel) {
                return OverviewTemplate;
            } else if (value is LecturesViewModel) {
                return LecturesTemplate;
            } else if (value is ExercisesViewModel) {
                return ExercisesTemplate;
            } else {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
