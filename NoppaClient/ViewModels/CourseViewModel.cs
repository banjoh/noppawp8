using NoppaLib.DataModel;
using NoppaClient.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    public class CourseViewModel : BindableBase
    {
        private Course _course = null;

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

        private bool _isPinningActive = false;
        public bool? IsPinned
        {
            get { return _isPinningActive ? null : (bool?)App.PinnedCourses.Codes.Contains(_code); }
            set 
            {
                if (_isPinningActive || !value.HasValue)
                {
                    return;
                }

                PinCourseAsync(value.Value);
            }
        }

        private void PinCourseAsync(bool toggle)
        {
            _isPinningActive = true;
            NotifyPropertyChanged("IsPinned");
            if (toggle)
            {
                App.PinnedCourses.Add(_code);
            }
            else
            {
                App.PinnedCourses.Remove(_code);
            }
            _isPinningActive = false;
            NotifyPropertyChanged("IsPinned");
            NotifyPropertyChanged("IsPinnedText");
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

        public CourseViewModel() { /* For design mode */ }

        public CourseViewModel(string courseCode)
        {
            Code = courseCode;
            _contents.Add(new FrontPageViewModel(this)); // Always add this

            _toggleSecondaryTileCommand = new DelegateCommand(ToggleSecondaryTile, () => _course != null);
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

            IsLoading = false;
        }

        private void ToggleSecondaryTile()
        {
            if (_course != null)
            {
                if (CourseTile.Exists(_course))
                {
                    CourseTile.Delete(_course);
                }
                else
                {
                    CourseTile.CreateOrUpdate(_course);
                }
                UpdateToggleCommandText();
            }
        }

        private void UpdateToggleCommandText()
        {
            ToggleSecondaryTileText = CourseTile.Exists(_course) ? AppResources.RemoveCourseTileLabel 
                                                                 : AppResources.AddCourseTileLabel;
        }
    }
}
