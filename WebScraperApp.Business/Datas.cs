using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Web;
using WebScraperApp.Models;

namespace WebScraperApp.Business
{
    public class Datas
    {
        private readonly HtmlWeb web;
        public readonly List<Courses> findCourses;

        public Datas()
        {
            web = new HtmlWeb
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36",
                UseCookies = true
            };

            findCourses = new List<Courses>();
        }

        public void Collect(string target, string html)
        {
            Source(target, html);
        }

        private void Source(string source, string find)
        {
            try
            {
                var htmlDoc = web.Load(source);
                var links = Statics.FindHtmlGrab(htmlDoc.DocumentNode.SelectNodes(find));
                var findu = Statics.FindUdemies(links);

                GetDetails(findu);
            }
            catch
            {
                // ignored
            }
        }

        private void GetDetails(List<Links> data)
        {
            foreach (var item in data)
            {
                if (!item.Udemyurl.Contains("udemy.com/")) continue;

                string title = string.Empty;
                Uri myUri = new Uri(item.Udemyurl);
                string coupon_code = HttpUtility.ParseQueryString(myUri.Query).Get("couponcode");

                var htmlDoc = web.Load(item.Udemyurl);

                var h1 = htmlDoc.DocumentNode.SelectSingleNode("//h1");
                if (h1 != null)
                    title = h1.InnerText.Replace("\n", "").DecodeHtmlEntities();

                string udemy_link = htmlDoc.DocumentNode.SelectSingleNode("//link[@rel='canonical']").Attributes["href"].Value;

                if (string.IsNullOrWhiteSpace(title)) continue;

                findCourses.Add(new Courses { Title = title, Udemy_link = udemy_link, Coupon_code = coupon_code });
                Console.WriteLine($"Title: {title} | Link: {udemy_link} | Coupon Code: {coupon_code}");
            }
        }
    }
}
