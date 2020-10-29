using System;

namespace Business
{
    public class BotList
    {
        public string[] Value { get; set; }

        private BotList(string[] _value)
        {
            Value = _value;
        }

        public static BotList freewebcart
        {
            get
            {
                return new BotList(new string[] { "https://www.freewebcart.com/", "//a[contains(@class, 'entry-title-link')]" });
            }
        }

        public static BotList discount
        {
            get
            {
                return new BotList(new string[] { "https://www.real.discount/", "//a[contains(@class, 'product-loop-title')]" });
            }
        }

        public static BotList tutsnode
        {
            get
            {
                return new BotList(new string[] { "https://tutsnode.net/", "//div[contains(@class, 'title')]/h3/a" });
            }
        }

        public static BotList tutorialbar
        {
            get
            {
                return new BotList(new string[] { "https://www.tutorialbar.com/all-courses/", "//div[contains(@class, 'content_constructor')]/h3/a" });
            }
        }
    }
}
