using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.DataModel
{
    public class Organization
    {
        public string Id;
        private Dictionary<Language, string> _names;

        public string Name
        {
            get { return _names[Settings.Language]; }
            private set { _names[Settings.Language] = value; }
        }

        Organization(string id, Dictionary<Language, string> names)
        {
            Id = id;
            _names = names;
        }
    }
}
