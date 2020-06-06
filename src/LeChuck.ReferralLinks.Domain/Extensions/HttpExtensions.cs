using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper.Mappers;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Domain.Extensions
{
    public static class HttpExtensions
    {
        public static string ExpandUrl(this string shortUrl, ILogger logger = null)
        {
            string newurl = shortUrl;
            bool redirecting = true;

            while (redirecting)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(newurl);
                    request.AllowAutoRedirect = false;
                    request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3 (.NET CLR 4.0.20506)";
                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse) request.GetResponse();
                    }
                    catch (WebException e)
                    {
                        response = (HttpWebResponse)e.Response;
                    }
                    if ((int)response.StatusCode == 301 || (int)response.StatusCode == 302)
                    {
                        string uriString = response.Headers["Location"];
                        logger?.LogDebug("Redirecting " + newurl + " to " + uriString + " because " + response.StatusCode);
                        newurl = uriString;
                    }
                    else
                    {
                        logger?.LogDebug("Not redirecting " + shortUrl + " because " + response.StatusCode);
                        redirecting = false;
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("url", newurl);
                    logger?.LogDebug("Exception resolving short url",ex);
                    redirecting = false;
                    return shortUrl;
                }
            }
            return newurl;
        }

        public static bool IsShortUrl(this string url)
        {
            var uriBuilder = new UriBuilder(url);
            // path after the host 
            var path = uriBuilder.Path;
            // fragment of the uri
            var fragment = uriBuilder.Fragment;
            // Querystring
            var query = uriBuilder.Query;
            // parts of the path
            var pathParts = path.Split("/");

            if (!string.IsNullOrWhiteSpace(query) || !string.IsNullOrWhiteSpace(fragment))
                return false;

            if (pathParts.Length > 2) return false;

            if (path.Length > 11) return false;

            return true;
        }
    }
}
