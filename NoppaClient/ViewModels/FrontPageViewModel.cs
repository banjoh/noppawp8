using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    // Front page contains small snippets of various things
    public class FrontPageViewModel : CourseContentViewModel
    {
        private CourseViewModel _courseViewModel;

        public string Code { get { return _courseViewModel.Code; } }

        public FrontPageViewModel(CourseViewModel courseViewModel)
        {
            // Pick interesting things from the courseViewModel to show

            _courseViewModel = courseViewModel;
            
            Title = "Front Page " + courseViewModel.Code;
            Index = 0;
        }
    }
}
