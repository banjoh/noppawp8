﻿using Microsoft.Phone.Tasks;
using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    public class EventViewModel : BindableBase
    {
        private CourseEvent _event;

        public string CourseId { get { return _event.CourseId; } }
        public string Type { get { return _event.Type; } }
        public string Title { get { return _event.Title; } }
        public string Weekday { get { return _event.Weekday; } }
        public string Location { get { return _event.Location; } }
        public string StartTime { get { return _event.StartTime.ToShortTimeString(); } }
        public string EndTime { get { return _event.EndTime.ToShortTimeString(); } }
        
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public bool IsSingleDay { get { return StartDate.Date == EndDate.Date; } }

        private string _courseName;
        public string CourseName { get { return _courseName; } set { SetProperty(ref _courseName, value); } }

        private Course _course;
        public Course Course 
        { 
            get { return _course; } 
            set 
            {
                if (SetProperty(ref _course, value) && _showCourseCommand != null)
                {
                    _showCourseCommand.NotifyCanExecuteChanged();
                }
            } 
        }

        private DelegateCommand _showCourseCommand;
        public ICommand ShowCourseCommand { get { return _showCourseCommand; } }

        private DelegateCommand _addToCalendarCommand;
        public ICommand AddToCalendarCommand { get { return _addToCalendarCommand; } }

        public EventViewModel() { /* for sample data */ }

        public EventViewModel(CourseEvent courseEvent, INavigationController controller)
        {
            _event = courseEvent;

            StartDate = courseEvent.StartDate;
            EndDate = courseEvent.EndDate;

            _showCourseCommand = new DelegateCommand(() => controller.ShowCourse(Course), () => Course != null);
            _addToCalendarCommand = new DelegateCommand(AddToCalendar);

            LoadCourseDataAsync(controller);
        }

        private async void AddToCalendar()
        {
            /* Add to calendar */
            DateTime dtStartTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, _event.StartTime.Hour, _event.StartTime.Minute, _event.StartTime.Second);
            DateTime dtEndTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, _event.EndTime.Hour, _event.EndTime.Minute, _event.EndTime.Second);
            var appt = new SaveAppointmentTask
            {
                AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy,
                Subject = Title,
                Details = EventTypeConverter(Type) + " " + CourseName,
                Location = Location,
                StartTime = dtStartTime,
                EndTime = dtEndTime,
                Reminder = Reminder.OneHour
            };
            appt.Show();
        }

        private string EventTypeConverter(string eventType)
        {
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

        private async void LoadCourseDataAsync(INavigationController controller)
        {
            Course = await NoppaAPI.GetCourse(CourseId);
            CourseName = Course.LongName;
        }
    }
}
