using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient
{
    namespace DataModel
    {
        public enum Language
        {
            Finnish, // = "fi",
            Swedish, // = "sv",
            English //= "en"
        }

        class Link
        {
            public enum Relation
            {
                Self, // = "self",
                Up, // = "up",
                Related // = "related"
            }

            public enum TitleType
            {
                Overview, // = "overview",
                Pages, // = "pages",
                Results, // = "results",
                Lectures, // = "lectures",
                Exercises, // = "exercises",
                Assignments, // = "assignments",
                Schedule, // = "schedule",
                Material, // = "material",
                ExerciseMaterial, // = "exercise_material"
            }

            public TitleType Title;
            public Relation Rel;
            public Uri      Uri;
        }
    }
}
