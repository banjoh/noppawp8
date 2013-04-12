using NoppaClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    public interface INavigationController
    {
        void ShowHome();

        void ShowCourse(Course course);

        void ShowDepartment(Department department);

        void ShowCourseSearch();

        void ShowSettings();

        void ShowAbout();        
    }
}
