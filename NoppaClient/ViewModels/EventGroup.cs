using NoppaLib.DataModel;
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
        private DateTime _eventDate;
        public DateTime EventDate { get { return _eventDate; } }

        public EventGroup(DateTime eventDate)
        {
            _eventDate = eventDate;
        }

        public static ObservableCollection<EventGroup> CreateEventGroups(IEnumerable<CourseEvent> items)
        {
            var groups = new Dictionary<DateTime, EventGroup>();

            foreach (var item in items)
            {
                DateTime start = item.StartDate;
                if (groups.ContainsKey(start))
                {
                    groups[start].Add(item);
                }
                else
                {
                    var group = new EventGroup(start);
                    group.Add(item);
                    groups.Add(start, group);
                }
            }

            return new ObservableCollection<EventGroup>(groups.Values.OrderBy(e => e.EventDate));
        }
    }
}
