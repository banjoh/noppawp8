using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class CourseFilter
    {
        public enum Type { Code, Name, Department };

        public static Task<ObservableCollection<Course>> FilterCourses(IEnumerable<Course> unfilteredCourses, Type filter)
        {
            return Task<ObservableCollection<Course>>.Run(() =>
            {
                IEnumerable<Course> filteredCourses = null;
                switch (filter)
                {
                    case Type.Code:
                        filteredCourses = unfilteredCourses.OrderBy(course => course.Id);
                        break;
                    case Type.Name:
                        filteredCourses = unfilteredCourses.OrderBy(course => course.Name);
                        break;
                    case Type.Department:
                        filteredCourses = unfilteredCourses.OrderBy(course => course.DepartmentId);
                        break;
                }
                return new ObservableCollection<Course>(filteredCourses);
            });
        }
    }
}
