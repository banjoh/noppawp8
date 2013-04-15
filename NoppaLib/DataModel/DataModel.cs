using Newtonsoft.Json;

namespace NoppaLib.DataModel
{
    public enum Language
    {
        Undefined,
        Finnish,
        Swedish,
        English
    }

    public class Link
    {
        public enum RelType
        {
            Self,
            Up,
            Related
        }

        public enum TitleType
        {
            Overview,
            Pages,
            Results,
            Lectures,
            Exercises,
            Assignments,
            Schedule,
            Material,
            ExerciseMaterial
        }

        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("rel")]
        public string Rel { get; set; }
    }

    public class Material
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
