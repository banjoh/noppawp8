using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NoppaClient.Resources;

namespace NoppaClient.ViewModels
{
    public class MainViewModel : BindableBase
    {
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        private ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items { get { return _items; } }

        private ObservableCollection<ItemViewModel> _events = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Events { get { return _events; } }

        private CourseListViewModel _myCourses = new CourseListViewModel();
        public CourseListViewModel MyCourses { get { return _myCourses; } }

        private ObservableCollection<ItemViewModel> _news = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> News { get { return _news; } }

        private ObservableCollection<DepartmentGroup> _departments;
        public ObservableCollection<DepartmentGroup> Departments { get { return _departments; } }

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
        public async void LoadDataAsync()
        {
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

            Events.Add(new ItemViewModel() { LineOne = "Assignment", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            Events.Add(new ItemViewModel() { LineOne = "Deadline", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            Events.Add(new ItemViewModel() { LineOne = "Guest lecture", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            Events.Add(new ItemViewModel() { LineOne = "Exam", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

            News.Add(new ItemViewModel() { LineOne = "No lecture today", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            News.Add(new ItemViewModel() { LineOne = "Results for January exam", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            News.Add(new ItemViewModel() { LineOne = "Homeworks graded", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });

            _departments = DepartmentGroup.CreateDepartmentGroups(new DepartmentViewModel[] {
                // This is now the same data as in the sample data, but this should be taken from the data model
                new DepartmentViewModel("School of Assorted Magicks", "Department of Spirit Conjuring", "conj"),
                new DepartmentViewModel("School of Assorted Magicks", "Department of Illusion and Charms", "illusion"), 
                new DepartmentViewModel("School of Assorted Magicks", "Department of Black Arts", "black"),
    
                new DepartmentViewModel("School of Arcane Alchemy", "Department of Transmutation", "trans"),
                new DepartmentViewModel("School of Arcane Alchemy", "Department of Good and Bad Air", "air"),
                new DepartmentViewModel("School of Arcane Alchemy", "Department of Love Potions", "potions"),

                new DepartmentViewModel ("School of Witchcraft", "Department of Flying on Broomsticks", "broom"),
                new DepartmentViewModel ("School of Witchcraft", "Department of Cauldron Maintenance", "cauldron"),
                new DepartmentViewModel ("School of Witchcraft", "Department of Familiar Pacts", "familiar"),
            });

            await _myCourses.LoadMyCoursesAsync();

            this.IsDataLoaded = true;
        }
    }
}