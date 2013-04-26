﻿using NoppaClient.Resources;
using NoppaLib.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace NoppaClient.ViewModels
{
    public class OverviewItemViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class OverviewViewModel : CourseContentViewModel
    {

        public ObservableCollection<OverviewItemViewModel> Items { get; private set; }

        private string _oodiUrl;
        public string OodiUrl { get { return _oodiUrl; } private set { SetProperty(ref _oodiUrl, value); } }

        public OverviewViewModel()
        {
            Title = AppResources.OverviewTitle;
            Items = new ObservableCollection<OverviewItemViewModel>();
            Index = 1;
        }

        private void AddContent(string title, string content)
        {
            if (!String.IsNullOrWhiteSpace(Detail.StripHtml(content)))
            {
                Items.Add(new OverviewItemViewModel() { Title = title, Content = content });
            }
        }

        public async Task LoadDataAsync(Course course)
        {
            CourseOverview overview = await NoppaAPI.GetCourseOverview(course.Id);

            if (overview != null)
            {
                AddContent(AppResources.CourseNameTitle, course.LongName);
                AddContent(AppResources.CourseCreditsTitle, overview.Credits);
                AddContent(AppResources.CourseStatusTitle, overview.Status);
                AddContent(AppResources.CourseLevelTitle, overview.Level);
                AddContent(AppResources.CourseTeachingPeriodTitle, overview.TeachingPeriod);
                AddContent(AppResources.CourseWorkloadTitle, overview.Workload);
                AddContent(AppResources.CourseLearningOutcomesTitle, overview.LearningOutcomes);
                AddContent(AppResources.CourseContentTitle, overview.Content);
                AddContent(AppResources.CourseAssessmentTitle, overview.Assessment);
                AddContent(AppResources.CourseStudyMaterialTitle, overview.StudyMaterial);
                AddContent(AppResources.CourseSubstitutesTitle, overview.substitutes);
                AddContent(AppResources.CourseCEFRLevelTitle, overview.CefrLevel);
                AddContent(AppResources.CoursePrerequisitesTitle, overview.Prerequisites);
                AddContent(AppResources.CourseGradingScaleTitle, overview.GradingScale);
                AddContent(AppResources.CourseRegistrationTitle, overview.Registration);
                AddContent(AppResources.CourseInstructionLanguageTitle, overview.InstructionLanguage);
                AddContent(AppResources.CourseStaffTitle, overview.Staff);
                AddContent(AppResources.CourseOfficeHoursTitle, overview.OfficeHours);
                AddContent(AppResources.CourseDetailsTitle, overview.Details);

                OodiUrl = overview.OodiUrl;
            }
        }
    }
}
