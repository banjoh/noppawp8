using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NoppaClient.Resources;
using NoppaClient.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoppaClient.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private ObservableCollection<CourseEvent> _events = new ObservableCollection<CourseEvent>();
        public ObservableCollection<CourseEvent> Events { get { return _events; } }

        private CourseListViewModel _myCourses = new CourseListViewModel(new PhoneNavigationController());
        public CourseListViewModel MyCourses { get { return _myCourses; } }

        private ObservableCollection<CourseNews> _news = new ObservableCollection<CourseNews>();
        public ObservableCollection<CourseNews> News { get { return _news; } }

        private ObservableCollection<DepartmentGroup> _departments;
        public ObservableCollection<DepartmentGroup> Departments {
            get { return _departments; }
            private set { SetProperty(ref _departments, value); }
        }

        public ICommand DepartmentActivatedCommand { get; private set; }
        public ICommand ShowSettingsCommand { get; private set; }
        public ICommand ShowSearchCommand { get; private set; }
        public ICommand ActivateCourseCommand { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get { return _sampleProperty; }
            set { SetProperty(ref _sampleProperty, value); }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public string Title
        {
            get { return AppResources.ApplicationTitle; }
        }

        private bool _isDataLoaded = false;
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            private set { SetProperty(ref _isDataLoaded, value); }
        }

        public MainViewModel(INavigationController navigationController)
        {
            DepartmentActivatedCommand = ControllerUtil.MakeShowDepartmentCommand(navigationController);
            ShowSettingsCommand = ControllerUtil.MakeShowSettingsCommand(navigationController);
            ShowSearchCommand = ControllerUtil.MakeShowCourseSearchCommand(navigationController);
            ActivateCourseCommand = ControllerUtil.MakeShowCourseCommand(navigationController);
            // Here, make a model instance or something, and start filling in the 
            // view model data
         }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadDataAsync()
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
                System.Diagnostics.Debug.WriteLine("Caught exception: {0}", ex.Message);
            }

            this.IsDataLoaded = true;
        }
    }
}