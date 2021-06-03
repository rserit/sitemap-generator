using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SitemapGenerator
{
    public class XmlCreator
    {
        protected static List<RouteModel> Sitemaps = new List<RouteModel>();
        public static bool CreateSitemap(List<RouteModel> Pages, string Domain)
        {
            try
            {
                int Skip = 0, Take = 1000, Current = 0;
                int All = Pages.Count();
                int Loop = All / 1000;
                if (All % 1000 > 0) Loop++;
                if (All < 1000) Loop = 1;

                for (int i = 0; i < Loop; i++)
                {
                    // If you have not alternate URLs, you can use "CreateSimpleSitemap" method
                    CreateAdvancedSitemap(Pages.Skip(Skip).Take(Take).ToList(), "sitemap" + (i + 2), Domain);
                    Skip += 1000;

                    Current += (i + 2);
                    System.Console.WriteLine("Sitemap" + Current + ".xml created");
                }

                if (Sitemaps.Count > 1)
                {
                    CreateSitemapIndex(Sitemaps, "sitemap", Domain);
                    Console.WriteLine();
                }

                System.Console.WriteLine("All done!");
                return true;
            }
            catch (Exception Ex)
            {
                System.Console.WriteLine(Ex.Message);
                return false;
            }
        }
        /// <summary>
        /// If there are too many pages (may be thousands) multiple sitemap files are created and index mechanism is required for these files
        /// </summary>
        /// <param name="Routes">Route addresses</param>
        /// <param name="FileName">Sitemap file name</param>
        /// <param name="Domain">Current domain</param>
        static void CreateSitemapIndex(List<RouteModel> Routes, string FileName, string Domain)
        {
            try
            {
                // Site name should be written in its full form
                Routes.ForEach(x => x.Url = (!x.Url.Contains(Domain) ? Domain + x.Url : x.Url));
                FileName += ".xml";

                string Loc = AppDomain.CurrentDomain.BaseDirectory + "Sitemaps\\" + FileName;
                XDocument Doc = new XDocument(new XElement("body"));
                Doc.Save(Loc);
                XmlTextWriter Text = new XmlTextWriter(Loc, Encoding.UTF8);
                Text.WriteStartDocument();
                Text.WriteStartElement("sitemapindex");
                Text.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                foreach (var item in Routes)
                {
                    Text.WriteStartElement("sitemap");
                    Text.WriteElementString("loc", item.Url);
                    Text.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-mm-dd"));
                    Text.WriteEndElement();
                }

                Text.WriteEndDocument();
                Text.Flush();
                Text.Close();
                System.Console.WriteLine("Sitemap index created");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// If your page is not available in alternative languages, you can simply create a sitemap method as single language
        /// </summary>
        /// <param name="Routes">Route addresses</param>
        /// <param name="FileName">Sitemap</param>
        /// <param name="Domain">Current domain</param>
        static void CreateSimpleSitemap(List<RouteModel> Routes,string FileName,string Domain)
        {
            try
            {
                Routes.ForEach(x => x.Url = (!x.Url.Contains(Domain) ? Domain + x.Url : x.Url));
                FileName += ".xml";

                string Loc = AppDomain.CurrentDomain.BaseDirectory + "Sitemaps\\" + FileName;
                XDocument Doc = new XDocument(new XElement("body"));
                Doc.Save(Loc);
                XmlTextWriter Text = new XmlTextWriter(Loc, Encoding.UTF8);
                Text.WriteStartDocument();

                Text.WriteStartElement("urlset");
                Text.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                Text.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                Text.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");

                foreach (var i in Routes)
                {
                    Text.WriteStartElement("url"); // Url tag starting
                    Text.WriteElementString("loc", i.Url); // Current page
                    Text.WriteElementString("changefreq", "daily");
                    Text.WriteElementString("priority", "0.5"); // See more: https://www.sitemaps.org/protocol.html
                    Text.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-mm-dd"));
                }

                Text.WriteEndDocument();
                Text.Flush();
                Text.Close();

                Sitemaps.Add(new RouteModel() { Url = Domain + FileName });

            }
            catch (Exception Ex)
            {
                System.Console.WriteLine(Ex.Message);
            }
        }
        /// <summary>
        /// If there are alternative languages for your page, this method that allows you to define it in sitemap
        /// </summary>
        /// <see cref="https://developers.google.com/search/docs/advanced/crawling/localized-versions"/>
        /// <param name="Routes">Route addresses</param>
        /// <param name="FileName">Sitemap name</param>
        /// <param name="Domain">Current domain</param>
        static void CreateAdvancedSitemap(List<RouteModel> Routes, string FileName, string Domain)
        {
            try
            {
                Routes.ForEach(x => x.Url = (!x.Url.Contains(Domain) ? Domain + x.Url : x.Url)); 
                FileName += ".xml";

                string Loc = AppDomain.CurrentDomain.BaseDirectory + "Sitemaps\\" + FileName;
                XDocument Doc = new XDocument(new XElement("body"));
                Doc.Save(Loc);
                XmlTextWriter Text = new XmlTextWriter(Loc, Encoding.UTF8);
                Text.WriteStartDocument();

                Text.WriteStartElement("urlset");
                Text.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                Text.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                Text.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");
                Text.WriteAttributeString("xmlns:xhtml", "http://www.w3.org/1999/xhtml");

                foreach (var i in Routes)
                {
                    Text.WriteStartElement("url"); // Url tag starting
                    Text.WriteElementString("loc", i.Url); // Current page
                    Text.WriteElementString("changefreq", "daily"); 
                    Text.WriteElementString("priority", "0.5"); // See more: https://www.sitemaps.org/protocol.html
                    Text.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-mm-dd"));

                    foreach (var al in i.Alternates)
                    {
                        // Text.WriteElementString("xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"" + De_Alternate_Url + "\"", "");
                        Text.WriteElementString("xhtml:link rel=\"alternate\" hreflang=\"" + al.Key + "\" href=\"" + al.Value + "\"", "");
                    }
                    Text.WriteEndElement();
                }

                Text.WriteEndDocument();
                Text.Flush();
                Text.Close();

                Sitemaps.Add(new RouteModel() { Url = Domain + FileName });
            }
            catch (Exception Ex)
            {
                System.Console.WriteLine(Ex.Message);
            }
        }
    }
}
