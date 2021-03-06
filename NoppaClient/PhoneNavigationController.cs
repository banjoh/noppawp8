﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NoppaClient.ViewModels;
using NoppaLib.DataModel;
using Microsoft.Phone.Controls;
using NoppaLib;

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
            _frame.Navigate(PhoneNavigationController.MakeCoursePageUri(course));
        }

        public void ShowDepartment(DepartmentProxy department)
        {
            _frame.Navigate(NoppaUtility.MakeUri("/CourseListPage.xaml", "content", "department", "id", department.Id));
        }

        public void ShowCourseEvent(CourseEvent courseEvent)
        {
            // Pass the whole event as navigation parameters
            _frame.Navigate(NoppaUtility.MakeUri("/EventPage.xaml",
                "course_id",  courseEvent.CourseId,
                "type", courseEvent.Type,
                "title", courseEvent.Title,
                "weekday", courseEvent.Weekday,
                "location", courseEvent.Location,
                "start_time", courseEvent.StartTime,
                "end_time", courseEvent.EndTime,
                "start_date", courseEvent.StartDate,
                "end_date", courseEvent.EndDate));
        }

        public void ShowCourseNews(CourseNewsViewModel news)
        {
            _frame.Navigate(NoppaUtility.MakeUri("/CoursePage.xaml", 
                "id", news.Course.Id, 
                "news", news.Index));
        }

        public void ShowCourseSearch()
        {
            _frame.Navigate(NoppaUtility.MakeUri("/CourseSearchPage.xaml"));
        }

        public void ShowSettings()
        {
            _frame.Navigate(NoppaUtility.MakeUri("/SettingsPage.xaml"));
        }

        public void ShowHome()
        {
            _frame.Navigate(NoppaUtility.MakeUri("/MainPage.xaml"));
        }

        public void ShowAbout()
        {
            _frame.Navigate(NoppaUtility.MakeUri("/AboutPage.xaml"));
        }

        public static Uri MakeCoursePageUri(Course course)
        {
            return NoppaUtility.MakeUri("/CoursePage.xaml", "id", course.Id);
        }
    }
}
