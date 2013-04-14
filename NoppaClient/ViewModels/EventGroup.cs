using NoppaClient.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class EventGroup : ObservableCollection<CourseEvent>
    {
        private string _eventDate;
        public string EventDate { get { return _eventDate; } }

        public EventGroup(string eventDate)
        {
            _eventDate = eventDate;
        }

        public static ObservableCollection<EventGroup> CreateEventGroups(IEnumerable<CourseEvent> items)
        {
            var groups = new Dictionary<string, EventGroup>();

            foreach (var item in items)
            {
                if (groups.ContainsKey(item.StartDate))
                {
                    groups[item.StartDate].Add(item);
                }
                else
                {
                    var group = new EventGroup(item.StartDate);
                    group.Add(item);
                    groups.Add(item.StartDate, group);
                }
            }

            return new ObservableCollection<EventGroup>(groups.Values.OrderBy(e => e.EventDate));
        }
    }
}
