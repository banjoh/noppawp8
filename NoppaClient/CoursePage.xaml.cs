﻿using System;
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
        string _courseCode;
        List<Action> _unbindActions = new List<Action>();

        public CoursePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            
            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                _courseCode = NavigationContext.QueryString["id"];
            }

            /*if (App.PinnedCourses.Codes.Contains(_courseCode))
            {
                btn.IconUri = new Uri("/Assets/pin.remove.png", UriKind.Relative);
                btn.Text = "unpin";
            }
            else
            {
                btn.IconUri = new Uri("/Assets/pin.png", UriKind.Relative);
                btn.Text = "pin";
            }*/

            // Find menu objects
            var toggleFavoriteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            var toggleTileMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];            
            
            var removeUri = new Uri("/Assets/pin.remove.png", UriKind.Relative);
            var addUri = new Uri("/Assets/pin.png", UriKind.Relative);

            _viewModel = new CourseViewModel(_courseCode);

            // Pin toggle button
            _unbindActions.Add(AppBar.BindToggleButtonToBoolean(toggleFavoriteButton, removeUri, addUri, _viewModel, "IsPinned"));
            _unbindActions.Add(AppBar.BindText(toggleFavoriteButton, _viewModel, "IsPinnedText"));

            // Add/remove 
            _unbindActions.Add(AppBar.BindCommand(toggleTileMenuItem, _viewModel.ToggleSecondaryTileCommand));
            _unbindActions.Add(AppBar.BindText(toggleTileMenuItem, _viewModel, "ToggleSecondaryTileText"));

            await _viewModel.LoadContentAsync();
            DataContext = _viewModel;
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

        private void btn_Click(object sender, EventArgs e)
        {
            ApplicationBarIconButton btn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];

            if (btn.Text == "pin")
            {
                btn.Text = "unpin";
                btn.IconUri = new Uri("/Assets/pin.remove.png", UriKind.Relative);
                App.PinnedCourses.Add(_courseCode);
            }
            else if (btn.Text == "unpin")
            {
                btn.Text = "pin";
                btn.IconUri = new Uri("/Assets/pin.png", UriKind.Relative);
                App.PinnedCourses.Remove(_courseCode);
            }
        }
    }
}