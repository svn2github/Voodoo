using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Xml;

namespace Voodoo.other.SEO
{
    public class RssItem
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public DateTime PutTime { get; set; }
    }
    public class Rss
    {
        public static void GetRss(List<RssItem> items, string Title, string Link, string Description, string Copyright)
        {

            // Clear any previous output from the buffer
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/xml";
            XmlTextWriter feedWriter
              = new XmlTextWriter(HttpContext.Current.Response.OutputStream, Encoding.UTF8);

            feedWriter.WriteStartDocument();

            // These are RSS Tags
            feedWriter.WriteStartElement("rss");
            feedWriter.WriteAttributeString("version", "2.0");

            feedWriter.WriteStartElement("channel");
            feedWriter.WriteElementString("title", Title);
            feedWriter.WriteElementString("link", Link);
            feedWriter.WriteElementString("description", Description);
            feedWriter.WriteElementString("copyright", Copyright);

            // Get list of 20 most recent posts

            // Write all Posts in the rss feed
            foreach (var item in items)
            {
                feedWriter.WriteStartElement("item");
                feedWriter.WriteElementString("title", item.Title);
                feedWriter.WriteElementString("description", item.Description);
                feedWriter.WriteElementString("link", item.Link);
                feedWriter.WriteElementString("pubDate", item.PutTime.ToString());
                feedWriter.WriteEndElement();
            }

            // Close all open tags tags
            feedWriter.WriteEndElement();
            feedWriter.WriteEndElement();
            feedWriter.WriteEndDocument();
            feedWriter.Flush();
            feedWriter.Close();

            HttpContext.Current.Response.End();
        }
    }
}
