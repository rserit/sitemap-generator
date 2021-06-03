using Abot2.Crawler;
using Abot2.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SitemapGenerator
{
    public class DataHelper
    {
        static List<string> Urls = new List<string>();
        /// <summary>
        /// All URLs you have are to be brought from the DB, not crawled over the domain. There are also packages for crawl. You need to have a dataset anyway
        /// </summary>
        /// <returnsPages</returns>
        public static List<RouteModel> GetRoutes()
        {
            var Links = new Dictionary<string, string>();
            Links.Add("de", "https://www.mozilla.org/de/firefox/new");
            Links.Add("fr", "https://www.mozilla.org/fr/firefox/new");
            Links.Add("it", "https://www.mozilla.org/it/firefox/new");

            return new List<RouteModel>() { new RouteModel() { Url = "https://www.mozilla.org/tr/firefox/new", Alternates = Links } };
        }

        public static async Task<List<RouteModel>> GetCrawls(string Domain)
        {
            try
            {
                Uri Url = new Uri(Domain);
                IWebCrawler Crawler = new PoliteWebCrawler();
                Crawler.PageCrawlCompleted += Crawler_ProcessPageCrawlCompleted;
                await Crawler.CrawlAsync(Url);

                if (Urls.Count is 0)
                {
                    return default;
                }

                List<RouteModel> Routes = new List<RouteModel>();
                Urls.ForEach(x=> Routes.Add(new RouteModel() { Url = x }));
                return Routes;
            }
            catch (System.Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return default;
            }
        }

        static void Crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage Crawled = e.CrawledPage;

            if (Crawled.HttpResponseMessage != null && Crawled.HttpResponseMessage.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Page failed {0}", Crawled.Uri.AbsoluteUri);
            else Urls.Add(Crawled.Uri.AbsoluteUri);
            Console.WriteLine("Page succeeded {0}", Crawled.Uri.AbsoluteUri);

            if (string.IsNullOrEmpty(Crawled.Content.Text))
                Console.WriteLine("No content {0}", Crawled.Uri.AbsoluteUri);
        }
    }
}
