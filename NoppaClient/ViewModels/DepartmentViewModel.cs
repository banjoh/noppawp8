using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class DepartmentViewModel
    {
        public string Organization { get; private set; }
        public string Name { get; private set; }
        public string Id { get; private set; }

        public DepartmentViewModel(string organization, string name, string id)
        {
            Organization = organization;
            Name = name;
            Id = id;
        }
    }

    public class DepartmentGroup : ObservableCollection<DepartmentViewModel>
    {
        private string _organization;
        public string Organization { get { return _organization; } }

        public DepartmentGroup(string organization)
        {
            _organization = organization;
        }

        public static ObservableCollection<DepartmentGroup> CreateDepartmentGroups(IEnumerable<DepartmentViewModel> items)
        {
            var groups = new Dictionary<string, DepartmentGroup>();

            foreach (var item in items) 
            {
                if (groups.ContainsKey(item.Organization))
                {
                    groups[item.Organization].Add(item);
                }
                else
                {
                    var group = new DepartmentGroup(item.Organization);
                    group.Add(item);
                    groups.Add(item.Organization, group);
                }
            }

            return new ObservableCollection<DepartmentGroup>(groups.Values.OrderBy(o => o.Organization));
        }
    }
}
