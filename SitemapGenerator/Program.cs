using System;

namespace SitemapGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var _getData = DataHelper.GetCrawls("https://www.tasomarket.com").Result;
            XmlCreator.CreateSitemap(_getData, "https://www.tasomarket.com");
            Console.ReadKey();
        }
    }
}
