using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class EventViewModel : BindableBase
    {
        private string _dayOfTheWeek;
        public string DayOfTheWeek { get { return _dayOfTheWeek; } protected set { SetProperty(ref _dayOfTheWeek, value); } }

        private DateTime _date;
        public DateTime Date { get { return _date; } protected set { SetProperty(ref _date, value);} }

        private DateTime _startTime;
        public DateTime StartTime { get {return _startTime; } protected set { SetProperty(ref _startTime, value); } }

        private DateTime _endTime;
        public DateTime EndTime { get {return _endTime; } protected set { SetProperty(ref _endTime, value); } }

        private string _title;
        public string Title { get { return _title; } protected set { SetProperty(ref _title, value); } }

        private string _content;
        public string Content { get { return _content; } protected set { SetProperty(ref _content, value); } }

        private string _location;
        public string Location { get { return _location; } protected set { SetProperty(ref _location, value); } }

        /*

            lecture_id lecture id 50361 
date date of lecture 2012-01-18 
start_time start time 14:15:00 
end_time end time 16:00:00 
location lecture location AS1 
title title Introduction: contents, practicalities, assignments. 
content description of lecture Introduction, passing the course, course assignment 
materials object details in next table
  
authentication_required material needs authentication
true false
         */
    }
}
