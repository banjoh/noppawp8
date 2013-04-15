using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NoppaClient.Resources;
using NoppaLib.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private ObservableCollection<EventGroup> _events;
        public ObservableCollection<EventGroup> Events
        {
            get { return _events; }
            private set { SetProperty(ref _events, value); }
        }

        private CourseListViewModel _myCourses = new CourseListViewModel(new PhoneNavigationController());
        public CourseListViewModel MyCourses { get { return _myCourses; } }

        private ObservableCollection<CourseNews> _news = new ObservableCollection<CourseNews>();
        public ObservableCollection<CourseNews> News {
            get { return _news; }
            private set { SetProperty(ref _news, value); }
        }

        private ObservableCollection<DepartmentGroup> _departments;
        public ObservableCollection<DepartmentGroup> Departments {
            get { return _departments; }
            private set { SetProperty(ref _departments, value); }
        }

        public ICommand DepartmentActivatedCommand { get; private set; }
        public ICommand ShowSettingsCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }
        public ICommand ShowSearchCommand { get; private set; }
        public ICommand ActivateCourseCommand { get; private set; }
        public ICommand EventActivatedCommand { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get { return _sampleProperty; }
            set { SetProperty(ref _sampleProperty, value); }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public string Title
        {
            get { return AppResources.ApplicationTitle; }
        }

        private bool _isDataLoaded = false;
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            private set { SetProperty(ref _isDataLoaded, value); }
        }

        public MainViewModel(INavigationController navigationController)
        {
            DepartmentActivatedCommand = ControllerUtil.MakeShowDepartmentCommand(navigationController);
            ShowSettingsCommand = ControllerUtil.MakeShowSettingsCommand(navigationController);
            ShowAboutCommand = ControllerUtil.MakeShowAboutCommand(navigationController);
            ShowSearchCommand = ControllerUtil.MakeShowCourseSearchCommand(navigationController);
            ActivateCourseCommand = ControllerUtil.MakeShowCourseCommand(navigationController);
            EventActivatedCommand = ControllerUtil.MakeShowCourseEventCommand(navigationController);
            // Here, make a model instance or something, and start filling in the 
            // view model data
         }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadDataAsync()
        {
            try
            {
                List<Organization> orgs = await NoppaAPI.GetAllOrganizations();
                List<Department> depts = await NoppaAPI.GetDepartments();

                if (depts != null && orgs != null)
                {
                    var orgMap = new Dictionary<string, Organization>();
                    foreach (var org in orgs)
                    {
                        orgMap.Add(org.Id, org);
                    }

                    Departments = DepartmentGroup.CreateDepartmentGroups(orgMap, depts);
                }

                List<CourseNews> news = new List<CourseNews>();
                List<CourseEvent> events = new List<CourseEvent>();

                foreach (var courseId in App.PinnedCourses.Codes)
                {
                    List<CourseNews> courseNews = await NoppaAPI.GetCourseNews(courseId);
                    if (courseNews != null)
                        news.AddRange(courseNews);

                    List<CourseEvent> courseEvents = await NoppaAPI.GetCourseEvents(courseId);
                    if (courseEvents != null)
                        events.AddRange(courseEvents);
                }

                /* Figure out a better sorting strategy */
                news.Sort( (a,b) => string.Compare(a.Date, b.Date) );
 
                News = new ObservableCollection<CourseNews>(news);
                if (events != null)
                        Events = EventGroup.CreateEventGroups(events);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Caught exception: {0}", ex.Message);
            }

            this.IsDataLoaded = true;
        }
    }
}