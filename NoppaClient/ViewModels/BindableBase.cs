using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This is usually called within a property setter or getter, and therefore propert name is filled
        // in by the compiler for you. If it is called from elsewhere, it has to be explicitly provided.
        protected bool SetProperty<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                return false;
            }
            currentValue = newValue;
            NotifyPropertyChanged(propertyName);

            return true;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
