using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class CourseViewModel : BindableBase
    {
        private ObservableCollection<CourseContentViewModel> _contents = new ObservableCollection<CourseContentViewModel>();
        public ObservableCollection<CourseContentViewModel> Contents { get { return _contents; } }

        private bool _isLoading;
        public bool IsLoading { get { return _isLoading; } private set { SetProperty(ref _isLoading, value); } }

        private string _code = "";
        public string Code { get { return _code; } set { SetProperty(ref _code, value); } }

        public CourseViewModel(string courseCode)
        {
            Code = courseCode;
            _contents.Add(new FrontPageViewModel(this)); // Always add this

            /* Add all that exist dynamically, depending on the course model. */
            LoadContentAsync();
        }

        private async void LoadContentAsync()
        {
            var tasks = new List<Task<CourseContentViewModel>>();

            /* Load Overview */
            tasks.Add(Task.Run(async delegate () {
                    OverviewViewModel model = new OverviewViewModel(Code);
                    await model.LoadDataAsync(); 
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
        }
    }
}
