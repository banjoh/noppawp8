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
    public partial class CoursePage : PhoneApplicationPage
    {
        CourseViewModel _viewModel;

        public CoursePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DataContext = _viewModel = new CourseViewModel();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}