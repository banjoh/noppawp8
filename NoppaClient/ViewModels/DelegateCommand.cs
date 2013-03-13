using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;
        private Func<bool> _canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void NotifyCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }

    class DelegateCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<T> _execute;
        private Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || (parameter is T && _canExecute((T)parameter));
        }

        public void Execute(object parameter)
        {
            if (parameter is T)
            {
                _execute((T)parameter);
            }
        }

        public void NotifyCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }
}
