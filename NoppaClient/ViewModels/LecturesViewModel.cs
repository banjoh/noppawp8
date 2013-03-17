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
        private ObservableCollection<Lecture> _lectures = new ObservableCollection<Lecture>();
        public ObservableCollection<Lecture> Lectures { get { return _lectures; } }

        public LecturesViewModel()
        {
            Title = "Lectures";
            Index = 4;

            for (int i = 0; i < 10; i++)
            {

                string json = @"{'lecture_id': '5601', 
                                'date': '2012-01-18',
                                'start_time':'14:15',
                                'end_time':'16:00',
                                'location':'AS1',
                                'title': 'Introduction: contents,practicalities, assignments.',
                                'content':''}";
                _lectures.Add(new Lecture(json));
            }

 
            

        }
    }
}
