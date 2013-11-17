using NoppaLib.DataModel;
using NoppaLib;
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
using System.Collections.Specialized;

namespace NoppaClient.ViewModels
{
    // This view model should be used for any purpose where a list of courses is shown, for instance 
    // search results, or my courses on the main hub panorama
    public class CourseListViewModel : BindableBase
    {
        private ObservableCollection<Course> _courses = new ObservableCollection<Course>();
        public ObservableCollection<Course> Courses 
        { 
            get { return _courses; } 
            set 
            {
                var oldCourses = _courses;
                if (SetProperty(ref _courses, value))
                {
                    oldCourses.CollectionChanged -= OnCoursesChanged;
                    _courses.CollectionChanged += OnCoursesChanged;
                    IsEmpty = value.Count == 0;
                }
            } 
        }

        private bool _isSearchHintVisible = true;
        public bool IsSearchHintVisible { get { return _isSearchHintVisible; } set { SetProperty(ref _isSearchHintVisible, value); } }

        private bool _isEmpty = true;
        public bool IsEmpty { get { return _isEmpty; } set { SetProperty(ref _isEmpty, value); } }

        private string _subtitle = "";
        public string Subtitle { get { return _subtitle; } private set { SetProperty(ref _subtitle, value); } }

        private string _title = "";
        public string Title { get { return _title; } private set { SetProperty(ref _title, value); } }

        private bool _isLoading = false;
        public bool IsLoading { get { return _isLoading; } set { SetProperty(ref _isLoading, value); } }

        private ICommand _searchCommand;
        public ICommand SearchCommand { get { return _searchCommand; } }

        public ICommand ActivateCourseCommand { get; private set; }

        #region Course list filter

        public enum CourseFilter { Code, Name, Department };

        CourseFilter _courseFilter = CourseFilter.Code;
        public CourseFilter Filter
        {
            get { return _courseFilter; }
            set
            {
                if (!IsLoading && SetProperty(ref _courseFilter, value))
                {
                    UpdateFilter();                    
                }
            }
        }

        private async void UpdateFilter()
        {
            IsLoading = true;
            await FilterCourses(_courseFilter, _courses);
            IsLoading = false;
        }

        private async Task FilterCourses(CourseFilter filter, IEnumerable<Course> unfilteredCourses)
        {
            Courses = new ObservableCollection<Course>();

            Courses = await Task<ObservableCollection<Course>>.Run(() =>
                {
                    IEnumerable<Course> filteredCourses = null;
                    switch (filter)
                    {
                        case CourseFilter.Code:
                            filteredCourses = unfilteredCourses.OrderBy(course => course.Id);
                            break;
                        case CourseFilter.Name:
                            filteredCourses = unfilteredCourses.OrderBy(course => course.Name);
                            break;
                        case CourseFilter.Department:
                            filteredCourses = unfilteredCourses.OrderBy(course => course.DepartmentId);
                            break;
                    }
                    return new ObservableCollection<Course>(filteredCourses);
                });
        }

        #endregion

        private CancellationTokenSource _cts;
        Task _loaderTask = null;

        public CourseListViewModel() { }
        public CourseListViewModel(INavigationController navigationController)
        {
            _searchCommand = new DelegateCommand<string>(query => {
                StopLoading();
                _cts = new CancellationTokenSource();
                _loaderTask = LoadSearchResultsAsync(query, _cts.Token);
            });

            _courses.CollectionChanged += OnCoursesChanged;

            ActivateCourseCommand = ControllerUtil.MakeShowCourseCommand(navigationController);
        }

        private void OnCoursesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsEmpty = _courses.Count == 0;
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
            IsSearchHintVisible = false;
            _courses.Clear();

            try
            {
                List<Course> courses = await NoppaAPI.GetCourses(query);

                if (courses != null)
                {
                    await FilterCourses(_courseFilter, courses);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task cancelled.");
            }

            IsLoading = false;
        }

        public async void LoadDepartmentAsync(string departmentId)
        {
            IsLoading = true;
            _courses.Clear();

            Department dept = await NoppaAPI.GetDepartment(departmentId);

            /* Department search should mostly fail due to connectivity, as
             * the ids that lead here are given by NoppaAPI. Caching leads
             * to a situation that user may get into courselistview without
             * having connectivity. */
            if (dept == null)
            {
                IsLoading = false;
                IsEmpty = true;
                return;
            }

            string deptName = "";
            switch (App.Settings.Language)
            {
                case Language.Finnish: deptName = dept.name_fi; break;
                case Language.English: deptName = dept.name_en; break;
                case Language.Swedish: deptName = dept.name_sv; break;
                default: break;
            }

            Title = departmentId.ToUpper();
            Subtitle = dept != null ? deptName : Title;

            try
            {
                List<Course> courses = await NoppaAPI.GetCourses("", "", departmentId);
                if (courses != null)
                {
                    await FilterCourses(_courseFilter, courses);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task cancelled.");
            }

            IsLoading = false;
        }

        public async void LoadMyCoursesAsync()
        {
            IsLoading = true;

            var courses = await App.PinnedCourses.GetCoursesAsync();
            if (courses != null)
            {
                Courses = new ObservableCollection<Course>(courses);
            }
            else
            {
                Courses = new ObservableCollection<Course>();
            }

            IsLoading = false;
        }
    }
}
