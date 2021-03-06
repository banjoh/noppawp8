﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NoppaLib
{
    // Class to hold helper functions required all over the application
    public class NoppaUtility
    {
        public static Uri MakeUri(string relativePath, params object[] parameters)
        {
            List<string> query = new List<string>();

            for (int i = 0; i + 1 < parameters.Length; i += 2)
            {
                var key = parameters[i] != null ? parameters[i].ToString() : "";
                var val = parameters[i + 1] != null ? parameters[i + 1].ToString() : "";
                query.Add(HttpUtility.UrlEncode(key) + '=' + HttpUtility.UrlEncode(val));
            }

            return new Uri(relativePath + (query.Count == 0 ? "" : "?" + String.Join("&", query)), UriKind.Relative);
        }
    }
}
