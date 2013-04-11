using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using NoppaClient.DataModel;

namespace NoppaClient.ViewModels
{
    public class LecturesViewModel : CourseContentViewModel
    {
        private ObservableCollection<CourseLecture> _lectures = new ObservableCollection<CourseLecture>();
        public ObservableCollection<CourseLecture> Lectures { get { return _lectures; } }

        public LecturesViewModel()
        {
            Title = "Lectures";
            Index = 4;
        }

        public async Task LoadDataAsync(string id)
        {
            List<CourseLecture> lectures = await NoppaAPI.GetCourseLectures(id);
            if (lectures != null)
            {
                foreach (var lecture in lectures)
                {
                    _lectures.Add(lecture);
                }
            }
            IsEmpty = lectures == null || lectures.Count == 0;
        }
    }
}

