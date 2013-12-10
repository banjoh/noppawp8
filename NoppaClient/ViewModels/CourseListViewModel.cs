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
        #region Bindable properties

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

        private bool _isEmpty = true;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetProperty(ref _isEmpty, value); }
        }

        private string _subtitle = "";
        public string Subtitle
        {
            get { return _subtitle; }
            set { SetProperty(ref _subtitle, value); }
        }

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public ICommand ActivateCourseCommand { get; private set; }

        #endregion

        #region Course list filter

        CourseFilter.Type _courseFilter = CourseFilter.Type.Code;
        public CourseFilter.Type Filter
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
            Courses = await CourseFilter.FilterCourses(Courses, Filter);
            IsLoading = false;
        }

        #endregion

        public CourseListViewModel() { }
        public CourseListViewModel(INavigationController navigationController)
        {
            _courses.CollectionChanged += OnCoursesChanged;

            ActivateCourseCommand = ControllerUtil.MakeShowCourseCommand(navigationController);
        }

        private void OnCoursesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsEmpty = _courses.Count == 0;
        }

        public async void LoadCoursesBySearchAsync(string query)
        {
            Title = String.Format(AppResources.SearchResultsPageTitle, query);

            IsLoading = true;

            try
            {
                List<Course> courses = await NoppaAPI.GetCourses(query);

                if (courses != null)
                {
                    Courses = await CourseFilter.FilterCourses(courses, CourseFilter.Type.Name);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task cancelled.");
            }

            IsLoading = false;
        }

        public async void LoadCoursesByDepartmentAsync(string departmentId)
        {
            IsLoading = true;
            Courses.Clear();

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
                List<Course> courses = await NoppaAPI.GetCoursesByDepartment(departmentId);
                if (courses != null)
                {
                    Courses = await CourseFilter.FilterCourses(courses, CourseFilter.Type.Department);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task cancelled.");
            }

            IsLoading = false;
        }
    }
}
