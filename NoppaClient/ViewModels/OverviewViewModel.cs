using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace NoppaClient.ViewModels
{
    public class OverviewViewModel : CourseContentViewModel
    {
        public string CourseId { get; private set; }
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

        public OverviewViewModel(string id)
        {
            CourseId = id;
            Title = "Overview";
            Index = 1;
        }

        public async Task LoadDataAsync()
        {
            DataModel.CourseOverview overview = await NoppaAPI.GetCourseOverview(CourseId);

            if (overview != null)
            {
                Credits = overview.Credits;
                Level = overview.Level;
                TeachingPeriod = overview.TeachingPeriod;
                Workload = overview.Workload;
                LearningOutcomes = overview.LearningOutcomes;
                Assessment = overview.Assessment;
                StudyMaterial = overview.StudyMaterial;
                GradingScale = overview.GradingScale;
                InstructionLanguage = overview.InstructionLanguage;
                Details = overview.Details;

                var noppaUrl = new Uri(String.Format("https://noppa.aalto.fi/noppa/kurssi/{0}/etusivu", CourseId));
                var oodiUrl = new Uri(overview.OodiUrl);

                _openOodiPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(oodiUrl); });
                _openNoppaPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(noppaUrl); });
            }
        }
    }
}
