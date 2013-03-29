﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using NoppaClient.DataModel;

namespace NoppaClient.ViewModels
{
    public class ExercisesViewModel : CourseContentViewModel
    {
        private ObservableCollection<CourseExercise> _exercises = new ObservableCollection<CourseExercise>();
        public ObservableCollection<CourseExercise> Exercises { get { return _exercises; } }

        public ExercisesViewModel()
        {
            Title = "Exercises";
            Index = 5;
        }

        public async Task LoadDataAsync(string id)
        {
            List<CourseExercise> exercises = await NoppaAPI.GetCourseExercises(id);
            if (exercises != null)
            {
                foreach (var exercise in exercises)
                {
                    _exercises.Add(exercise);
                }
            }
        }
    }
}
