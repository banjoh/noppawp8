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


        public CourseViewModel()
        {
            /* Add all that exist dynamically, depending on the course model. */
            _contents.Add(new OverviewViewModel());
            _contents.Add(new LecturesViewModel());
            _contents.Add(new ExercisesViewModel());
        }
    }
}
