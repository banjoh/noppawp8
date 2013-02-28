using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NoppaClient.DataModel
{
    public class Department
    {
        private string id;
        private string orgId;
        private Dictionary<Language, string> names;

        public string Name
        {
            get { return names[Settings.Language]; }
        }

        public string Id
        {
            get { return id; }
        }

        public string OrgId
        {
            get { return orgId; }
        }

        public Department(string json)
        {
            JObject obj = JObject.Parse(json);
            this.id = (string)obj["dept_id"];
            this.orgId = (string)obj["org_id"];

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
