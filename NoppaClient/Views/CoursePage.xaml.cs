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
            int? newsItem = null;
            
            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                courseCode = NavigationContext.QueryString["id"];
            }

            if (NavigationContext.QueryString.ContainsKey("news"))
            {
                try
                {
                    newsItem = Convert.ToInt32(NavigationContext.QueryString["news"]);
                }
                catch (Exception)
                {
                    /* news parameter wasn't a valid integer */
                }
            }

            _viewModel = new CourseViewModel(courseCode);
            DataContext = _viewModel;

            // Find menu objects
            var openWebOodiButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            var toggleFavoriteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            var openWebNoppaButton = (ApplicationBarIconButton)ApplicationBar.Buttons[2];
            var toggleTileMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];            
            
            var pinnedButtonUri = new Uri("/Assets/star.full.png", UriKind.Relative);
            var unpinnedButtonUri = new Uri("/Assets/star.empty.png", UriKind.Relative);

            // Make pivot view start on the news page with the item selected
            if (newsItem.HasValue)
            {
                _viewModel.CurrentContent = _viewModel.NewsModel;
                _viewModel.NewsModel.CurrentNewsIndex = newsItem.Value;
            }

            await _viewModel.LoadContentAsync(new PhoneNavigationController());

            // Note: oodi button label isn't localized, but does it need to be?
            _unbindActions.Add(AppBar.BindCommand(openWebOodiButton, _viewModel.OpenOodiPage));

            // Pin toggle button
            _unbindActions.Add(AppBar.BindToggleButtonToBoolean(toggleFavoriteButton, pinnedButtonUri, unpinnedButtonUri, _viewModel, "IsPinned"));
            _unbindActions.Add(AppBar.BindText(toggleFavoriteButton, _viewModel, "IsPinnedText"));

            // Note: noppa button label isn't localized, but does it need to be?
            _unbindActions.Add(AppBar.BindCommand(openWebNoppaButton, _viewModel.OpenNoppaPage));

            // Add/remove secondary tile
            _unbindActions.Add(AppBar.BindCommand(toggleTileMenuItem, _viewModel.ToggleSecondaryTileCommand));
            _unbindActions.Add(AppBar.BindText(toggleTileMenuItem, _viewModel, "ToggleSecondaryTileText"));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            /* Unbind every app bar menu event manually. */
            _unbindActions.ForEach( action => action() );
            _unbindActions.Clear();
        }
    }
}