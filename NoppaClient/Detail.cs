﻿using System;
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
                html = Regex.Replace(html,
                                     "(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)|\\<.+?\\>",
                                     string.Empty,
                                     RegexOptions.Singleline | RegexOptions.IgnoreCase
                ).Replace("\n", " ");
                return System.Net.HttpUtility.HtmlDecode(html).Trim();
            }
            else
                return String.Empty;
        }
    }
}
