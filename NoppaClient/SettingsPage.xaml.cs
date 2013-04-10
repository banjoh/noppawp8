using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NoppaClient
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            
            // Logic to check whether the noppa background agent is running or not
        }

        private void updateTile_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}