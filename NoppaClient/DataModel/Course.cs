using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoppaClient.DataModel
{
    public class Course
    {
        string Id;
        string DeparmentId;
        public string Name { get; set; }
        Uri CourseUrl;
        Uri OodiUrl;
        Language CourseLanguage;

        List<Link> Links;
    }
}
