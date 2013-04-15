using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class DepartmentGroup : ObservableCollection<Department>
    {
        private Organization _organization;
        public string Organization
        {
            get
            {
                switch (App.Settings.Language)
                {
                    case Language.Finnish: return _organization.name_fi;
                    case Language.English: return _organization.name_en;
                    case Language.Swedish: return _organization.name_sv;
                    default: return "";
                }
            }
        }

        public DepartmentGroup(Organization organization)
        {
            _organization = organization;
        }

        public static ObservableCollection<DepartmentGroup> CreateDepartmentGroups(IDictionary<string, Organization> organizations, IEnumerable<Department> items)
        {
            var groups = new Dictionary<string, DepartmentGroup>();

            foreach (var item in items) 
            {
                if (groups.ContainsKey(item.OrgId))
                {
                    groups[item.OrgId].Add(item);
                }
                else
                {
                    Organization org = null;
                    if (!organizations.TryGetValue(item.OrgId, out org))
                    {
                        Debug.WriteLine("No organization found: " + item.OrgId);
                        org = organizations.First().Value;
                    }
                    var group = new DepartmentGroup(org);
                    group.Add(item);
                    groups.Add(item.OrgId, group);
                }
            }

            return new ObservableCollection<DepartmentGroup>(groups.Values.OrderBy(o => o.Organization));
        }
    }
}
