using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NoppaClient.Resources;
using NoppaClient.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoppaClient.ViewModels
{
    public class MainViewModel : BindableBase
    {
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        private ObservableCollection<CourseEvent> _events = new ObservableCollection<CourseEvent>();
        public ObservableCollection<CourseEvent> Events { get { return _events; } }

        private CourseListViewModel _myCourses = new CourseListViewModel();
        public CourseListViewModel MyCourses { get { return _myCourses; } }

        private ObservableCollection<CourseNews> _news = new ObservableCollection<CourseNews>();
        public ObservableCollection<CourseNews> News { get { return _news; } }

        private ObservableCollection<DepartmentGroup> _departments;
        public ObservableCollection<DepartmentGroup> Departments {
            get { return _departments; }
            set { SetProperty(ref _departments, value); }
        }

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

        public MainViewModel()
        {
            // Here, make a model instance or something, and start filling in the 
            // view model data
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadDataAsync()
        {

            List<DepartmentViewModel> models = new List<DepartmentViewModel>();

            try
            {
                List<Organization> orgs = await NoppaAPI.GetAllOrganizations();

                if (orgs != null)
                {
                    foreach (var org in orgs)
                    {
                        var depts = await NoppaAPI.GetDepartments(org.Id);

                        if (depts != null)
                        {
                            foreach (var dept in depts)
                            {
                                models.Add(new DepartmentViewModel(org.Name, dept.Name, dept.Id));
                            }
                        }
                    }
                }

                Departments = DepartmentGroup.CreateDepartmentGroups(models);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Caught exception: {0}", ex.Message);
            }

            this.IsDataLoaded = true;
        }
    }
}