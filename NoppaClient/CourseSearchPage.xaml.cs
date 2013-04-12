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
            _viewModel = new CourseListViewModel(new PhoneNavigationController());
            DataContext = _viewModel;

            this.Loaded += (o, e) => SearchBox.Focus();
        }
    }
}