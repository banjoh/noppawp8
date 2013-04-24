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
        public ObservableCollection<CourseResult> Results {
            get { return _results; }
            private set { SetProperty(ref _results, value); }
        }

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
                results.Sort((a, b) => b.Date.CompareTo(a.Date));
                Results = new ObservableCollection<CourseResult>(results);
            }
            IsEmpty = results == null || results.Count == 0;
        }
    }
}

