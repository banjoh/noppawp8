using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    internal class ControllerUtil
    {
        public static DelegateCommand<Course> MakeShowCourseCommand(INavigationController controller)
        {
            return new DelegateCommand<Course>(controller.ShowCourse, course => course != null);
        }

        public static DelegateCommand<DepartmentProxy> MakeShowDepartmentCommand(INavigationController controller)
        {
            return new DelegateCommand<DepartmentProxy>(controller.ShowDepartment, dept => dept != null);
        }

        public static DelegateCommand<CourseEvent> MakeShowCourseEventCommand(INavigationController controller)
        {
            return new DelegateCommand<CourseEvent>(controller.ShowCourseEvent, ev => ev != null);
        }

        public static DelegateCommand MakeShowCourseSearchCommand(INavigationController controller)
        {
            return new DelegateCommand(controller.ShowCourseSearch);
        }

        public static DelegateCommand MakeShowSettingsCommand(INavigationController controller)
        {
            return new DelegateCommand(controller.ShowSettings);
        }

        public static DelegateCommand MakeShowHomeCommand(INavigationController controller)
        {
            return new DelegateCommand(controller.ShowHome);
        }

        public static DelegateCommand MakeShowAboutCommand(INavigationController controller)
        {
            return new DelegateCommand(controller.ShowAbout);
        }
    }
}
