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

        private ICommand _openOodiPage;
        public ICommand OpenOodiPage { get { return _openOodiPage; } }

        public OverviewViewModel()
        {
            CourseId = "T-75.4400";
            Credits = "4 cr";
            Level = "<p>The course is only for students who have completed their general studies.</p>";
            TeachingPeriod = "III - IV";
            Workload = "Lectures 24 h, exercises 42 h, self-study 36 h. HTML";
            LearningOutcomes = "Upon completion of the course the student knows the basic concepts and methods of information retrieval.";
            Content = "Classic information retrieval (Boolean method, vector space model, probability models), [...]";
            Assessment = "Exam and exercises.";
            StudyMaterial = "To be announced later.";
            GradingScale = "0-5";
            InstructionLanguage = "FI. Primarily Finnish. The assessed work may be completed in English or upon request.";
            Details = "Kurssikirja: Christopher D. Manning, Prabhakar Raghavan and Hinrich Schütze, Introduction to Information Retrieval, Cambridge University Press. 2008.  ISBN: 0521865719. Saatavissa osoitteesta: http://nlp.stanford.edu/ IR-book/";

            string oodiUrl = "https://oodi.aalto.fi/a/opintjakstied.jsp?Kieli=6&Tunniste=T-75.4400&html=1";
            var url = new Uri(oodiUrl);
            _openOodiPage = new DelegateCommand(async delegate { await Launcher.LaunchUriAsync(url); });

            Title = "Overview";
            Index = 1;
        }
    }
}
