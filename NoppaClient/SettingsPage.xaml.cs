using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.Windows.Data;
using NoppaLib;

namespace NoppaClient
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            var b = new Binding("IsNotEmpty");
            b.Source = Cache.Instance;
            clearButton.SetBinding(Button.IsEnabledProperty, b);
        }
    }
}
