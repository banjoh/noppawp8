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
using NoppaClient.DataModel;

namespace NoppaClient
{
    public partial class CourseSearchPage : PhoneApplicationPage
    {
        CourseListViewModel _viewModel;

        public CourseSearchPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _viewModel = new CourseListViewModel(new PhoneNavigationController());
            DataContext = _viewModel;
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