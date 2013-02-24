using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.DataModel
{
    public class Department
    {
        public string Id;
        public string OrganizationId;
        private Dictionary<Language, string> _names;

        public string Name
        {
            get { return _names[Settings.Language]; }
            private set { _names[Settings.Language] = value; }
        }

        Department(string id, string orgId, Dictionary<Language, string> names)
        {
            Id = id;
            OrganizationId = orgId;
            _names = names;
        }
    }
}
