using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NoppaClient.DataModel
{
    public class Course
    {
        private string id;
        private string depId;
        private string name;
        private Uri courseUrl;
        private Uri oodiUrl;
        private Language language;

        List<Link> links;

        public string Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string DepId
        {
            get { return depId; }
        }

        public Uri CourseUrl
        {
            get { return courseUrl; }
        }

        public Uri OodiUrl
        {
            get { return oodiUrl; }
        }

        public Language Language
        {
            get { return language; }
        }

        public List<Link> Links
        {
            get { return links; }
        }

        public Course(string json)
        {
            try
            {
                JToken token;

                JObject obj = JObject.Parse(json);
                this.id = obj.TryGetValue("course_id", out token) ? token.ToString() : "N/A";
                this.depId = obj.TryGetValue("dept_id", out token) ? token.ToString() : "N/A";
                this.name = obj.TryGetValue("name", out token) ? token.ToString() : "N/A";
                this.courseUrl = obj.TryGetValue("course_url", out token) ? new Uri(token.ToString()) : new Uri("www.example.com");
                this.oodiUrl = obj.TryGetValue("course_url_oodi", out token) ? new Uri(token.ToString()) : new Uri("www.example.com");

                switch ((string)obj["noppa_language"])
                {
                    case "fi":
                        language = DataModel.Language.Finnish;
                        break;
                    case "en":
                        language = DataModel.Language.English;
                        break;
                    case "sv":
                        language = DataModel.Language.Swedish;
                        break;
                    default:
                        language = DataModel.Language.Undefined;
                        break;
                }

                foreach (var item in obj["links"])
                {

                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
