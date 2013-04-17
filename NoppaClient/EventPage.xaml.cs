using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoppaLib.DataModel;
using NoppaClient.ViewModels;

namespace NoppaClient
{
    public partial class EventPage : PhoneApplicationPage
    {
        public EventPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // There's no identifiers for individual events, so the
            // fields of the event is passed as parameters, and
            // a CourseEvent is reassembled here.

            string courseId, type, title, weekday, location;
            string startTime, endTime, startDate, endDate;

            var query = NavigationContext.QueryString;

            query.TryGetValue("course_id", out courseId);
            query.TryGetValue("type", out type);
            query.TryGetValue("title", out title);
            query.TryGetValue("weekday", out weekday);
            query.TryGetValue("location", out location);
            query.TryGetValue("start_time", out startTime);
            query.TryGetValue("end_time", out endTime);
            query.TryGetValue("start_date", out startDate);
            query.TryGetValue("end_date", out endDate);

            var courseEvent = new CourseEvent
            {
                  CourseId = Detail.StripHtml(courseId),
                  Type = Detail.StripHtml(type),
                  Title = Detail.StripHtml(title),
                  Weekday = Detail.StripHtml(weekday),
                  Location = Detail.StripHtml(location),
                  StartTime = Detail.StripHtml(startTime),
                  EndTime = Detail.StripHtml(endTime),
                  StartDate = Detail.StripHtml(startDate),
                  EndDate = Detail.StripHtml(endDate)
            };

            DataContext = new EventViewModel(courseEvent, new PhoneNavigationController());
        }

        private void TextBlock_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {

        }
    }
}
