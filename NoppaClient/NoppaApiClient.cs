﻿using System;
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

        /**
         * This probably looks hairy. Call the API and create
         * a list of DataModel objects. Utility function for
         * certain kinds of calls.
         */
        private async Task<List<T>> GetList<T>(string query)
        {
            HttpWebResponse response = await CallAPIAsync(query);
            
            List<T> list = new List<T>();

            using (var sr = new StreamReader(response.GetResponseStream()))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var array = (JArray)JArray.ReadFrom(reader);
                foreach (JObject item in array)
                    list.Add((T)Activator.CreateInstance(typeof(T), item.ToString()));
            }
            return list;
        }

        public async Task<List<Organization>> GetAllOrganizations()
        {
            string query = String.Format("{0}/organizations?key={1}", _apiURL, _apiKey);
            
            return await GetList<Organization>(query);
           
        }

        public async Task<List<Department>> GetDepartments(Organization org)
        {
            string query = String.Format("{0}/departments?key={1}&org_id={2}", _apiURL, _apiKey, org.Id);

            return await GetList<Department>(query);
        }

        public async Task<List<Department>> GetDepartments()
        {
            string query = String.Format("{0}/departments?key={1}", _apiURL, _apiKey);

            return await GetList<Department>(query);
        }
    }
}
