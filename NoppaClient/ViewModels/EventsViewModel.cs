using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using NoppaLib;
using NoppaLib.DataModel;
using NoppaClient.Resources;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    public class EventsViewModel : CourseContentViewModel
    {
        private ObservableCollection<EventGroup> _events = new ObservableCollection<EventGroup>();
        public ObservableCollection<EventGroup> Events { get { return _events; } set { SetProperty(ref _events, value); } }

        public ICommand EventActivatedCommand { get; private set; }

        public EventsViewModel(INavigationController navigationController)
        {
            Title = AppResources.EventsTitle;

            EventActivatedCommand = ControllerUtil.MakeShowCourseEventCommand(navigationController);
            Index = 8;
        }

        public override async Task<CourseContentViewModel> LoadDataAsync(string id)
        {
            List<CourseEvent> events = await NoppaAPI.GetCourseEvents(id);
            if (events != null)
            {
                Events = EventGroup.CreateEventGroups(events);
            }
            IsEmpty = events == null || events.Count == 0;

            return this;
        }
    }
}

