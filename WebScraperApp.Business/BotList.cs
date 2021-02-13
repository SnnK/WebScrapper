using WebScraperApp.Models;
using System.Collections.Generic;

namespace WebScraperApp.Business
{
    public class BotList
    {
        public static List<BotDetail> Bots()
        {
            return new List<BotDetail>
            {
                new BotDetail { Source = "freewebcart", Url = "https://www.freewebcart.com/", Node = "//a[contains(@class, 'entry-title-link')]" },
                new BotDetail { Source = "discount", Url = "https://www.real.discount/", Node = "//a[contains(@class, 'product-loop-title')]" },
                new BotDetail { Source = "tutsnode", Url = "https://tutsnode.net/", Node = "//div[contains(@class, 'title')]/h3/a" },
                new BotDetail { Source = "tutorialbar", Url = "https://www.tutorialbar.com/all-courses/", Node = "//div[contains(@class, 'content_constructor')]/h3/a" }
            };
        }
    }
}
