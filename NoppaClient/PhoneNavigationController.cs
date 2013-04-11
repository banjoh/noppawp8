using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NoppaClient.ViewModels;
using NoppaClient.DataModel;
using Microsoft.Phone.Controls;
using System.Net;

namespace NoppaClient
{
    public class PhoneNavigationController : INavigationController
    {
        private PhoneApplicationFrame _frame;

        public PhoneNavigationController() : this(App.RootFrame) { }
        public PhoneNavigationController(PhoneApplicationFrame frame)
        {
            _frame = frame;
        }

        public void ShowCourse(Course course)
        {
            _frame.Navigate(MakeCoursePageUri(course));
        }

        public void ShowDepartment(Department department)
        {
            _frame.Navigate(MakeUri("/CourseListPage.xaml", "content", "department", "id", department.Id));
        }

        public void ShowCourseSearch()
        {
            _frame.Navigate(MakeUri("/CourseSearchPage.xaml"));
        }

        public void ShowSettings()
        {
            _frame.Navigate(MakeUri("/SettingsPage.xaml"));
        }

        public void ShowHome()
        {
            _frame.Navigate(MakeUri("/MainPage.xaml"));
        }

        private static Uri MakeUri(string relativePath, params object[] parameters)
        {
            List<string> query = new List<string>();

            for (int i = 0; i + 1 < parameters.Length; i += 2)
            {
                query.Add(HttpUtility.UrlEncode(parameters[i].ToString()) + '=' + HttpUtility.UrlEncode(parameters[i + 1].ToString()));
            }

            return new Uri(relativePath + (query.Count == 0 ? "" : "?" + String.Join("&", query)), UriKind.Relative);
        }

        public static Uri MakeCoursePageUri(Course course)
        {
            return MakeUri("/CoursePage.xaml", "id", course.Id);
        }
    }
}
