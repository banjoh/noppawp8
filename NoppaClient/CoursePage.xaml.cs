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
            Task loadTask = null;
            base.OnNavigatedTo(e);
            string courseCode = "";
            
            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                courseCode = NavigationContext.QueryString["id"];
            }

            // Find menu objects
            var openWebOodiButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            var toggleFavoriteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            var openWebNoppaButton = (ApplicationBarIconButton)ApplicationBar.Buttons[2];
            var toggleTileMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];            
            
            var removeUri = new Uri("/Assets/star.full.png", UriKind.Relative);
            var addUri = new Uri("/Assets/star.empty.png", UriKind.Relative);

            if (_viewModel == null)
            {
                _viewModel = new CourseViewModel(courseCode, App.PinnedCourses);
                /* No need to await this here, the page shows up faster w/o doing it, and it loads asynchronously just fine.  */
                loadTask =_viewModel.LoadContentAsync(); 
                DataContext = _viewModel;
            }

            // Note: oodi button label isn't localized, but does it need to be?
            _unbindActions.Add(AppBar.BindCommand(openWebOodiButton, _viewModel.OpenOodiPage));

            // Pin toggle button
            _unbindActions.Add(AppBar.BindToggleButtonToBoolean(toggleFavoriteButton, removeUri, addUri, _viewModel, "IsPinned"));
            _unbindActions.Add(AppBar.BindText(toggleFavoriteButton, _viewModel, "IsPinnedText"));

            // Note: noppa button label isn't localized, but does it need to be?
            _unbindActions.Add(AppBar.BindCommand(openWebNoppaButton, _viewModel.OpenNoppaPage));

            // Add/remove secondary tile
            _unbindActions.Add(AppBar.BindCommand(toggleTileMenuItem, _viewModel.ToggleSecondaryTileCommand));
            _unbindActions.Add(AppBar.BindText(toggleTileMenuItem, _viewModel, "ToggleSecondaryTileText"));

            if (loadTask != null)
            {
                await loadTask;
            }
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