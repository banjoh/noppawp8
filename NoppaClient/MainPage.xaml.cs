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

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ReloadDataAsync();
            _loadedLanguage = App.Settings.Language;

            var searchButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            var settingsMenu = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
            var aboutMenu = (ApplicationBarMenuItem)ApplicationBar.MenuItems[1];

            searchButton.Text = AppResources.SearchTitle;
            settingsMenu.Text = AppResources.SettingsTitle;
            aboutMenu.Text = AppResources.AboutTitle;

            AppBar.BindCommand(searchButton, _viewModel.ShowSearchCommand);
            AppBar.BindCommand(settingsMenu, _viewModel.ShowSettingsCommand);
            AppBar.BindCommand(aboutMenu, _viewModel.ShowAboutCommand);
        }

        private async void ReloadDataAsync()
        {
            _viewModel = new MainViewModel(new PhoneNavigationController());
            DataContext = _viewModel;
            await _viewModel.LoadDataAsync(App.PinnedCourses);
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (_loadedLanguage != App.Settings.Language)
                {
                    ReloadDataAsync();
                    _loadedLanguage = App.Settings.Language;
                }              
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Couldn't load data: {0}", ex.Message);
            }
        }
    }
}