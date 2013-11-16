using NoppaClient.Resources;
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

using NoppaLib;

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
            var stripped = Detail.StripHtml(content);
            if (!String.IsNullOrWhiteSpace(stripped))
            {
                Items.Add(new OverviewItemViewModel() { Title = title, Content = stripped });
            }
        }

        public override async Task<CourseContentViewModel> LoadDataAsync(string id)
        {
            Course course = await NoppaAPI.GetCourse(id);
            CourseOverview overview = await NoppaAPI.GetCourseOverview(id);

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

            return this;
        }
    }
}
