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

        public ICommand ActivateCourseCommand { get; private set; }

        public CourseListViewModel(INavigationController navigationController)
        {
            _searchCommand = new DelegateCommand<string>(query => {
                StopLoading();
                _cts = new CancellationTokenSource();
                _loaderTask = LoadSearchResultsAsync(query, _cts.Token);
            });

            ActivateCourseCommand = ControllerUtil.MakeShowCourseCommand(navigationController);
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
                List<Course> courses = await NoppaAPI.GetCourses(query);

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
            IsLoading = true;
            _courses.Clear();

            Department dept = await NoppaAPI.GetDepartment(departmentId);
            Title = String.Format(AppResources.DepartmentCourseListTitle, dept != null ? dept.Name : departmentId);

            try
            {
                List<Course> courses = await NoppaAPI.GetCourses("", "", departmentId);
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
            Courses.Clear();
            foreach (string c in App.PinnedCourses.Codes){
                Course course = await NoppaAPI.GetCourse(c);
                Courses.Add(course);
            }
            IsLoading = false;
        }
    }
}
