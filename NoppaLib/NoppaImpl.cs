using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NoppaLib.DataModel;

namespace NoppaLib
{
    public class NoppaImpl
    {
        private static readonly int _timeout = 35000; /* 5secs */
        private static Lazy<NoppaImpl> _instance = new Lazy<NoppaImpl>( () => new NoppaImpl() );
        
        public static NoppaImpl GetInstance()
        {
            return _instance.Value;
        }

        #region API Call methods

        private Task<HttpWebResponse> CallAPIAsync(string query)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(query);
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

        public async Task<T> GetObject<T>(Cache.PolicyLevel policy, string format, params object[] args) where T : class
        {
            HttpWebResponse response;

            string query = APIConfigHolder.Url + String.Format(format, args);

            // Check to see if the cache contains the requested query.
            if (Cache.Exists(query) && policy != Cache.PolicyLevel.Reload)
            {
                return JsonConvert.DeserializeObject<T>(Cache.Get(query));
            }

            var timeoutCancel = new CancellationTokenSource();

            try
            {
                Task<HttpWebResponse> responseTask = CallAPIAsync(query);

                /* Handle the timeout */
                var timeoutTask = Task.Delay(_timeout, timeoutCancel.Token);
                var completeTask = await Task.WhenAny(responseTask, timeoutTask);
                if (completeTask == responseTask)
                {
                    timeoutCancel.Cancel();
                    response = await responseTask.ConfigureAwait(false);
                }
                else
                {
                    /* Timeout */
                    await timeoutTask;
                    System.Diagnostics.Debug.WriteLine("NoppaApiClient: Timed out ({0} ms)", _timeout);
                    return null;
                }
            }
            catch (WebException webExc)
            {
                /* Caught exception */
                System.Diagnostics.Debug.WriteLine("NoppaApiClient: Caught exception: {0}", webExc.Message);
                timeoutCancel.Cancel();
                return null;
            }

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                string json = await sr.ReadToEndAsync();
                Cache.Add(query, json, policy);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        #endregion
    }
}
