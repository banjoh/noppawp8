using NoppaClient.Resources;
using NoppaLib;
using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace NoppaClient.ViewModels
{
    public class CourseViewModel : BindableBase
    {
        private string _courseId;
        private Course _course = null;

        public string Code { get { return _course.Id; } }

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<CourseContentViewModel> _contents = new ObservableCollection<CourseContentViewModel>();
        public ObservableCollection<CourseContentViewModel> Contents { get { return _contents; } }

        private CourseContentViewModel _currentContent;
        public CourseContentViewModel CurrentContent { get { return _currentContent; } set { SetProperty(ref _currentContent, value); } }

        private bool _isLoading;
        public bool IsLoading { get { return _isLoading; } private set { SetProperty(ref _isLoading, value); } }

        public OverviewViewModel OverviewModel { get; private set; }
        public NewsViewModel NewsModel { get; private set; }

        #region Pinned courses

        private string _isPinnedText;
        public string IsPinnedText
        {
            get { return _isPinnedText; }
            set { SetProperty(ref _isPinnedText, value); }
        }

        private bool _isPinned;
        public bool IsPinned
        {
            get { return _isPinned; }
            set
            {
                _isPinned = value;
                SetPinCourseState(value);
            }
        }

        private void SetPinCourseState(bool value)
        {
            if (value)
            {
                App.Settings.PinnedCourses.Add(_course);
            }
            else
            {
                App.Settings.PinnedCourses.Remove(_course);
            }
            IsPinnedText = value ? AppResources.UnpinCourseTitle : AppResources.PinCourseTitle;
        }

        public ICommand TogglePinCourseCommand { get; set; }

        #endregion

        #region Secondary tile properties

        public ICommand ToggleSecondaryTileCommand { get; set; }

        private string _toggleSecondaryTileText = AppResources.AddCourseTileLabel;
        public string ToggleSecondaryTileText
        {
            get { return _toggleSecondaryTileText; }
            set { SetProperty(ref _toggleSecondaryTileText, value); }
        }

        #endregion

        #region Open in web commands

        Uri _noppaPageUri;
        private Uri NoppaPageUri
        {
            get { return _noppaPageUri; }
            set
            {
                if (SetProperty(ref _noppaPageUri, value) && _openNoppaPage != null)
                {
                    _openNoppaPage.NotifyCanExecuteChanged();
                }
            }
        }

        DelegateCommand _openNoppaPage;
        public ICommand OpenNoppaPage { get { return _openNoppaPage; } }

        Uri _oodiPageUri = null;
        private Uri OodiPageUri 
        { 
            get { return _oodiPageUri; } 
            set 
            { 
                if (SetProperty(ref _oodiPageUri, value) && _openOodiPage != null)
                {
                    _openOodiPage.NotifyCanExecuteChanged();
                }
            }
        }

        DelegateCommand _openOodiPage;
        public ICommand OpenOodiPage { get { return _openOodiPage; } }

        #endregion

        public CourseViewModel() { /* For design mode */ }

        public CourseViewModel(string courseId)
        {
            _courseId = courseId;

            Title = AppResources.ApplicationTitle.ToUpper() + " " + courseId.ToUpper();

            // Jump through hoops because you cannot bind commands directly to ApplicationBar
            ToggleSecondaryTileCommand = new DelegateCommand(ToggleSecondaryTile, () => _course != null);
            _openNoppaPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(NoppaPageUri); }, () => NoppaPageUri != null);
            _openOodiPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(OodiPageUri); }, () => OodiPageUri != null);

            NoppaPageUri = new Uri(String.Format("https://noppa.aalto.fi/noppa/kurssi/{0}/etusivu", courseId));

            // By default, always add these two items (which can be assigned to CurrentContent by the view)
            OverviewModel = new OverviewViewModel();
            NewsModel = new NewsViewModel();

            Contents.Add(OverviewModel);
            Contents.Add(NewsModel);
        }

        public async Task LoadContentAsync(INavigationController navigationController)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            _course = await NoppaAPI.GetCourse(_courseId);
            (ToggleSecondaryTileCommand as DelegateCommand).NotifyCanExecuteChanged();

            bool coursePinned = App.Settings.PinnedCourses.Find(course => course.Id == _courseId) != null;
            IsPinned = coursePinned;
            IsPinnedText = coursePinned ? AppResources.UnpinCourseTitle : AppResources.PinCourseTitle;

            UpdateToggleCommandText();
            
            List<CourseContentViewModel> viewmodels = new List<CourseContentViewModel>()
            {
                new LecturesViewModel(),
                new ExercisesViewModel(),
                new ResultsViewModel(),
                new AssignmentsViewModel(),
                new EventsViewModel(navigationController)
            };

            List<Task<CourseContentViewModel>> tasks = new List<Task<CourseContentViewModel>>();


            await Task.WhenAll(
                /* Load Overview (already in the contents) */
                OverviewModel.LoadDataAsync(_courseId),
                /* Load News (already in the contents) */
                NewsModel.LoadDataAsync(_courseId)
            );


            foreach (var vm in viewmodels)
                tasks.Add(vm.LoadDataAsync(_courseId));

            /* Add items in the order they are finished. */
            while (tasks.Count > 0)
            {
                var task = await Task.WhenAny(tasks);
                tasks.Remove(task);
                var content = task.Result;

                if (!content.IsEmpty)
                {
                    int index = _contents.Count;

                    // Find the correct position
                    for (int i = 0; i < _contents.Count; i++)
                    {
                        if (_contents[i].Index > content.Index)
                        {
                            index = i;
                            break;
                        }
                    }
                    Contents.Insert(index, content);
                }
            }

            if (OverviewModel.OodiUrl != null)
            {
                OodiPageUri = new Uri(OverviewModel.OodiUrl);
            }

            IsLoading = false;
        }

        private void ToggleSecondaryTile()
        {
            if (_course != null)
            {
                if (NoppaTiles.Exists(_course))
                {
                    NoppaTiles.Delete(_course);
                }
                else
                {
                    NoppaTiles.CreateOrUpdate(_course);
                }
                UpdateToggleCommandText();
            }
        }

        private void UpdateToggleCommandText()
        {
            ToggleSecondaryTileText = NoppaTiles.Exists(_course) ? AppResources.RemoveCourseTileLabel 
                                                                 : AppResources.AddCourseTileLabel;
        }
    }
}
