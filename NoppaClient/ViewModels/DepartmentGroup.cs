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
    public class DepartmentProxy
    {
        private Department _department;

        public string Id { get { return _department.Id; } }
        public string OrgId { get { return _department.OrgId; } }
        public string Name {
            get
            {
                switch (App.Settings.Language)
                {
                    case Language.Finnish: return _department.name_fi;
                    case Language.English: return _department.name_en;
                    case Language.Swedish: return _department.name_sv;;
                    default: return _department.name_en;
                }
            }
        }
        public DepartmentProxy(NoppaLib.DataModel.Department dept)
        {
            _department = dept;
        }
    }

    public class DepartmentGroup : ObservableCollection<DepartmentProxy>
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

            foreach (var dept in items) 
            {
                DepartmentProxy item = new DepartmentProxy(dept);
                if (groups.ContainsKey(dept.OrgId))
                {
                    groups[item.OrgId].Add(item);
                }
                else
                {
                    Organization org = null;
                    if (!organizations.TryGetValue(item.OrgId, out org))
                    {
                        Debug.WriteLine("No organization found: " + dept.OrgId);
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
