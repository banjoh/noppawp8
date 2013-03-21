using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace NoppaClient.View
{
    public class OpenBrowserCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter is string || parameter is Uri;
        }

        public async void Execute(object parameter)
        {
            Uri uri = null;

            if (parameter is string)
            {
                uri = new Uri((string)parameter);
            }
            else if (parameter is Uri)
            {
                uri = (Uri)parameter;
            }

            if (uri != null)
            {
                await Launcher.LaunchUriAsync(uri);
            }
        }
    }
}
