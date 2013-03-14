using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NoppaClient.View
{
    public class SearchCommand
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(SearchCommand), new PropertyMetadata(null, CommandPropertyChanged));

        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.RegisterAttached("Parameter", typeof(Object), typeof(SearchCommand), null);

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetParameter(DependencyObject obj, Object value)
        {
            obj.SetValue(ParameterProperty, value);
        }

        public static Object GetParameter(DependencyObject obj)
        {
            return obj.GetValue(ParameterProperty);
        }

        private static void CommandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is FrameworkElement)
            {
                var textbox = (obj as FrameworkElement);

                if (args.OldValue != null)
                {
                    textbox.KeyUp -= KeyUp;
                }

                if (args.NewValue != null)
                {
                    textbox.KeyUp += KeyUp;
                }
            }
        }

        private static void KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var obj = sender as FrameworkElement;
                var command = GetCommand(obj);
                var parameter = GetParameter(obj);

                if (command != null && command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
        }
    }
}
