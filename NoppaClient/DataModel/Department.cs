using System.Collections.Generic;
using Newtonsoft.Json;

namespace NoppaClient.DataModel
{
    public class Department
    {
        [JsonProperty("dept_id")]
        public string Id { get; set; }
        [JsonProperty("org_id")]
        public string OrgId { get; set; }
        public string name_fi { get; set; }
        public string name_sv { get; set; }
        public string name_en { get; set; }
        public List<Link> links { get; set; }

        public string Name
        {
            get
            {
                switch (App.Settings.Language)
                {
                    case Language.Finnish: return name_fi;
                    case Language.English: return name_en;
                    case Language.Swedish: return name_sv;
                    default: return "";
                }
            }
        }
    }
}
