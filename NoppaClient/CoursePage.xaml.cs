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

namespace NoppaClient
{
    public partial class CoursePage : PhoneApplicationPage
    {
        CourseViewModel _viewModel;
        string _courseCode;

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

            ApplicationBarIconButton btn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            if (App.PinnedCourses.Codes.Contains(_courseCode))
            {
                btn.IconUri = new Uri("/Assets/pin.remove.png", UriKind.Relative);
                btn.Text = "unpin";
            }
            else
            {
                btn.IconUri = new Uri("/Assets/pin.png", UriKind.Relative);
                btn.Text = "pin";
            }

            _viewModel = new CourseViewModel(_courseCode);

            await _viewModel.LoadContentAsync();
            DataContext = _viewModel;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            ApplicationBarIconButton btn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];

            if (btn.Text == "pin")
            {
                btn.Text = "unpin";
                btn.IconUri = new Uri("/Assets/pin.remove.png", UriKind.Relative);
                Task add = App.PinnedCourses.Add(_courseCode);
            }
            else if (btn.Text == "unpin")
            {
                btn.Text = "pin";
                btn.IconUri = new Uri("/Assets/pin.png", UriKind.Relative);
                Task remove = App.PinnedCourses.Remove(_courseCode);
            }
        }
    }
}