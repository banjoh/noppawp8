using NoppaLib.DataModel;
using NoppaLib;
using NoppaClient.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace NoppaClient.ViewModels
{
    public class CourseViewModel : BindableBase
    {
        private Course _course = null;
        private PinnedCourses _pinnedCourses;

        OverviewViewModel _overviewModel;

        private ObservableCollection<CourseContentViewModel> _contents = new ObservableCollection<CourseContentViewModel>();
        public ObservableCollection<CourseContentViewModel> Contents { get { return _contents; } }

        private bool _isLoading;
        public bool IsLoading { get { return _isLoading; } private set { SetProperty(ref _isLoading, value); } }

        private string _code = "";
        public string Code { get { return _code; } set { SetProperty(ref _code, value); } }

        #region Pinned courses

        public string IsPinnedText
        {
            get { return IsPinned.HasValue && IsPinned.Value ? AppResources.UnpinCourseTitle : AppResources.PinCourseTitle; }
        }

        private bool? _isPinned = null;
        public bool? IsPinned
        {
            get { return _isPinned; }
            set 
            {
                if (_isPinned == null || !value.HasValue)
                {
                    return;
                }

                PinCourseAsync(value.Value);
            }
        }

        private async void PinCourseAsync(bool toggle)
        {
            _isPinned = null;
            NotifyPropertyChanged("IsPinned");
            if (toggle)
            {
                await App.PinnedCourses.AddAsync(_code);
            }
            else
            {
                await App.PinnedCourses.RemoveAsync(_code);
            }
            _isPinned = toggle;
            NotifyPropertyChanged("IsPinned");
            NotifyPropertyChanged("IsPinnedText");
        }

        private async void SetPinnedStateAsync()
        {
            _isPinned = await _pinnedCourses.ContainsAsync(_code);
            NotifyPropertyChanged("IsPinned");
        }

        #endregion

        #region Secondary tile properties

        private DelegateCommand _toggleSecondaryTileCommand;
        public ICommand ToggleSecondaryTileCommand { get { return _toggleSecondaryTileCommand; } }

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

        public CourseViewModel(string courseCode, PinnedCourses pinnedCourses)
        {
            Code = courseCode;
            _pinnedCourses = pinnedCourses;

            _toggleSecondaryTileCommand = new DelegateCommand(ToggleSecondaryTile, () => _course != null);

            _openNoppaPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(NoppaPageUri); }, () => NoppaPageUri != null);
            _openOodiPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(OodiPageUri); }, () => OodiPageUri != null);

            NoppaPageUri = new Uri(String.Format("https://noppa.aalto.fi/noppa/kurssi/{0}/etusivu", courseCode));

            SetPinnedStateAsync();
        }

        public async Task LoadContentAsync()
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            _course = await NoppaAPI.GetCourse(Code);
            _toggleSecondaryTileCommand.NotifyCanExecuteChanged();
            UpdateToggleCommandText();

            var tasks = new List<Task<CourseContentViewModel>>();

            /* Load Overview */
            tasks.Add(Task.Run(async delegate () {
                    OverviewViewModel model = new OverviewViewModel();
                    await model.LoadDataAsync(_course);
                    _overviewModel = model;
                    return model as CourseContentViewModel;
                })
            );

            /* Load Lectures */
            tasks.Add(Task.Run(async delegate(){
                    LecturesViewModel model = new LecturesViewModel();
                    await model.LoadDataAsync(Code);
                    return model as CourseContentViewModel;
                })
            );

            /* Load Exercises */
            tasks.Add(Task.Run(async delegate(){
                    ExercisesViewModel model = new ExercisesViewModel();
                    await model.LoadDataAsync(Code);
                    return model as CourseContentViewModel;
                })
            );

            /* Load News */
            tasks.Add(Task.Run(async delegate () {
                    NewsViewModel model = new NewsViewModel();
                    await model.LoadDataAsync(Code);
                    return model as CourseContentViewModel;
                })
            );

            /* Load Results */
            tasks.Add(Task.Run(async delegate(){
                    ResultsViewModel model = new ResultsViewModel();
                    await model.LoadDataAsync(Code);
                    return model as CourseContentViewModel;
                })
            );


            /* Load Assignments */
            tasks.Add(Task.Run(async delegate(){
                    AssignmentsViewModel model = new AssignmentsViewModel();
                    await model.LoadDataAsync(Code);
                    return model as CourseContentViewModel;
                })
            );


            /* Add items in the order they are finished. */
            while (tasks.Count > 0)
            {
                var task = await Task.WhenAny(tasks);
                tasks.Remove(task);

                var content = await task;
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
                    _contents.Insert(index, content);
                }
            }

            if (_overviewModel.OodiUrl != null)
            {
                OodiPageUri = new Uri(_overviewModel.OodiUrl);
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
