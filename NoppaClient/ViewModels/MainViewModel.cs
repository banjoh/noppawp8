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
    public class CourseNewsViewModel
    {
        public CourseNews News { get; set; }
        public Course Course { get; set; }
        public int Index { get; set; }
        public DateTime Date { get { return News.Date; } }
        public string Title { get { return News.Title; } }
        public string Content { get { return News.Content; } }
        public List<Link> Links { get { return News.Links; } }
    }

    public class NewsGroup : ObservableCollection<CourseNewsViewModel>
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
    }

    public class NewsGroupCollection : ObservableCollection<NewsGroup>
    {
        public NewsGroupCollection() : this(DateTime.Today.AddYears(-1)) { }

        public NewsGroupCollection(DateTime dateLimit) 
        {
            _dateLimit = dateLimit;
        }

        DateTime _dateLimit;

        public void AddNewItems(List<CourseNewsViewModel> courseNews)
        {
            foreach (var news in courseNews)
            {
                if (news.Date.CompareTo(_dateLimit) < 0)
                {
                    continue;
                }

                bool found = false;
                foreach (NewsGroup item in Items)
                {
                    if (item.NewsDate == news.Date.ToShortDateString())
                    {
                        found = true;
                        item.Add(news);
                        break;
                    }
                }
                if (!found)
                {
                    NewsGroup newItem = new NewsGroup(news.Date.ToShortDateString(), news.Date);
                    newItem.Add(news);
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

        private CourseListViewModel _myCourses;
        public CourseListViewModel MyCourses { get { return _myCourses; } }

        private NewsGroupCollection _news;
        public NewsGroupCollection News
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

        private bool _isLoading = true;
        private bool _isDepartmanentsLoading = true;
        private bool _isMyCoursesLoading = true;
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

            DepartmentActivatedCommand = ControllerUtil.MakeShowDepartmentCommand(navigationController);
            ShowSettingsCommand = ControllerUtil.MakeShowSettingsCommand(navigationController);
            ShowAboutCommand = ControllerUtil.MakeShowAboutCommand(navigationController);
            ShowSearchCommand = ControllerUtil.MakeShowCourseSearchCommand(navigationController);
            ActivateCourseCommand = ControllerUtil.MakeShowCourseCommand(navigationController);
            EventActivatedCommand = ControllerUtil.MakeShowCourseEventCommand(navigationController);
            NewsActivatedCommand = ControllerUtil.MakeShowCourseNewsCommand(navigationController);
         }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadDataAsync(PinnedCourses pinnedCourses)
        {
            System.Diagnostics.Debug.WriteLine("Start loading");
            IsLoading = true;
            
            try
            {
                UpdateMyCoursesAsync(pinnedCourses);
                LoadDepartmentGroupsAsync();
                
                var courses = await pinnedCourses.GetCodesAsync();
                courses.CollectionChanged += (o, e) => UpdateMyCoursesAsync(pinnedCourses);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadDataAsync: Caught exception: {0}", ex.Message);
            }
        }

        private async void LoadDepartmentGroupsAsync()
        {
            _isDepartmanentsLoading = true;

            System.Diagnostics.Debug.WriteLine("Start loading departments");
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
            System.Diagnostics.Debug.WriteLine("Stop loading departments");

            _isDepartmanentsLoading = false;
            if (!_isMyCoursesLoading)
                IsLoading = false;
        }

        private async void UpdateMyCoursesAsync(PinnedCourses pinnedCourses)
        {
            _isMyCoursesLoading = true;

            try
            {
                News = new NewsGroupCollection(DateTime.Today.AddMonths(-3));
                var courses = await pinnedCourses.GetCodesAsync();
                await MyCourses.LoadMyCoursesAsync(pinnedCourses);

                List<CourseEvent> events = new List<CourseEvent>();

                /* For each course task to get list of news/events */
                var newsTasks = new List<Task<List<CourseNewsViewModel>>>();
                var eventTasks = new List<Task<List<CourseEvent>>>();

                foreach (var courseId in courses)
                {
                    newsTasks.Add(Task.Run(async () =>
                        {
                            var courseTask = NoppaAPI.GetCourse(courseId);
                            var newsTask = NoppaAPI.GetCourseNews(courseId);
                            await Task.WhenAll(newsTask, courseTask);

                            var newsList = new List<CourseNewsViewModel>();
                            var result = newsTask.Result;
                            var course = courseTask.Result;
                            for (int i = 0; i < result.Count; i++)
                            {
                                newsList.Add(new CourseNewsViewModel { News = result[i], Course = course, Index = i });
                            }
                            return newsList;
                        }));

                    eventTasks.Add(NoppaAPI.GetCourseEvents(courseId));
                }

                while (newsTasks.Count > 0 || eventTasks.Count > 0)
                {
                    if (newsTasks.Count > 0)
                    {
                        var newsTask = await Task.WhenAny(newsTasks);
                        newsTasks.Remove(newsTask);
                        var newsItems = newsTask.Result;

                        if (newsItems != null)
                            /* Each add now also sorts the list and updates UI. If there are LOTS of
                             * news, this will hurt performance. However, at this point I favor immediate
                             * response so well see how this goes. */
                            News.AddNewItems(newsItems);
                    }

                    if (eventTasks.Count > 0)
                    {
                        var eventTask = await Task.WhenAny(eventTasks);
                        eventTasks.Remove(eventTask);
                        var eventItems = await eventTask;

                        if (eventItems != null)
                            events.AddRange(eventItems);
                    }
                }

                Events = EventGroup.CreateEventGroups(events);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadNewsAndEventsAsync: Caught exception: {0}", ex.Message);
            }

            _isMyCoursesLoading = false;
            if (!_isDepartmanentsLoading)
                IsLoading = false;
        }
    }
}