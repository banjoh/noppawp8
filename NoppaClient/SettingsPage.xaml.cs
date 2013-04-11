using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace NoppaClient
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            SetToggleValue(App.Settings.PrimaryTileIsActive);
        }

        private void updateTile_Toggle(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                App.Settings.PrimaryTileIsActive = (bool)toggleSwitch.IsChecked.Value;
                
                // Update value once the background agent has been started.
                // It may fail so the ToggleSwitch may need to be updated
                SetToggleValue(App.Settings.PrimaryTileIsActive);
            }
        }

        private void SetToggleValue(bool value)
        {
            EventHandler<RoutedEventArgs> h = new EventHandler<RoutedEventArgs>(updateTile_Toggle);
            // Temporarily unhook the event handler
            updateTile.Checked -= h;
            updateTile.Unchecked -= h;

            updateTile.IsChecked = value;
            
            // Reconnect the event handler
            updateTile.Checked += h; // DOES NOT SEEM TO SUPPRESS THIS EVENT
            updateTile.Unchecked += h;  
        }
    }
}
