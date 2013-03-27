using Newtonsoft.Json;

namespace NoppaClient.DataModel
{
    public class Organization
    {
        /* JSON properties */
        [JsonProperty("org_id")]
        public string Id { get; set; }
        public string name_fi { get; set; }
        public string name_sv { get; set; }
        public string name_en { get; set; }

        /* Helper getters */
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
