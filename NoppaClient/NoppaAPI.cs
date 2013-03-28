using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.IO;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NoppaClient.DataModel;

namespace NoppaClient
{
    public class NoppaAPI
    {
        private const string _apiURL = "http://noppa-api-dev.aalto.fi/api/v1";
        private static readonly int _timeout = 35000; /* 5secs */
        private static string _apiKey;
        private static NoppaAPI _instance;

        private NoppaAPI()
        {
            _apiKey = APIKeyHolder.Key;
        }

        public static NoppaAPI GetInstance()
        {
            if (_instance == null)
                _instance = new NoppaAPI();

            return _instance;
        }

        #region API Call methods

        private Task<HttpWebResponse> CallAPIAsync(string query)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_apiURL + query);
            System.Diagnostics.Debug.WriteLine("Query: {0}", request.RequestUri.ToString());
            request.Method = "GET";
            request.BeginGetResponse(asyncResponse =>
            {
                try
                {
                    HttpWebRequest responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
                    HttpWebResponse response = responseRequest.EndGetResponse(asyncResponse) as HttpWebResponse;
                    taskComplete.TrySetResult(response);
                }
                catch (WebException webExc)
                {
                    taskComplete.TrySetException(webExc);
                }
            }, request);
            return taskComplete.Task;
        }

        private async Task<T> GetObject<T>(string format, params object[] args) where T : class
        {
            HttpWebResponse response;

            try
            {
                Task<HttpWebResponse> responseTask = CallAPIAsync(String.Format(format, args));

                /* Handle the timeout */
                var completeTask = await Task.WhenAny(responseTask, Task.Delay(_timeout));
                if (completeTask == responseTask)
                    response = await responseTask;
                else
                {
                    /* Timeout */
                    System.Diagnostics.Debug.WriteLine("NoppaApiClient: Timed out ({0} ms)", _timeout);
                    return null;
                }
            }
            catch (WebException webExc)
            {
                /* Caught exception */
                System.Diagnostics.Debug.WriteLine("NoppaApiClient: Caught exception: {0}", webExc.Message);
                return null;
            }

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return JsonConvert.DeserializeObject<T>(await sr.ReadToEndAsync());
            }
        }

        #endregion

        #region Organization getters

        public static async Task<List<Organization>> GetAllOrganizations()
        {
            return await NoppaAPI.GetInstance().GetObject<List<Organization>>("/organizations?key={0}", _apiKey);
           
        }

        public static async Task<Organization> GetOrganization(string organization_id)
        {
            return await NoppaAPI.GetInstance().GetObject<Organization>("/organizations/{0}?key={1}", organization_id, _apiKey);
        }

        #endregion

        #region Department getters

        public static async Task<List<Department>> GetDepartments(string organization_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<Department>>("/departments?key={0}&org_id={1}", _apiKey, organization_id);
        }

        public static async Task<List<Department>> GetDepartments()
        {
            return await NoppaAPI.GetInstance().GetObject<List<Department>>("/departments?key={0}", _apiKey);
        }

        public static async Task<Department> GetDepartment(string department_id)
        {
            return await NoppaAPI.GetInstance().GetObject<Department>("/departments/{0}?key={1}", department_id, _apiKey);
        }

        #endregion

        #region Course getters

        public static async Task<List<Course>> GetCourses(string search_pattern, string org_id = "", string dept_id = "")
        {
            return await NoppaAPI.GetInstance().GetObject<List<Course>>("/courses?key={0}{1}{2}{3}", _apiKey,
                search_pattern != "" ? "&search=" + HttpUtility.UrlEncode(search_pattern) : "",
                org_id != "" ? "&org_id=" + org_id : "",
                dept_id != "" ? "&dept_id=" + dept_id : "");
        }

        public static async Task<Course> GetCourse(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<Course>("/courses/{0}?key={1}", course_id, _apiKey);
        }

        #endregion

        #region Course Content getters

        public static async Task<CourseOverview> GetCourseOverview(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<CourseOverview>("/courses/{0}/overview?key={1}", course_id, _apiKey);
        }

        public static async Task<CourseAdditionalPage> GetCourseAdditionalPages(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<CourseAdditionalPage>("/courses/{0}/pages?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseNews>> GetCourseNews(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseNews>>("/courses/{0}/news?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseResult>> GetCourseResults(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseResult>>("/courses/{0}/results?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseLecture>> GetCourseLectures(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseLecture>>("/courses/{0}/lectures?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseExercise>> GetCourseExercises(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseExercise>>("/courses/{0}/exercises?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseAssignment>> GetCourseAssignments(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseAssignment>>("/courses/{0}/assignments?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseEvent>> GetCourseEvents(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseEvent>>("/courses/{0}/events?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseMaterial>> GetCourseMaterial(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseMaterial>>("/courses/{0}/material?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseExerciseMaterial>> GetCourseExerciseMaterial(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseExerciseMaterial>>("/courses/{0}/exercise_material?key={1}", course_id, _apiKey);
        }

        public static async Task<List<CourseText>> GetCourseAdditionalTexts(string course_id)
        {
            return await NoppaAPI.GetInstance().GetObject<List<CourseText>>("/courses/{0}/texts?key={1}", course_id, _apiKey);
        }

        #endregion
    }
}
