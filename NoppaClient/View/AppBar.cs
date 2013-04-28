using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.View
{
    public static class AppBar
    {
        /* Bind menu item's Click event and IsEnabled property to the command. Return an action that unbinds. */
        public static Action BindCommand(IApplicationBarMenuItem item, ICommand command, object parameter = null)
        {
            EventHandler onClick = (o, e) =>
                {
                    if (command.CanExecute(parameter))
                    {
                        command.Execute(parameter);
                    }
                };

            EventHandler onCanExecuteChanged = (o, e) =>
                {
                    item.IsEnabled = command.CanExecute(parameter);
                };

            item.Click += onClick;
            command.CanExecuteChanged += onCanExecuteChanged;
            item.IsEnabled = command.CanExecute(parameter);

            return () =>
                {
                    item.Click -= onClick;
                    command.CanExecuteChanged -= onCanExecuteChanged;
                };
        }

        public static Action BindText(IApplicationBarMenuItem item, INotifyPropertyChanged obj, string property)
        {
            PropertyChangedEventHandler eh = (o, e) =>
                {
                    if (property == e.PropertyName)
                    {
                        item.Text = GetPropertyValue<string>(obj, property);
                    }
                };

            obj.PropertyChanged += eh;
            item.Text = GetPropertyValue<string>(obj, property);

            return () => { obj.PropertyChanged -= eh; };
        }

        public static Action BindToggleButtonToBoolean(ApplicationBarIconButton button, Uri trueUri, Uri falseUri, INotifyPropertyChanged obj, string property)
        {
            PropertyChangedEventHandler onPropertyChanged = (o, e) =>
                {
                    if (property == e.PropertyName)
                    {
                        UpdateToggleButtonState(button, trueUri, falseUri, obj, property);
                    }
                };

            EventHandler onClick = (o, e) =>
                {
                    var value = GetPropertyValue<bool?>(obj, property);
                    if (value.HasValue)
                    {
                        SetPropertyValue(obj, property, !value.Value);
                    }
                };

            obj.PropertyChanged += onPropertyChanged;
            button.Click += onClick;
            UpdateToggleButtonState(button, trueUri, falseUri, obj, property);

            return () => 
            {
                obj.PropertyChanged -= onPropertyChanged;
                button.Click -= onClick;
            };
        }

        public static Action BindRadioButton(ApplicationBarIconButton button, INotifyPropertyChanged obj, string property, object targetValue)
        {
            PropertyChangedEventHandler onPropertyChanged = (o, e) =>
            {
                if (property == e.PropertyName)
                {
                    button.IsEnabled = !GetPropertyValue<object>(obj, property).Equals(targetValue);
                }
            };

            EventHandler onClick = (o, e) =>
            {
                SetPropertyValue(obj, property, targetValue);
            };

            obj.PropertyChanged += onPropertyChanged;
            button.Click += onClick;
            var value = GetPropertyValue<object>(obj, property);
            button.IsEnabled = !value.Equals(targetValue);

            return () =>
            {
                obj.PropertyChanged -= onPropertyChanged;
                button.Click -= onClick;
            };
        }

        private static T GetPropertyValue<T>(object o, string propertyName)
        {
            try
            {
                var type = o.GetType();
                var property = type.GetProperty(propertyName);
                return (T)property.GetValue(o);
            }
            catch (Exception)
            {
                Debug.WriteLine("Bad binding: " + propertyName);
                return Activator.CreateInstance<T>();
            }
        }

        private static void SetPropertyValue(object o, string propertyName, object value)
        {
            try
            {
                var type = o.GetType();
                var property = type.GetProperty(propertyName);
                property.SetValue(o, value);
            }
            catch (Exception)
            {
                Debug.WriteLine("Bad binding: " + propertyName);
            }
        }

        private static void UpdateToggleButtonState(ApplicationBarIconButton button, Uri trueUri, Uri falseUri, INotifyPropertyChanged obj, string property)
        {
            var state = GetPropertyValue<bool?>(obj, property);
            if (state.HasValue)
            {
                button.IsEnabled = true;
                button.IconUri = state.Value ? trueUri : falseUri;
            }
            else
            {
                button.IsEnabled = false;
            }
        }
    }
}
