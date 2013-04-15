using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using NoppaLib.DataModel;
using NoppaClient.Resources;

namespace NoppaClient.ViewModels
{
    public class ResultsViewModel : CourseContentViewModel
    {
        private ObservableCollection<CourseResult> _results = new ObservableCollection<CourseResult>();
        public ObservableCollection<CourseResult> Results { get { return _results; } }

        public ResultsViewModel()
        {
            Title = AppResources.ResultsTitle;
            Index = 6;
        }

        public async Task LoadDataAsync(string id)
        {
            List<CourseResult> results = await NoppaAPI.GetCourseResults(id);
            if (results != null)
            {
                foreach (var r in results)
                {
                    _results.Add(r);
                }
            }
            IsEmpty = results == null || results.Count == 0;
        }
    }
}

