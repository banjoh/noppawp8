using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class CourseContentViewModel : BindableBase
    {
        private string _title = "";
        public string Title { get { return _title; } protected set { SetProperty(ref _title, value); } }

        public override string ToString()
        {
            return _title;
        }
    }
}
