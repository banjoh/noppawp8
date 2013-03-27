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

            for (int i = 0; i < 10; i++)
            {
                _lectures.Add(new CourseLecture {
                    LectureId = "5601",
                    Date = "2012-01-18",
                    StartTime = "14:15",
                    EndTime = "16:00",
                    Location = "AS1",
                    Title = "Introduction: contents,practicalities, assignments."
                });
            }

 
            

        }
    }
}
