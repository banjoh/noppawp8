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
using NoppaClient.Resources;
using NoppaClient.View;
using System.Collections.Specialized;
using System.ComponentModel;

namespace NoppaClient
{
    public partial class CourseSearchPage : PhoneApplicationPage
    {
        CourseListViewModel _viewModel;
        List<Action> _unbindActions = new List<Action>();

        public CourseSearchPage()
        {
            InitializeComponent();
            _viewModel = new CourseListViewModel(new PhoneNavigationController());
            DataContext = _viewModel;

            // Focus search box automatically
            this.Loaded += (o, e) => { if (_viewModel.IsEmpty) SearchBox.Focus(); };

            // Unfocus when pressing enter
            SearchBox.KeyUp += (o, e) => { if (e.Key == Key.Enter) this.Focus(); };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var sortByCodeButton = (ApplicationBarIconButton)this.ApplicationBar.Buttons[0];
            var sortByNameButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            //var sortByDepartmentButton = (ApplicationBarIconButton)ApplicationBar.Buttons[2];

            _unbindActions.Add(AppBar.BindRadioButton(sortByCodeButton, _viewModel, "Filter", CourseListViewModel.CourseFilter.Code));
            _unbindActions.Add(AppBar.BindRadioButton(sortByNameButton, _viewModel, "Filter", CourseListViewModel.CourseFilter.Name));
            //_unbindActions.Add(AppBar.BindRadioButton(sortByDepartmentButton, _viewModel, "Filter", CourseListViewModel.CourseFilter.Department));

            sortByCodeButton.Text = AppResources.SortByCodeTitle;
            sortByNameButton.Text = AppResources.SortByNameTitle;
            //sortByDepartmentButton.Text = AppResources.SortByDepartmentTitle;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            _viewModel.StopLoading();

            /* Unbind every app bar menu event manually. */
            foreach (var action in _unbindActions)
            {
                action();
            }
            _unbindActions.Clear();
        }
    }
}