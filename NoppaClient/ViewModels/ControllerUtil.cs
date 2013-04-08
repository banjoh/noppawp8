﻿using NoppaClient.DataModel;
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

        public static DelegateCommand<Department> MakeShowDepartmentCommand(INavigationController controller)
        {
            return new DelegateCommand<Department>(controller.ShowDepartment, dept => dept != null);
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
    }
}