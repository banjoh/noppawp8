using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace NoppaLib.DataModel
{
    public class Course
    {
        [JsonProperty("course_id")]             public string Id { get; set; }
        [JsonProperty("dept_id")]               public string DepartmentId { get; set; }
        [JsonProperty("name")]                  public string Name { get; set; }
        [JsonProperty("course_url")]            public string Url { get; set; }
        [JsonProperty("course_url_oodi")]       public string OodiUrl { get; set; }
        [JsonProperty("noppa_language")]        public string Language { get; set; }
        [JsonProperty("links")]                 public List<Link> Links { get; set; }

                                                public string LongName { get { return Id + " " + Name; } }
    }

    public class CourseOverview
    {
        [JsonProperty("course_id")]             public string Id { get; set; }
        [JsonProperty("credits")]               public string Credits { get; set; }
        [JsonProperty("status")]                public string Status { get; set; }
        [JsonProperty("level")]                 public string Level { get; set; }
        [JsonProperty("teaching_period")]       public string TeachingPeriod { get; set; }
        [JsonProperty("workload")]              public string Workload { get; set; }
        [JsonProperty("learning_outcomes")]     public string LearningOutcomes { get; set; }
        [JsonProperty("content")]               public string Content { get; set; }
        [JsonProperty("assessment")]            public string Assessment { get; set; }
        [JsonProperty("study_material")]        public string StudyMaterial { get; set; }
        [JsonProperty("substitutes")]           public string substitutes { get; set; }
        [JsonProperty("CEFR_level")]            public string CefrLevel { get; set; }
        [JsonProperty("prerequisites")]         public string Prerequisites { get; set; }
        [JsonProperty("grading_scale")]         public string GradingScale { get; set; }
        [JsonProperty("registration")]          public string Registration { get; set; }
        [JsonProperty("instruction_language")]  public string InstructionLanguage { get; set; }
        [JsonProperty("staff")]                 public string Staff { get; set; }
        [JsonProperty("office_hours")]          public string OfficeHours { get; set; }
        [JsonProperty("details")]               public string Details { get; set; }
        [JsonProperty("oodi_url")]              public string OodiUrl { get; set; }
    }

    public class CourseAdditionalPage
    {
        [JsonProperty("title")]                 public string Title { get; set; }
        [JsonProperty("url")]                   public string Url { get; set; }
        [JsonProperty("text")]                  public string Text { get; set; }  
        [JsonProperty("authentication_required")] public string AuthenticationRequired { get; set; }
    }

    public class CourseNews
    {
        [JsonProperty("date")]                  public DateTime Date { get; set; }
        [JsonProperty("title")]                 public string Title { get; set; }
        [JsonProperty("content")]               public string Content { get; set; }
        [JsonProperty("link")]                  public List<Link> Links { get; set; }
    }

    public class CourseResult
    {
        [JsonProperty("course_id")]             public string CourseId { get; set; }
        [JsonProperty("date")]                  public DateTime Date { get; set; }
        [JsonProperty("grade_name")]            public string GradeName { get; set; }
        [JsonProperty("url")]                   public string Url { get; set; }
        [JsonProperty("published")]             public DateTime Published { get; set; }
        [JsonProperty("grade_review")]          public string GradeReview { get; set; }
        [JsonProperty("grade_scale")]           public string GradeScale { get; set; }
    }

    public class CourseLecture
    {
        [JsonProperty("lecture_id")]            public string LectureId { get; set; }
        [JsonProperty("date")]                  public DateTime Date { get; set; }
        [JsonProperty("start_time")]            public DateTime StartTime { get; set; }
        [JsonProperty("end_time")]              public DateTime EndTime { get; set; }
        [JsonProperty("location")]              public string Location { get; set; }
        [JsonProperty("title")]                 public string Title { get; set; }
        [JsonProperty("content")]               public string Content { get; set; }
        [JsonProperty("materials")]             public List<Material> Materials { get; set; }
        [JsonProperty("authentication_required")] public bool authentication_required { get; set; }
    }

    public class CourseExercise
    {
        [JsonProperty("course_id")]             public string CourseId { get; set; }
        [JsonProperty("group")]                 public string Group { get; set; }
        [JsonProperty("weekday")]               public string Weekday { get; set; }
        [JsonProperty("start_time")]            public DateTime StartTime { get; set; }
        [JsonProperty("end_time")]              public DateTime EndTime { get; set; }
        [JsonProperty("location")]              public string Location { get; set; }
        [JsonProperty("start_date")]            public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]              public DateTime EndDate { get; set; }
        [JsonProperty("additional_info")]       public string AdditionalInfo { get; set; }
    }

    public class CourseAssignment
    {
        [JsonProperty("deadline")]              public DateTime Deadline { get; set; }
        [JsonProperty("title")]                 public string Title { get; set; }
        [JsonProperty("content")]               public string Content { get; set; }
        [JsonProperty("material")]              public List<Material> Material { get; set; }
        [JsonProperty("authentication_required")] public bool AuthenticationRequired { get; set; }
    }

    public class CourseEvent
    {
        [JsonProperty("course_id")]             public string CourseId { get; set; }
        [JsonProperty("type")]                  public string Type { get; set; }
        [JsonProperty("title")]                 public string Title { get; set; }
        [JsonProperty("weekday")]               public string Weekday { get; set; }
        [JsonProperty("location")]              public string Location { get; set; }
        [JsonProperty("start_time")]            public DateTime StartTime { get; set; }
        [JsonProperty("end_time")]              public DateTime EndTime { get; set; }
        [JsonProperty("start_date")]            public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]              public DateTime EndDate { get; set; }
    }

    public class CourseMaterial
    {
        [JsonProperty("course_id")]             public string CourseId { get; set; }
        [JsonProperty("title")]                 public string Title { get; set; }
        [JsonProperty("description")]           public string Description { get; set; }
        [JsonProperty("url")]                   public string Url { get; set; }
        [JsonProperty("authentication_required")] public string AuthenticationRequired { get; set; }
    }

    public class CourseExerciseMaterial
    {
        [JsonProperty("course_id")]             public string CourseId { get; set; }
        [JsonProperty("number")]                public string Number { get; set; }
        [JsonProperty("topic")]                 public string Topic { get; set; }
        [JsonProperty("material")]              public List<Material> Material { get; set; }
    }

    public class Content
    {
        [JsonProperty("text_location")]
        public string TextLocation { get; set; }
        [JsonProperty("text_content")]
        public string TextContent { get; set; }
    }

    public class CourseText
    {
        [JsonProperty("page")]
        public string Page { get; set; }
        [JsonProperty("content")]
        public List<Content> Content { get; set; }
    }
}
