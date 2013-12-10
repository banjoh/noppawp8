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
using System.ComponentModel;
using NoppaClient.View;
using NoppaClient.Resources;

namespace NoppaClient
{
    public partial class CourseListPage : PhoneApplicationPage
    {
        CourseListViewModel _viewModel;
        List<Action> _unbindActions = new List<Action>();

        public CourseListPage()
        {
            InitializeComponent();

            _viewModel = new CourseListViewModel(new PhoneNavigationController());
            DataContext = _viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("content"))
            {
                var content = NavigationContext.QueryString["content"];

                if (content == "department")
                {
                    string id = NavigationContext.QueryString["id"];
                    _viewModel.LoadCoursesByDepartmentAsync(id);
                }
            }

            var sortByCodeButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            var sortByNameButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];

            _unbindActions.Add(AppBar.BindRadioButton(sortByCodeButton, _viewModel, "Filter", CourseFilter.Type.Code));
            _unbindActions.Add(AppBar.BindRadioButton(sortByNameButton, _viewModel, "Filter", CourseFilter.Type.Name));

            sortByCodeButton.Text = AppResources.SortByCodeTitle;
            sortByNameButton.Text = AppResources.SortByNameTitle;
        }
        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            /* Unbind every app bar menu event manually. */
            foreach (var action in _unbindActions)
            {
                action();
            }

            _unbindActions.Clear();
        }
    }
}