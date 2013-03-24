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

namespace NoppaClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadDataAsync();
            }
        }

        private void DepartmentListSelected(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as LongListSelector;
            if (list != null)
            {
                var selection = list.SelectedItem as DepartmentViewModel;
                if (selection != null)
                {
                    NavigationService.Navigate(new Uri("/CourseListPage.xaml?content=department&id=" + HttpUtility.UrlEncode(selection.Id), UriKind.Relative));
                }
            }
        }

        private void ApplicationBarIconButton_SearchClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CourseSearchPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Settings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }
    }
}