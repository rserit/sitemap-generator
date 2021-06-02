using System.Collections.Generic;
using System.Linq;

namespace SitemapGenerator
{
    public class DataHelper
    {
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
    }
}
