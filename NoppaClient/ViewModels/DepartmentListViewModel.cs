using NoppaLib;
using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    class DepartmentListViewModel : BindableBase
    {
        private ObservableCollection<DepartmentGroup> _departments = new ObservableCollection<DepartmentGroup>();
        public ObservableCollection<DepartmentGroup> Departments
        {
            get { return _departments; }
            private set
            {
                if (SetProperty(ref _departments, value))
                    IsDepartmentListEmpty = value.Count == 0;
            }
        }

        private bool _isDepartmentListEmpty = true;
        public bool IsDepartmentListEmpty
        {
            get { return _isDepartmentListEmpty; }
            private set { SetProperty(ref _isDepartmentListEmpty, value); }
        }

        public ICommand DepartmentActivatedCommand { get; private set; }

        public DepartmentListViewModel() { }
        public DepartmentListViewModel(INavigationController navigationController)
        {
            DepartmentActivatedCommand = ControllerUtil.MakeShowDepartmentCommand(navigationController);
        }

        /**
         * Loads all organizations and departments at one go (asynchronously).
         * Cannot be separated into adding groups as they are received because
         * of the NoppaApi. Either make two requests to get all the data or
         * make 1 + #organizations.
         */
        public async void LoadDepartmentGroupsAsync()
        {
            try
            {
                List<Organization> orgs = await NoppaAPI.GetAllOrganizations();
                List<Department> depts = await NoppaAPI.GetDepartments();

                if (depts != null && orgs != null)
                {
                    var orgMap = new Dictionary<string, Organization>();
                    foreach (var org in orgs)
                    {
                        orgMap.Add(org.Id, org);
                    }

                    Departments = DepartmentGroup.CreateDepartmentGroups(orgMap, depts);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadDepartmentGroupsAsync: Caught exception: {0}", ex.Message);
            }
        }
    }
}
