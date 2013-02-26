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

        public class Organization
        {
            private string id;
            private Dictionary<Language, string> names;

            public string Name
            {
                get { return names[Settings.Language]; }
            }

            public string Id
            {
                get { return id; }
            }

            public Organization(string id, Dictionary<Language, string> names)
            {
                this.id = id;
                this.names = names;
            }
        }

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

            public Department(string id, string orgId, Dictionary<Language, string> names)
            {
                this.id = id;
                this.orgId = orgId;
                this.names = names;
            }
        }

        public class Course
        {
            private string id;
            private string depId;
            private string name;
            private Uri courseUrl;
            private Uri oodiUrl;
            private Language language;

            List<Link> links;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }

    }
}
