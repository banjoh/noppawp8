using NoppaClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NoppaClient.View
{
    public class DateTimeToWeekdayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                var date = (DateTime)value;
                switch (date.Date.DayOfWeek)
                {
                    case DayOfWeek.Monday: return AppResources.MondayTitle;
                    case DayOfWeek.Tuesday: return AppResources.TuesdayTitle;
                    case DayOfWeek.Wednesday: return AppResources.WednesdayTitle;
                    case DayOfWeek.Thursday: return AppResources.ThursdayTitle;
                    case DayOfWeek.Friday: return AppResources.FridayTitle;
                    case DayOfWeek.Saturday: return AppResources.SaturdayTitle;
                    case DayOfWeek.Sunday: return AppResources.SundayTitle;           
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
