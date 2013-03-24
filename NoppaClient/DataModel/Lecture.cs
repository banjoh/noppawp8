using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NoppaClient.DataModel
{
    public class Lecture
    {
        private string _lectureId;
        private string _date;
        private string _startTime;
        private string _endTime;
        private string _location;
        private string _lectureTitle;
        private string _content;

        public string LectureId { get { return _lectureId; } }
        public string Date { get { return _date; } }
        public string StartTime { get { return _startTime; }  }
        public string EndTime { get { return _endTime; } }
        public string Location { get { return _location; }  }
        public string LectureTitle { get { return _lectureTitle;}  }
        public string Content { get { return _content; }  }

        public Lecture() { }

        public Lecture(string json) : this(JObject.Parse(json)) {}
        public Lecture(JObject obj)
        {
            try
            {
                JToken token;

                this._lectureId = obj.TryGetValue("lecture_id", out token) ? token.ToString() : "N/A";
                this._date = obj.TryGetValue("date", out token) ? token.ToString() : "N/A";
                this._startTime = obj.TryGetValue("start_time", out token) ? token.ToString() : "N/A";
                this._endTime = obj.TryGetValue("end_time", out token) ? token.ToString() : "N/A";
                this._location = obj.TryGetValue("location", out token) ? token.ToString() : "N/A";
                this._lectureTitle = obj.TryGetValue("title", out token) ? token.ToString() : "N/A";
                this._content = obj.TryGetValue("content", out token) ? token.ToString() : "N/A";
            }
            catch (Exception)
            {
            }
        }
    }
}
