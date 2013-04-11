using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoppaClient.DataModel;
using NoppaClient.ViewModels;
using System.Threading.Tasks;
using NoppaClient.View;
using NoppaClient.Resources;

namespace NoppaClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        Language _loadedLanguage;
        MainViewModel _viewModel;
        List<Action> _unbindActions = new List<Action>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            _viewModel = App.ViewModel;
            DataContext = _viewModel;
            _loadedLanguage = App.Settings.Language;
        }

        // Load data for the ViewModel Items
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (!App.ViewModel.IsDataLoaded || _loadedLanguage != App.Settings.Language)
                {
                    await App.ViewModel.LoadDataAsync();
                    _loadedLanguage = App.Settings.Language;
                }
                //This loads MyCourses to MainPage, needs to be loaded every time in case changes in pinned courses
                await App.ViewModel.MyCourses.LoadMyCoursesAsync();

                var searchButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                var settingsMenu = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];

                searchButton.Text = AppResources.SearchTitle;
                settingsMenu.Text = AppResources.SettingsTitle;

                _unbindActions.Add(AppBar.BindCommand(searchButton, _viewModel.ShowSearchCommand));
                _unbindActions.Add(AppBar.BindCommand(settingsMenu, _viewModel.ShowSettingsCommand));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Couldn't load data: {0}", ex.Message);
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