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
    public class NewsItem
    {
        public string Title { get; set; }
        public string Course { get; set; }
    }

    public class NewsGroup : ObservableCollection<NewsItem>
    {
        private string _newsDate;
        public string NewsDate { get { return _newsDate; } }
        private DateTime _dtnewsDate;
        public DateTime dtNewsDate { get { return _dtnewsDate; } }

        public NewsGroup() { }

        public NewsGroup(string newsDate, DateTime dtnewsDate)
            : base()
        {
            _newsDate = newsDate;
            _dtnewsDate = dtnewsDate;
        }

        public static IEnumerable<NewsGroup> CreateNewsGroups(IEnumerable<CourseNews> courseNews, DateTime dateLimit)
        {
            var groups = new Dictionary<string, NewsGroup>();

            foreach (var news in courseNews)
            {
                if (news.Date.CompareTo(dateLimit) < 0)
                {
                    continue;
                }

                var date = news.Date.ToShortDateString();
                var item = new NewsItem { Title = news.Title, Course = news.CourseId };

                if (groups.ContainsKey(date))
                {
                    groups[date].Add(item);
                }
                else
                {
                    NewsGroup newItem = new NewsGroup(news.Date.ToShortDateString(), news.Date);
                    newItem.Add(item);
                    groups.Add(date, newItem);
                }
            }

            return groups.Values.OrderByDescending(news => news.dtNewsDate);
        }
    }

    public class MainViewModel : BindableBase
    {
        private ObservableCollection<EventGroup> _events = new ObservableCollection<EventGroup>();
        public ObservableCollection<EventGroup> Events
        {
            get { return _events; }
            private set { SetProperty(ref _events, value); }
        }

        private CourseListViewModel _myCourses;
        public CourseListViewModel MyCourses { get { return _myCourses; } }

        private ObservableCollection<NewsGroup> _news = new ObservableCollection<NewsGroup>();
        public ObservableCollection<NewsGroup> News
        {
            get { return _news; }
            private set { SetProperty(ref _news, value); }
        }

        private ObservableCollection<DepartmentGroup> _departments = new ObservableCollection<DepartmentGroup>();
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

        private int activeLoadingTasks = 0;

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { SetProperty(ref _isLoading, value); }
        }

        private bool _isDepartmentListEmpty = true;
        public bool IsDepartmentListEmpty
        {
            get { return _isDepartmentListEmpty; }
            private set { SetProperty(ref _isDepartmentListEmpty, value); }
        }

        public MainViewModel() { /* For compatibility with design time data. Don't use, don't remove. */ }

        public MainViewModel(INavigationController navigationController)
        {
            _myCourses = new CourseListViewModel(navigationController);

            DepartmentActivatedCommand  = ControllerUtil.MakeShowDepartmentCommand(navigationController);
            ShowSettingsCommand         = ControllerUtil.MakeShowSettingsCommand(navigationController);
            ShowAboutCommand            = ControllerUtil.MakeShowAboutCommand(navigationController);
            ShowSearchCommand           = ControllerUtil.MakeShowCourseSearchCommand(navigationController);
            ActivateCourseCommand       = ControllerUtil.MakeShowCourseCommand(navigationController);
            EventActivatedCommand       = ControllerUtil.MakeShowCourseEventCommand(navigationController);
            NewsActivatedCommand        = ControllerUtil.MakeShowCourseNewsCommand(navigationController);
         }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadDataAsync()
        {
            System.Diagnostics.Debug.WriteLine("Start loading");
            IsLoading = true;
            activeLoadingTasks = 3;

            try
            {
                LoadDepartmentGroupsAsync();

                MyCourses.LoadMyCoursesAsync();

                UpdateMyCourseNewsAsync();
                UpdateMyCourseEventsAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadDataAsync: Caught exception: {0}", ex.Message);
            }
        }

        private void OnLoadingFinished()
        {
            if (activeLoadingTasks == 0)
            {
                IsLoading = false;
                System.Diagnostics.Debug.WriteLine("Stop loading");
            }
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

            activeLoadingTasks--;

            OnLoadingFinished();
        }

        private async void UpdateMyCourseEventsAsync()
        {
            List<CourseEvent> events = new List<CourseEvent>();
            var courses = await App.PinnedCourses.GetCoursesAsync();

            if (courses != null)
            {   
                var eventTasks = new List<Task<List<CourseEvent>>>();

                foreach (var course in courses)
                {
                    eventTasks.Add(NoppaAPI.GetCourseEvents(course.Id));
                }

                while (eventTasks.Count > 0)
                {
                    var eventTask = await Task.WhenAny(eventTasks);

                    eventTasks.Remove(eventTask);
                    var eventItem = eventTask.Result;

                    if (eventItem != null)
                    {
                        var groups = EventGroup.CreateEventGroups(eventItem);
                        foreach (var group in groups)
                        {
                            Events.Add(group);
                        }
                    }
                }
            }

            activeLoadingTasks--;

            OnLoadingFinished();
        }

        private async void UpdateMyCourseNewsAsync()
        {
            /* For each course task to get list of news/events */
            var newsTasks = new List<Task<List<CourseNews>>>();
            var courses = await App.PinnedCourses.GetCoursesAsync();

            if (courses != null)
            {
                var courseNews = new List<CourseNews>();

                foreach (var course in courses)
                {
                    newsTasks.Add(NoppaAPI.GetCourseNews(course.Id));
                }

                while (newsTasks.Count > 0)
                {
                    var newsTask = await Task.WhenAny(newsTasks);

                    newsTasks.Remove(newsTask);
                    var newsItem = newsTask.Result;

                    if (newsItem != null)
                    {
                        var groups = NewsGroup.CreateNewsGroups(newsItem, DateTime.Today.AddMonths(-3));
                        foreach (var news in groups)
                        {
                            News.Add(news);
                        }
                    }
                }
            }

            activeLoadingTasks--;

            OnLoadingFinished();
        }
    }
}