using NoppaLib.DataModel;
using NoppaClient.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    class NewsViewModel : CourseContentViewModel
    {
        private ObservableCollection<CourseNews> _news = new ObservableCollection<CourseNews>();
        public ObservableCollection<CourseNews> News { get { return _news; } }

        public NewsViewModel()
        {
            Title = AppResources.NewsTitle;
            Index = 2;
        }

        public async Task LoadDataAsync(string id)
        {
            List<CourseNews> news = await NoppaAPI.GetCourseNews(id);
            if (news != null)
            {
                foreach (var n in news)
                {
                    _news.Add(n);
                }
            }
        }
    }
}
