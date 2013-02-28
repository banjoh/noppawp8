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
    public static class HttpExtension
    {
        public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<Stream>();
            request.BeginGetRequestStream(ar =>
            {
                Stream requestStream = request.EndGetRequestStream(ar);
                taskComplete.TrySetResult(requestStream);
            }, request);
            return taskComplete.Task;
        }
    }

    public class NoppaApiClient
    {
        private const string _apiURL = "http://noppa-api-dev.aalto.fi/api/v1";
        private string _apiKey;

        public NoppaApiClient(string apiKey)
        {
            this._apiKey = apiKey;
        }

        public Task<HttpWebResponse> CallAPIAsync(string query)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(query);
            request.Method = "GET";
            request.BeginGetResponse(asyncResponse =>
            {
                try
                {
                    HttpWebRequest responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
                    HttpWebResponse response = (HttpWebResponse)responseRequest.EndGetResponse(asyncResponse);
                    taskComplete.TrySetResult(response);
                }
                catch (WebException webExc)
                {
                    HttpWebResponse failedResponse = (HttpWebResponse)webExc.Response;
                    taskComplete.TrySetResult(failedResponse);
                }
            }, request);
            return taskComplete.Task;
        }

        public async Task<List<Organization>> GetAllOrganizations()
        {
            string query = String.Format("{0}/organizations?key={1}", _apiURL, _apiKey);
            
            HttpWebResponse response = await CallAPIAsync(query);
            List<Organization> organizations = new List<Organization>();
            
            using (var sr = new StreamReader(response.GetResponseStream()))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JArray orgs = (JArray)JArray.ReadFrom(reader);

                foreach (var item in orgs)
                {
                    Dictionary<Language, string> names = new Dictionary<Language, string>();
                    names[Language.English] = (string)item["name_en"];
                    names[Language.Finnish] = (string)item["name_fi"];
                    names[Language.Swedish] = (string)item["name_sv"];
                    organizations.Add(new Organization((string)item["org_id"], names));
                }
            }

            return organizations;
        }
    }
}
