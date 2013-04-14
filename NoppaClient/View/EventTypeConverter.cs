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
    public class EventTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string eventType = (string)value;
            switch (eventType)
            {
                case "event_course": return ("Course Event");
                case "exams": return ("Exam");
                case "mid_term_exams": return ("Mid term exam");
                case "other": return ("Other event");
                case "seminar": return ("Seminar");
                case "casework": return ("Casework");
                case "demonstration": return ("Demonstration");
                case "group_studies": return ("Group studies");
                case "individual_studies": return ("Individual studies");
                case "hybrid_studies": return ("Hybrid studies");
                case "online_studies": return ("Online studies");
                default: return ("");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
