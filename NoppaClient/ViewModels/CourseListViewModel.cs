using NoppaClient.DataModel;
using NoppaClient.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    // This view model should be used for any purpose where a list of courses is shown, for instance 
    // search results, or my courses on the main hub panorama
    public class CourseListViewModel : BindableBase
    {
        private ObservableCollection<Course> _courses = new ObservableCollection<Course>();
        public ObservableCollection<Course> Courses { get { return _courses; } }

        private string _title = "";
        public string Title { get { return _title; } private set { SetProperty(ref _title, value); } }

        private bool _isLoading = false;
        public bool IsLoading { get { return _isLoading; } set { SetProperty(ref _isLoading, value); } }

        // This should only be called once per object
        public async Task LoadSearchResultsAsync(string query)
        {
            Title = String.Format(AppResources.SearchResultsPageTitle, query);

            IsLoading = true;

            var random = new Random();
            for (int i = 0, end = 10 + random.Next(10); i < end; i++)
            {
                // When await returns, we're back in the UI thread
                var course = await Task.Run(async () =>
                {
                    // Long running background code that is done in another thread
                    var index = i;
                    var randomSource = new Random();
                    await Task.Delay(randomSource.Next(1000));
                    //TODO: Use real data
                    string json = @"{'name': '" + String.Format("X-{0}.{1} My course name {2}", 100 + randomSource.Next(900), 1000 + randomSource.Next(9000), index) + "'}";
                    return new Course(json);
                });

                _courses.Add(course);
            }

            IsLoading = false;
        }

        public async Task LoadDepartmentAsync(string departmentId)
        {
            Title = String.Format(AppResources.DepartmentCourseListTitle, departmentId);

            IsLoading = true;

            var random = new Random();
            for (int i = 0, end = 20 + random.Next(20); i < end; i++)
            {
                // When await returns, we're back in the UI thread
                var course = await Task.Run(async () =>
                {
                    // Long running background code that is done in another thread
                    var index = i;
                    var randomSource = new Random();
                    await Task.Delay(randomSource.Next(1000));
                    //TODO: Use real data
                    string json = @"{'name': '" + String.Format("X-{0}.{1} My course name {2}", 100 + randomSource.Next(900), 1000 + randomSource.Next(9000), index) + "'}";
                    return new Course(json);
                });

                _courses.Add(course);
            }

            IsLoading = false;
        }

        public async Task LoadMyCoursesAsync()
        {
            Title = AppResources.MyCoursesTitle;

            IsLoading = true;

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
                    string json = @"{'name': '" + String.Format("X-{0}.{1} My course name {2}", 100 + randomSource.Next(900), 1000 + randomSource.Next(9000), index) + "'}";
                    return new Course(json);
                });

                _courses.Add(course);
            }

            IsLoading = false;
        }
    }
}
