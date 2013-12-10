using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.IO;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NoppaLib.DataModel;

namespace NoppaLib
{
    public class NoppaAPI
    {
        private static NoppaImpl _instance = NoppaImpl.GetInstance();

        #region Organization getters

        public static async Task<List<Organization>> GetAllOrganizations()
        {
            return await _instance.GetObject<List<Organization>>(Cache.PolicyLevel.Long, "/organizations?key={0}", APIConfigHolder.Key);
           
        }

        public static async Task<Organization> GetOrganization(string organization_id)
        {
            return await _instance.GetObject<Organization>(Cache.PolicyLevel.Long, "/organizations/{0}?key={1}", organization_id, APIConfigHolder.Key);
        }

        #endregion

        #region Department getters

        public static async Task<List<Department>> GetDepartments(string organization_id)
        {
            return await _instance.GetObject<List<Department>>(Cache.PolicyLevel.Long, "/departments?key={0}&org_id={1}", APIConfigHolder.Key, organization_id);
        }

        public static async Task<List<Department>> GetDepartments()
        {
            return await _instance.GetObject<List<Department>>(Cache.PolicyLevel.Long, "/departments?key={0}", APIConfigHolder.Key);
        }

        public static async Task<Department> GetDepartment(string department_id)
        {
            return await _instance.GetObject<Department>(Cache.PolicyLevel.Long, "/departments/{0}?key={1}", department_id, APIConfigHolder.Key);
        }

        #endregion

        #region Course getters

        public static Task<List<Course>> GetCoursesByOrganization(string organization)
        {
            return GetCourses("", organization, "");
        }

        public static Task<List<Course>> GetCoursesByDepartment(string department)
        {
            return GetCourses("", "", department);
        }

        public static Task<List<Course>> GetCourses(string search_pattern, string org_id = "", string dept_id = "", bool use_cached_results = true)
        {
            return _instance.GetObject<List<Course>>(
                use_cached_results ? Cache.PolicyLevel.Short : Cache.PolicyLevel.Reload,
                "/courses?key={0}{1}{2}{3}", APIConfigHolder.Key,
                search_pattern != "" ? "&search=" + HttpUtility.UrlEncode(search_pattern) : "",
                org_id != "" ? "&org_id=" + org_id : "",
                dept_id != "" ? "&dept_id=" + dept_id : ""
                );
        }

        public static async Task<Course> GetCourse(string course_id)
        {
            return await _instance.GetObject<Course>(Cache.PolicyLevel.Long, "/courses/{0}?key={1}", course_id, APIConfigHolder.Key);
        }

        #endregion

        #region Course Content getters

        public static Task<CourseOverview> GetCourseOverview(string course_id)
        {
            return _instance.GetObject<CourseOverview>(Cache.PolicyLevel.Short, "/courses/{0}/overview?key={1}", course_id, APIConfigHolder.Key);
        }

        public static Task<CourseAdditionalPage> GetCourseAdditionalPages(string course_id)
        {
            return _instance.GetObject<CourseAdditionalPage>(Cache.PolicyLevel.Short, "/courses/{0}/pages?key={1}", course_id, APIConfigHolder.Key);
        }

        public static async Task<List<CourseNews>> GetCourseNews(string course_id)
        {
            var list = await _instance.GetObject<List<CourseNews>>(Cache.PolicyLevel.BypassCache, "/courses/{0}/news?key={1}", course_id, APIConfigHolder.Key);

            /* Idiotical hack to work around the fact that Noppa API does not
             * include the course id in the result */
            foreach (var news in list)
                news.CourseId = course_id;

            return list;
        }

        public static Task<List<CourseResult>> GetCourseResults(string course_id)
        {
            return _instance.GetObject<List<CourseResult>>(Cache.PolicyLevel.Short, "/courses/{0}/results?key={1}", course_id, APIConfigHolder.Key);
        }

        public static Task<List<CourseLecture>> GetCourseLectures(string course_id)
        {
            return _instance.GetObject<List<CourseLecture>>(Cache.PolicyLevel.Short, "/courses/{0}/lectures?key={1}", course_id, APIConfigHolder.Key);
        }

        public static Task<List<CourseExercise>> GetCourseExercises(string course_id)
        {
            return _instance.GetObject<List<CourseExercise>>(Cache.PolicyLevel.Short, "/courses/{0}/exercises?key={1}", course_id, APIConfigHolder.Key);
        }

        public static Task<List<CourseAssignment>> GetCourseAssignments(string course_id)
        {
            return _instance.GetObject<List<CourseAssignment>>(Cache.PolicyLevel.Short, "/courses/{0}/assignments?key={1}", course_id, APIConfigHolder.Key);
        }

        public static Task<List<CourseEvent>> GetCourseEvents(string course_id)
        {
            return _instance.GetObject<List<CourseEvent>>(Cache.PolicyLevel.BypassCache, "/courses/{0}/events?key={1}", course_id, APIConfigHolder.Key);
        }

        public static Task<List<CourseMaterial>> GetCourseMaterial(string course_id)
        {
            return _instance.GetObject<List<CourseMaterial>>(Cache.PolicyLevel.Short, "/courses/{0}/material?key={1}", course_id, APIConfigHolder.Key);
        }

        public static Task<List<CourseExerciseMaterial>> GetCourseExerciseMaterial(string course_id)
        {
            return _instance.GetObject<List<CourseExerciseMaterial>>(Cache.PolicyLevel.Short, "/courses/{0}/exercise_material?key={1}", course_id, APIConfigHolder.Key);
        }

        public static Task<List<CourseText>> GetCourseAdditionalTexts(string course_id)
        {
            return _instance.GetObject<List<CourseText>>(Cache.PolicyLevel.Short, "/courses/{0}/texts?key={1}", course_id, APIConfigHolder.Key);
        }

        #endregion
    }
}
