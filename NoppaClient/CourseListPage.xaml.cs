using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoppaClient.ViewModels;

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
                case "search":
                    string searchQuery = NavigationContext.QueryString["query"];
                    await _viewModel.LoadSearchResultsAsync(searchQuery);
                    break;

                case "department":
                    string id = NavigationContext.QueryString["id"];
                    await _viewModel.LoadDepartmentAsync(id);
                    break;

                default:
                    await _viewModel.LoadMyCoursesAsync();
                    break;
            }
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CoursePage.xaml", UriKind.Relative));
        }
    }
}