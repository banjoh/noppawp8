using NoppaClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace NoppaClient.ViewModels
{
    public class OverviewViewModel : CourseContentViewModel
    {
        public string LongName { get; private set; }
        public string Credits { get; private set; }
        public string Status { get; private set; }
        public string Level { get; private set; }
        public string TeachingPeriod { get; private set; }
        public string Workload { get; private set; }
        public string LearningOutcomes { get; private set; }
        public string Content { get; private set; }
        public string Assessment { get; private set; }
        public string StudyMaterial { get; private set; }
        public string GradingScale { get; private set; }
        public string InstructionLanguage { get; private set; }
        public string Details { get; private set; }
        public string OodiUrl { get; private set; }
        public string Substitutes { get; private set; }
        public string CEFRLevel { get; private set; } 
        public string Registration { get; private set; }
        public string Staff { get; private set; }
        public string OfficeHours { get; private set; }

        private ICommand _openNoppaPage;
        public ICommand OpenNoppaPage { get { return _openNoppaPage; } }

        private ICommand _openOodiPage;
        public ICommand OpenOodiPage { get { return _openOodiPage; } }

        public OverviewViewModel()
        {
            Title = AppResources.OverviewTitle;
            Index = 1;
        }

        public async Task LoadDataAsync(NoppaLib.DataModel.Course course)
        {
            NoppaLib.DataModel.CourseOverview overview = await NoppaAPI.GetCourseOverview(course.Id);

            if (overview != null)
            {
                LongName = course.LongName;
                Credits = Detail.StripHtml(overview.Credits);
                Level = Detail.StripHtml(overview.Level);
                TeachingPeriod = Detail.StripHtml(overview.TeachingPeriod);
                Workload = Detail.StripHtml(overview.Workload);
                LearningOutcomes = Detail.StripHtml(overview.LearningOutcomes);
                Assessment = Detail.StripHtml(overview.Assessment);
                StudyMaterial = Detail.StripHtml(overview.StudyMaterial);
                GradingScale = Detail.StripHtml(overview.GradingScale);
                InstructionLanguage = Detail.StripHtml(overview.InstructionLanguage);
                Details = Detail.StripHtml(overview.Details);

                var noppaUrl = new Uri(String.Format("https://noppa.aalto.fi/noppa/kurssi/{0}/etusivu", course.Id));
                var oodiUrl = new Uri(overview.OodiUrl);

                _openOodiPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(oodiUrl); });
                _openNoppaPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(noppaUrl); });
            }
        }
    }
}
