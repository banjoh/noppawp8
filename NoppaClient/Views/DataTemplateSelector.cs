using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NoppaClient.View
{
    public class DataTemplateSelector : ContentControl
    {
        public ResourceDictionary TemplateDictionary { get; set; }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (TemplateDictionary != null)
            {
                var key = newContent.GetType().Name;
                //Debug.WriteLine("Looking for " + key);

                if (TemplateDictionary.Contains(key))
                {
                    var value = TemplateDictionary[key];
                    if (value is DataTemplate)
                    {
                        ContentTemplate = (DataTemplate)value;
                    }
                    else
                    {
                        Debug.WriteLine("Not a datatemplate.");
                    }
                }
                else
                {
                    Debug.WriteLine("Could not find " + key);
                }
            }
            else
            {
                Debug.WriteLine("No resource dictionary.");
            }
        }
    }
}
