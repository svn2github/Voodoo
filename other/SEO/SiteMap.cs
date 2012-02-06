using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo.other.SEO
{
    public class SiteMap
    {
        public List<PageInfo> url
        {
            get;
            set;
        }

        /// <summary>
        /// 生成SiteMap字符串
        /// </summary>
        /// <returns></returns>
        public string GenerateSiteMapString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?> ");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"> ");

            foreach (PageInfo pi in url)
            {
                sb.AppendLine("<url>");
                sb.AppendLine(string.Format("<loc>{0}</loc>",pi.loc));
                sb.AppendLine(string.Format("<lastmod>{0}</lastmod> ", pi.lastmod.ToString()));
                sb.AppendLine(string.Format("<changefreq>{0}</changefreq> ", pi.changefreq));
                sb.AppendLine(string.Format("<priority>{0}</priority> ",pi.priority));
                sb.AppendLine("</url>");
            }

            sb.AppendLine("</urlset>");
            return sb.ToString();
        }

        /// <summary>
        /// 保存Site文件
        /// </summary>
        /// <param name="FilePath">路径</param>
        public void SaveSiteMap(string FilePath)
        {
            Voodoo.IO.File.Write(FilePath, GenerateSiteMapString());
        }
    }

    public class PageInfo
    {
        /// <summary>
        /// 网址
        /// </summary>
        public string loc { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime lastmod { get; set; }

        /// <summary>
        /// 更新频繁程度
        /// </summary>
        public string changefreq{get;set;}

        /// <summary>
        /// 优先级，0-1
        /// </summary>
        public string priority { get; set; }
    }
}
