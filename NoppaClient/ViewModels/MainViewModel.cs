using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NoppaClient.Resources;
using NoppaLib.DataModel;
using NoppaLib;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Specialized;
using System.Linq;

namespace NoppaClient.ViewModels
{
    public class NewsGroupItem : ObservableCollection<CourseNews>
    {
        private string _newsDate;
        public string NewsDate { get { return _newsDate; } }
        private DateTime _dtnewsDate;
        public DateTime dtNewsDate { get { return _dtnewsDate; } }

        public NewsGroupItem(string newsDate, DateTime dtnewsDate)
            : base()
        {
            _newsDate = newsDate;
            _dtnewsDate = dtnewsDate;
        }
    }

    public class NewsGroup : ObservableCollection<NewsGroupItem>
    {
        public NewsGroup() : base() { }

        public async void AddNewItems(List<CourseNews> courseNews)
        {
            foreach (var c in courseNews)
            {
                bool found = false;
                foreach (NewsGroupItem item in Items)
                {
                    if (item.NewsDate == c.Date.ToShortDateString())
                    {
                        found = true;
                        item.Add(c);
                        break;
                    }
                }
                if (!found)
                {
                    NewsGroupItem newItem = new NewsGroupItem(c.Date.ToShortDateString(), c.Date);
                    newItem.Add(c);
                    Add(newItem);
                }
            }
            var sortedList = this.OrderByDescending(x => x.dtNewsDate).ToList();
            this.Clear();
            foreach (var sortedItem in sortedList)
                this.Add(sortedItem);
        }
    }

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

        private NewsGroup _news;
        public NewsGroup News
        {
            get { return _news; }
            private set { SetProperty(ref _news, value); }
        }

        private ObservableCollection<DepartmentGroup> _departments;
        public ObservableCollection<DepartmentGroup> Departments 
        {
            get { return _departments; }
            private set { SetProperty(ref _departments, value); }
        }

        public ICommand DepartmentActivatedCommand { get; private set; }
        public ICommand ShowSettingsCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }
        public ICommand ShowSearchCommand { get; private set; }
        public ICommand ActivateCourseCommand { get; private set; }
        public ICommand EventActivatedCommand { get; private set; }
        public ICommand NewsActivatedCommand { get; private set; }

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

        private bool _isDepartmentListEmpty = true;
        public bool IsDepartmentListEmpty
        {
            get { return _isDepartmentListEmpty; }
            private set { SetProperty(ref _isDepartmentListEmpty, value); }
        }

        public MainViewModel(INavigationController navigationController)
        {
            DepartmentActivatedCommand = ControllerUtil.MakeShowDepartmentCommand(navigationController);
            ShowSettingsCommand = ControllerUtil.MakeShowSettingsCommand(navigationController);
            ShowAboutCommand = ControllerUtil.MakeShowAboutCommand(navigationController);
            ShowSearchCommand = ControllerUtil.MakeShowCourseSearchCommand(navigationController);
            ActivateCourseCommand = ControllerUtil.MakeShowCourseCommand(navigationController);
            EventActivatedCommand = ControllerUtil.MakeShowCourseEventCommand(navigationController);
         }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadDataAsync(PinnedCourses pinnedCourses)
        {
            try
            {
                LoadDepartmentGroupsAsync();
                UpdateMyCoursesAsync(pinnedCourses);

                var courses = await pinnedCourses.GetCodesAsync();
                courses.CollectionChanged += (o, e) => UpdateMyCoursesAsync(pinnedCourses);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadDataAsync: Caught exception: {0}", ex.Message);
            }

            this.IsDataLoaded = true;
        }

        private async void LoadDepartmentGroupsAsync()
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
                    IsDepartmentListEmpty = Departments.Count == 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadDepartmentGroupsAsync: Caught exception: {0}", ex.Message);
            }
        }

        private async void UpdateMyCoursesAsync(PinnedCourses pinnedCourses)
        {
            try
            {
                News = new NewsGroup();
                var courses = await pinnedCourses.GetCodesAsync();
                await MyCourses.LoadMyCoursesAsync(pinnedCourses);

                List<CourseEvent> events = new List<CourseEvent>();

                /* For each course task to get list of news/events */
                var newsTasks = new List<Task<List<CourseNews>>>();
                var eventTasks = new List<Task<List<CourseEvent>>>();

                foreach (var courseId in courses)
                {
                    newsTasks.Add(Task.Run(async () => await NoppaAPI.GetCourseNews(courseId)));
                    eventTasks.Add(Task.Run(async () => await NoppaAPI.GetCourseEvents(courseId)));
                }

                while (newsTasks.Count > 0 || eventTasks.Count > 0)
                {
                    var newsTask = await Task.WhenAny(newsTasks);
                    newsTasks.Remove(newsTask);
                    var newsItems = await newsTask;

                    if (newsItems != null)
                        /* Each add now also sorts the list and updates UI. If there are LOTS of
                         * news, this will hurt performance. However, at this point I favor immediate
                         * response so well see how this goes. */
                        News.AddNewItems(newsItems);

                    var eventTask = await Task.WhenAny(eventTasks);
                    eventTasks.Remove(eventTask);
                    var eventItems = await eventTask;

                    if (eventItems != null)
                        events.AddRange(eventItems);
                }

                Events = EventGroup.CreateEventGroups(events);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadNewsAndEventsAsync: Caught exception: {0}", ex.Message);
            }
        }
    }
}