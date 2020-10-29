using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Web;
using Models;

namespace Business
{
    public class Datas
    {
        private const string ua = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";

        public void Collect(string target, string html)
        {
            Source(target, html);
        }

        private void Source(string source, string find)
        {
            HtmlWeb web = new HtmlWeb();
            web.UserAgent = ua;
            web.UseCookies = true;

            try
            {
                var htmlDoc = web.Load(source);
                var links = Statics.FindHtmlGrab(htmlDoc.DocumentNode.SelectNodes(find));
                var findu = Statics.FindUdemies(links);

                GetDetails(findu);
            }
            catch { throw; }
        }

        private void GetDetails(List<links> data)
        {
            foreach (var item in data)
            {
                if (item.udemyurl.Contains("udemy.com/"))
                {
                    string title = string.Empty;
                    int course_id = 0;
                    Uri myUri = new Uri(item.udemyurl);
                    string coupon_code = HttpUtility.ParseQueryString(myUri.Query).Get("couponcode");
            
                    HtmlWeb web = new HtmlWeb();
                    web.UserAgent = ua;
                    web.UseCookies = true;
                    var htmlDoc = web.Load(item.udemyurl);
            
                    var h1 = htmlDoc.DocumentNode.SelectSingleNode("//h1");
                    if (h1 != null)
                        title = h1.InnerText.Replace("\n", "").DecodeHtmlEntities();
            
                    var body = htmlDoc.DocumentNode.SelectSingleNode("//body");
                    if (body != null)
                        course_id = Convert.ToInt32(body.Attributes["data-clp-course-id"].Value);
            
                    string udemy_link = htmlDoc.DocumentNode.SelectSingleNode("//link[@rel='canonical']").Attributes["href"].Value;
            
                    if (!string.IsNullOrEmpty(title))
                    {
                        Udemy.findCourses.Add(new courses { coupon_code = coupon_code, title = title, udemy_link = udemy_link, course_id = course_id });
                        Console.WriteLine("Title: {0} | Link: {1} | Coupon Code: {2}", title, udemy_link, coupon_code);
                    }
                }
            }
        }
    }
}
