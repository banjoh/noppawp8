using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NoppaClient.View
{
    public class HtmlTextBox
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached("Html", typeof(string), typeof(HtmlTextBox), new PropertyMetadata(HtmlPropertyChanged));

        public static void SetHtml(DependencyObject obj, string val)
        {
            obj.SetValue(HtmlProperty, val);
        }

        public static string GetHtml(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        private static void HtmlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is WebBrowser)
            {
                (obj as WebBrowser).NavigateToString((string)args.NewValue);
            }
        }
    }
}
