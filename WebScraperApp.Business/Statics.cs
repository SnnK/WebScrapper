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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
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

            if (html != null)
            {
                foreach (HtmlNode data in html)
                {
                    links.Add(data.Attributes["href"].Value.Trim());
                }
            }

            return links;
        }

        public static List<Links> FindUdemies(List<string> links, string searchStr = "udemy.com/")
        {
            List<Links> ulinks = new List<Links>();

            if (links.Count() > 0)
            {
                foreach (var item in links)
                {
                    string htmlDocDetail = GetWebSource(item);
                    List<string> linkdtl = FindHrefs(htmlDocDetail);

                    if (linkdtl.Any())
                    {
                        foreach (string linkditem in linkdtl)
                        {
                            if (linkditem.Contains(searchStr))
                            {
                                ulinks.Add(new Links { Udemyurl = linkditem });
                            }
                        }
                    }
                }
            }

            return ulinks;
        }
        #endregion

        #region extension_methods
        public static string DecodeHtmlEntities(this object s)
        {
            StringWriter writer = new StringWriter();
            HttpUtility.HtmlDecode(s.ToString(), writer);

            return writer.ToString();
        }
        #endregion
    }
}