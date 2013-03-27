using NoppaClient.DataModel;
using NoppaClient.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    // This view model should be used for any purpose where a list of courses is shown, for instance 
    // search results, or my courses on the main hub panorama
    public class CourseListViewModel : BindableBase
    {
        private ObservableCollection<Course> _courses = new ObservableCollection<Course>();
        public ObservableCollection<Course> Courses { get { return _courses; } }

        private CancellationTokenSource _cts;
        Task _loaderTask = null;

        private string _title = "";
        public string Title { get { return _title; } private set { SetProperty(ref _title, value); } }

        private bool _isLoading = false;
        public bool IsLoading { get { return _isLoading; } set { SetProperty(ref _isLoading, value); } }

        private ICommand _searchCommand;
        public ICommand SearchCommand { get { return _searchCommand; } }

        public CourseListViewModel()
        {
            _searchCommand = new DelegateCommand<string>(query => {
                StopLoading();
                _cts = new CancellationTokenSource();
                _loaderTask = LoadSearchResultsAsync(query, _cts.Token);
            });
        }

        public void StopLoading()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
            IsLoading = false;
        }

        private async Task LoadSearchResultsAsync(string query, CancellationToken cancellationToken)
        {
            Title = String.Format(AppResources.SearchResultsPageTitle, query);

            IsLoading = true;
            _courses.Clear();

            try
            {
                List<Course> courses = await NoppaApiClient.GetCourses(query);

                if (courses != null)
                {
                    foreach (var course in courses)
                    {
                        _courses.Add(course);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task cancelled.");
            }

            IsLoading = false;
        }

        public async Task LoadDepartmentAsync(string departmentId)
        {
            Title = String.Format(AppResources.DepartmentCourseListTitle, departmentId);

            IsLoading = true;
            _courses.Clear();

            try
            {
                List<Course> courses = await NoppaApiClient.GetCourses("", "", departmentId);
                if (courses != null)
                {
                    foreach (var course in courses)
                    {
                        _courses.Add(course);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task cancelled.");
            }

            IsLoading = false;
        }

        public async Task LoadMyCoursesAsync()
        {
            Title = AppResources.MyCoursesTitle;

            IsLoading = true;
            _courses.Clear();

            var random = new Random();
            for (int i = 0, end = 4 + random.Next(3); i < end; i++)
            {
                // When await returns, we're back in the UI thread
                var course = await Task.Run(async () =>
                {
                    // Long running background code that is done in another thread
                    var index = i;
                    var randomSource = new Random();
                    await Task.Delay(randomSource.Next(1000));
                    //TODO: Use real data
                    return new Course
                    {
                        Name = String.Format("X-{0}.{1} My course name {2}", 100 + randomSource.Next(900), 1000 + randomSource.Next(9000), index)
                    };
                });

                _courses.Add(course);
            }

            IsLoading = false;
        }
    }
}
