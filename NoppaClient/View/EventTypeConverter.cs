using NoppaClient.Resources;
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
            if (!(value is string))
                return AppResources.EventOtherTitle;

            string eventType = (string)value;
            switch (eventType)
            {
                case "event_course": return AppResources.EventCourseEventTitle;
                case "exams": return AppResources.EventExamTitle;
                case "mid_term_exams": return AppResources.EventMidTermExamTitle;
                case "other": return AppResources.EventOtherTitle;
                case "seminar": return AppResources.EventSeminarTitle;
                case "casework": return AppResources.EventCaseworkTitle;
                case "demonstration": return AppResources.EventDemonstrationTitle;
                case "group_studies": return AppResources.EventGroupStudies;
                case "individual_studies": return AppResources.EventIndividualStudies;
                case "hybrid_studies": return AppResources.EventHybridStudies;
                case "online_studies": return AppResources.EventOnlineStudies;
                default: return AppResources.EventOtherTitle;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
