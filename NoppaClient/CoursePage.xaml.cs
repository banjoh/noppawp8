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
using System.Threading.Tasks;
using NoppaClient.View;

namespace NoppaClient
{
    public partial class CoursePage : PhoneApplicationPage
    {
        CourseViewModel _viewModel;
        List<Action> _unbindActions = new List<Action>();

        public CoursePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string courseCode = "";
            
            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                courseCode = NavigationContext.QueryString["id"];
            }

            // Find menu objects
            var toggleFavoriteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            var toggleTileMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];            
            
            var removeUri = new Uri("/Assets/pin.remove.png", UriKind.Relative);
            var addUri = new Uri("/Assets/pin.png", UriKind.Relative);

            if (_viewModel == null)
            {
                _viewModel = new CourseViewModel(courseCode, App.PinnedCourses);
                _viewModel.LoadContentAsync();
                DataContext = _viewModel;
            }

            // Pin toggle button
            _unbindActions.Add(AppBar.BindToggleButtonToBoolean(toggleFavoriteButton, removeUri, addUri, _viewModel, "IsPinned"));
            _unbindActions.Add(AppBar.BindText(toggleFavoriteButton, _viewModel, "IsPinnedText"));

            // Add/remove 
            _unbindActions.Add(AppBar.BindCommand(toggleTileMenuItem, _viewModel.ToggleSecondaryTileCommand));
            _unbindActions.Add(AppBar.BindText(toggleTileMenuItem, _viewModel, "ToggleSecondaryTileText"));

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