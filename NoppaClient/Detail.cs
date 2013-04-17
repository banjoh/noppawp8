using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace NoppaClient
{
    /**
     * Utility class where to collect useful methods, such as input validation
     * or input filtering.
     */
    public class Detail
    {
        public static string StripHtml(string html)
        {
            if (html != null)
            {
                html = Regex.Replace(html, "<.+?>", string.Empty).Replace("\n", " ");
                return System.Net.HttpUtility.HtmlDecode(html);
            }
            else
                return String.Empty;
        }
    }
}
