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

        public string Code { get; set; }

        public FrontPageViewModel()
        {
            // Needed for design time data initialization
        }

        public FrontPageViewModel(CourseViewModel courseViewModel)
        {
            // Pick interesting things from the courseViewModel to show

            _courseViewModel = courseViewModel;
            Code = _courseViewModel.Code;
            
            Title = "Front Page";
            Index = 0;
        }
    }
}
