﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoppaClient.ViewModels;
using NoppaClient.DataModel;

namespace NoppaClient
{
    public partial class CourseListPage : PhoneApplicationPage
    {
        CourseListViewModel _viewModel;

        public CourseListPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _viewModel = new CourseListViewModel();
            DataContext = _viewModel;

            var content = "";
            if (NavigationContext.QueryString.ContainsKey("content")) {
                content = NavigationContext.QueryString["content"];
            }

            switch (content) 
            {
                case "department":
                    string id = NavigationContext.QueryString["id"];
                    await _viewModel.LoadDepartmentAsync(id);
                    break;

                default:
                    await _viewModel.LoadMyCoursesAsync();
                    break;
            }
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            _viewModel.StopLoading();
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as LongListSelector;
            if (list != null)
            {
                var selection = list.SelectedItem as Course;
                if (selection != null)
                {
                    NavigationService.Navigate(new Uri("/CoursePage.xaml?id=" + HttpUtility.UrlEncode(selection.Id), UriKind.Relative));
                }
            }
        }
    }
}