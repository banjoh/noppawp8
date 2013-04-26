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
    public class NewsViewModel : CourseContentViewModel
    {
        private ObservableCollection<CourseNews> _news = new ObservableCollection<CourseNews>();
        public ObservableCollection<CourseNews> News { get { return _news; } }

        private int _currentNewsIndex = -1;
        public int CurrentNewsIndex 
        { 
            get { return _currentNewsIndex >= 0 && _currentNewsIndex < News.Count ? _currentNewsIndex : -1; } 
            set { SetProperty(ref _currentNewsIndex, value); } 
        }

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

                // In case the current index was set and read before the items were loaded
                if (_currentNewsIndex >= 0 && _currentNewsIndex < _news.Count)
                {
                    NotifyPropertyChanged("CurrentNewsIndex");
                }
            }
        }
    }
}
