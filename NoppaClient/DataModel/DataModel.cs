using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.DataModel
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
        public enum Rel
        {
            Self,
            Up,
            Related
        }

        public enum Title
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

        public Title    title;
        public Rel      rel;
        public Uri      uri;
    }
}
