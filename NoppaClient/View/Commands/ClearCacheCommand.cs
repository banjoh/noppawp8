using System;
using Windows.System;
using System.Windows.Input;
using NoppaLib;

namespace NoppaClient.View
{
    public class ClearCacheCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Cache.EmptyCache();
        }
    }
}
