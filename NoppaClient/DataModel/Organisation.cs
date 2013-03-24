using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NoppaClient.DataModel
{
    public class Organization
    {
        private string id;
        private Dictionary<Language, string> names;
        private List<Link> links;

        public string Name
        {
            get { return names[App.Settings.Language]; }
        }

        public string Id
        {
            get { return id; }
        }

        public Organization(string json) : this(JObject.Parse(json)) {}
        public Organization(JObject obj)
        {
            this.id = (string)obj["org_id"];

            this.names = new Dictionary<Language, string>();            
            this.names.Add(Language.English, (string)obj["name_en"]);
            this.names.Add(Language.Finnish, (string)obj["name_fi"]);
            this.names.Add(Language.Swedish, (string)obj["name_sv"]);

            foreach (var item in obj["links"])
            {

            }
        }
    }
}
