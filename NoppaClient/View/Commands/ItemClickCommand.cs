using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoppaClient.View
{
    /* Item click command for LongListSelector elements */
    public class ItemClickCommand
    {
        public static readonly DependencyProperty CommandProperty =
           DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ItemClickCommand), new PropertyMetadata(null, CommandPropertyChanged));

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        private static void CommandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var element = obj as LongListSelector;

            if (element != null)
            {
                if (args.OldValue != null)
                {
                    element.SelectionChanged -= SelectionChanged;

                }

                if (args.NewValue != null)
                {
                    element.SelectionChanged += SelectionChanged;
                }
            }
        }

        static void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as LongListSelector;
            
            if (list != null)
            {
                var item = list.SelectedItem;

                if (item == null)
                {
                    return;
                }

                var command = GetCommand(list);
                if (command != null && command.CanExecute(item))
                {
                    command.Execute(item);
                }

                list.SelectedItem = null;             
            }
        }
    }
}
