using System.Collections.Generic;

namespace SitemapGenerator
{
    public class RouteModel
    {
        public string Url { get; set; }
        public Dictionary<string, string> Alternates { get; set; }
    }
}
