using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using NoppaClient.DataModel;

namespace NoppaClient.ViewModels
{
    public class AssignmentsViewModel : CourseContentViewModel
    {
        private ObservableCollection<CourseAssignment> _assignments = new ObservableCollection<CourseAssignment>();
        public ObservableCollection<CourseAssignment> Assignments { get { return _assignments; } }

        public AssignmentsViewModel()
        {
            Title = "Assignments";
            Index = 7;
        }

        public async Task LoadDataAsync(string id)
        {
            List<CourseAssignment> assignments = await NoppaAPI.GetCourseAssignments(id);
            if (assignments != null)
            {
                foreach (var a in assignments)
                {
                    _assignments.Add(a);
                }
            }
            IsEmpty = assignments == null || assignments.Count == 0;
        }
    }
}

