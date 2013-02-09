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
            Finnish = "fi",
            Swedish = "sv",
            English = "en"
        }

        class Link
        {
            enum Rel
            {
                Self = "self",
                Up = "up",
                Related = "related"
            }

            enum Title
            {
                Overview = "overview",
                Pages = "pages",
                Results = "results",
                Lectures = "lectures",
                Exercises = "exercises",
                Assignments = "assignments",
                Schedule = "schedule",
                Material = "material",
                ExerciseMaterial = "exercise_material"
            }

            public Title    Title;
            public Rel      Rel;
            public Uri      Uri;
        }

        class Organization
        {
            public string Id;
            private Dictionary<Language, string> _names;

            public string Name
            {
                get { return _names[Settings.Language]; }
                private set;
            }

            Organization(string id, Dictionary<Language, string> names)
            {
                Id = id;
                _names = names;
            }
        }

        class Department
        {
            public string Id;
            public string OrganizationId;
            private Dictionary<Language, string> _names;

            public string Name
            {
                get { return _names[Settings.Language]; }
                private set;
            }

            Department(string id, string orgId, Dictionary<Language, string> names)
            {
                Id = id;
                OrganizationId = orgId;
                _names = names;
            }
        }

        class Course
        {
            string Id;
            string DeparmentId;
            string Name;
            Uri CourseUrl;
            Uri OodiUrl;
            Language CourseLanguage;

            List<Link> Links;
        }

    }
}
