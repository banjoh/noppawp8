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
using NoppaClient.View;
using NoppaClient.Resources;

namespace NoppaClient
{
    public partial class EventPage : PhoneApplicationPage
    {
        EventViewModel _viewModel;
        Action _unbindAction = () => { };

        public EventPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_viewModel == null)
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
                    StartTime = DateTime.Parse(startTime),
                    EndTime = DateTime.Parse(endTime),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate)
                };

                _viewModel = new EventViewModel(courseEvent, new PhoneNavigationController());
                DataContext = _viewModel;
            }

            var addToCalendarButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            _unbindAction = AppBar.BindCommand(addToCalendarButton, _viewModel.AddToCalendarCommand);
            addToCalendarButton.Text = AppResources.AddToCalendarTitle;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            _unbindAction();
            _unbindAction = () => { };
        }

        private void TextBlock_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {

        }
    }
}
