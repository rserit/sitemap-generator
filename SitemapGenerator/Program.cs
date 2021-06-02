using System;

namespace SitemapGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var _getData = DataHelper.GetRoutes();
            XmlCreator.CreateSitemap(_getData, "https://www.mozilla.org");
            Console.ReadKey();
        }
    }
}
