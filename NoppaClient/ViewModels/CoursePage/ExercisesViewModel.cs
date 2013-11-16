using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

using NoppaLib;
using NoppaLib.DataModel;
using NoppaClient.Resources;

namespace NoppaClient.ViewModels
{
    public class ExercisesViewModel : CourseContentViewModel
    {
        private ObservableCollection<CourseExercise> _exercises = new ObservableCollection<CourseExercise>();
        public ObservableCollection<CourseExercise> Exercises { get { return _exercises; } }

        private bool _hasExercises = false;
        public bool HasExercises { get { return _hasExercises; } set { SetProperty(ref _hasExercises, value); } }

        private ObservableCollection<CourseExerciseMaterial> _exerciseMaterial = new ObservableCollection<CourseExerciseMaterial>();
        public ObservableCollection<CourseExerciseMaterial> ExerciseMaterial { get { return _exerciseMaterial; } }

        private bool _hasExerciseMaterial = false;
        public bool HasExerciseMaterial { get { return _hasExerciseMaterial; } set { SetProperty(ref _hasExerciseMaterial, value); } }

        public ExercisesViewModel()
        {
            Title = AppResources.ExercisesTitle;
            Index = 5;
        }

        public override async Task<CourseContentViewModel> LoadDataAsync(string id)
        {
            Task<List<CourseExercise>> exercisesTask = NoppaAPI.GetCourseExercises(id);
            Task<List<CourseExerciseMaterial>> materialTask = NoppaAPI.GetCourseExerciseMaterial(id);

            await Task.WhenAll(exercisesTask, materialTask);

            var exercises = await exercisesTask;
            var exerciseMaterial = await materialTask;

            if (exercises != null)
            {
                foreach (var exercise in exercises)
                {
                    _exercises.Add(exercise);
                }
            }

            if (exerciseMaterial != null)
            {
                foreach (var material in exerciseMaterial)
                {
                    _exerciseMaterial.Add(material);
                }
            }

            HasExercises = exercises != null && exercises.Count > 0;
            HasExerciseMaterial = exerciseMaterial != null && exerciseMaterial.Count > 0;
            IsEmpty = !HasExercises && !HasExerciseMaterial;

            return this;
        }
    }
}
