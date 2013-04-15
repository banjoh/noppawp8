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
        public string StartTime { get { return _event.StartTime; } }
        public string EndTime { get { return _event.EndTime; } }
        
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public bool IsSingleDay { get { return StartDate.Date == EndDate.Date; } }

        private string _courseName;
        public string CourseName { get { return _courseName; } set { SetProperty(ref _courseName, value); } }

        private ICommand _showCourseCommand;
        public ICommand ShowCourseCommand { get { return _showCourseCommand; } private set { SetProperty(ref _showCourseCommand, value); } }

        public EventViewModel() { /* for sample data */ }

        public EventViewModel(CourseEvent courseEvent, INavigationController controller)
        {
            _event = courseEvent;

            StartDate = DateTime.Parse(courseEvent.StartDate);
            EndDate = DateTime.Parse(courseEvent.EndDate);

            LoadCourseDataAsync(controller);
        }

        private async void LoadCourseDataAsync(INavigationController controller)
        {
            var course = await NoppaAPI.GetCourse(CourseId);
            CourseName = course.LongName;

            ShowCourseCommand = new DelegateCommand(() => controller.ShowCourse(course));
        }
    }
}
