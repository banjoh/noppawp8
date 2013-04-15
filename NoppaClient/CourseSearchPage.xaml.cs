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
using NoppaLib.DataModel;
using System.Windows.Input;

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

            // Focus search box automatically
            this.Loaded += (o, e) => SearchBox.Focus();

            // Unfocus when pressing enter
            SearchBox.KeyUp += (o, e) => { if (e.Key == Key.Enter) this.Focus(); };
            }
    }
}