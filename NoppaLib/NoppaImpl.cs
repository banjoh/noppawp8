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
    public delegate void TimeoutHandler(object sender, EventArgs e);
    public delegate void NoConnectionHandler(object sender, EventArgs e);

    public class NoppaImpl
    {
        private static readonly int _timeout = 35000; /* 5secs */
        private static Lazy<NoppaImpl> _instance = new Lazy<NoppaImpl>( () => new NoppaImpl() );

        public event NoConnectionHandler NoInternetConnection;
        public event TimeoutHandler TimeoutOccurred;
        
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

                    if (TimeoutOccurred != null)
                        TimeoutOccurred(this, EventArgs.Empty);

                    return null;
                }
            }
            catch (WebException webExc)
            {
                /* Caught exception */

                if (webExc.Status == WebExceptionStatus.ConnectFailure)
                {
                    System.Diagnostics.Debug.WriteLine("NoppaApiClient: Connection failed");

                    if (NoInternetConnection != null)
                        NoInternetConnection(this, EventArgs.Empty);
                }
                else if (webExc.Status == WebExceptionStatus.MessageLengthLimitExceeded)
                {
                    System.Diagnostics.Debug.WriteLine("NoppaApiClient: Too large response received");
                }
                else if (webExc.Status == WebExceptionStatus.SendFailure)
                {
                    System.Diagnostics.Debug.WriteLine("NoppaApiClient: Could not send request");
                }
                else if (webExc.Status == WebExceptionStatus.RequestCanceled ||
                         webExc.Status == WebExceptionStatus.UnknownError)
                {
                    System.Diagnostics.Debug.WriteLine("NoppaApiClient: Unknown connection error");
                }
                else
                {
                    /* Other errors seem to be not supported on WP8. However, if new SDK
                     * or something happens, at least log what error happened. */
                    System.Diagnostics.Debug.WriteLine("NoppaApiClient: Unknown error: {0}", webExc.Message);
                }

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
