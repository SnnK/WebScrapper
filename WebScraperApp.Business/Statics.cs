using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using WebScraperApp.Models;

namespace WebScraperApp.Business
{
    public static class Statics
    {
        #region static_methods
        public static string GetWebSource(string uri)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }

            }
            catch { return ""; }
        }

        public static List<string> FindHrefs(string input)
        {
            var links = new List<string>();
            Regex regex = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase);
            Match match;
            for (match = regex.Match(input); match.Success; match = match.NextMatch())
            {
                if (match.Groups.Count > 1)
                {
                    links.Add(match.Groups[1].ToString());
                }
            }

            return links;
        }

        public static List<string> FindHtmlGrab(HtmlNodeCollection html)
        {
            var links = new List<string>();

            if (html == null) return links;

            links.AddRange(html.Select(data => data.Attributes["href"].Value.Trim()));

            return links;
        }

        public static List<Links> FindUdemies(List<string> links, string searchStr = "udemy.com/")
        {
            var ulinks = new List<Links>();

            if (!links.Any()) return ulinks;

            foreach (var item in links)
            {
                string htmlDocDetail = GetWebSource(item);
                List<string> linkdtl = FindHrefs(htmlDocDetail);

                if (!linkdtl.Any()) continue;

                foreach (string linkditem in linkdtl)
                {
                    if (linkditem.Contains(searchStr))
                    {
                        ulinks.Add(new Links { Udemyurl = linkditem });
                    }
                }
            }

            return ulinks;
        }
        #endregion

        #region extension_methods
        public static string DecodeHtmlEntities(this object s)
        {
            var writer = new StringWriter();
            HttpUtility.HtmlDecode(s.ToString(), writer);

            return writer.ToString();
        }
        #endregion
    }
}