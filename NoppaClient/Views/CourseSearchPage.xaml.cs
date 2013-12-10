using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoppaClient.Resources;
using NoppaClient.View;
using NoppaClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Navigation;

namespace NoppaClient
{
    public partial class CourseSearchPage : PhoneApplicationPage
    {
        CourseSearchViewModel _viewModel;
        List<Action> _unbindActions = new List<Action>();

        public CourseSearchPage()
        {
            InitializeComponent();

            _viewModel = new CourseSearchViewModel(new PhoneNavigationController());
            DataContext = _viewModel;

            // Unfocus when pressing enter
            SearchBox.KeyUp += (o, e) =>
            {
                if (e.Key == Key.Enter) this.Focus();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var sortByCodeButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            var sortByNameButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            //var sortByDepartmentButton = (ApplicationBarIconButton)ApplicationBar.Buttons[2];

            _unbindActions.Add(AppBar.BindRadioButton(sortByCodeButton, _viewModel.CourseList, "Filter", CourseFilter.Type.Code));
            _unbindActions.Add(AppBar.BindRadioButton(sortByNameButton, _viewModel.CourseList, "Filter", CourseFilter.Type.Name));
            //_unbindActions.Add(AppBar.BindRadioButton(sortByDepartmentButton, _viewModel, "Filter", CourseListViewModel.CourseFilter.Department));

            sortByCodeButton.Text = AppResources.SortByCodeTitle;
            sortByNameButton.Text = AppResources.SortByNameTitle;
            //sortByDepartmentButton.Text = AppResources.SortByDepartmentTitle;

            _viewModel.DepartmentList.LoadDepartmentGroupsAsync();
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