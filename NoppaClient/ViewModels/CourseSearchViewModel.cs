using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    class CourseSearchViewModel : BindableBase
    {
        public CourseListViewModel CourseList { get; private set; }
        public DepartmentListViewModel DepartmentList { get; private set; }

        private bool _isSearchHintVisible = true;
        public bool IsSearchHintVisible
        {
            get { return _isSearchHintVisible; }
            set { SetProperty(ref _isSearchHintVisible, value); }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public ICommand SearchCommand { get; private set; }

        public CourseSearchViewModel() { }
        public CourseSearchViewModel(INavigationController controller)
        {
            CourseList = new CourseListViewModel(controller);
            DepartmentList = new DepartmentListViewModel(controller);

            SearchCommand = new DelegateCommand<string>(query =>
            {
                IsSearchHintVisible = false;
                CourseList.LoadCoursesBySearchAsync(query);
            });
        }
    }
}
