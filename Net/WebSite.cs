using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace Voodoo.Net
{
    public class WebSite
    {
        #region  获取Alexa排名
        /// <summary>
        /// 获取Alexa排名
        /// </summary>
        /// <param name="host">网站地址</param>
        /// <returns>排名序号</returns>
        public static int Alexa(string host)
        {
            string html = Net.Url.GetHtml("http://data.alexa.com/data/?cli=10&dat=snba&ver=7.0&url=" + host);
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(html);
                XmlNode node = xml.SelectSingleNode("/ALEXA/SD/POPULARITY");
                if (node != null && node.Attributes["TEXT"] != null)
                    return int.Parse(node.Attributes["TEXT"].Value);
            }
            catch
            {
                int a = html.IndexOf("RANK=\"");
                if (a > 0)
                {
                    int b = html.IndexOf("\"", a + 6);
                    if (b > 0)
                        return int.Parse(html.Substring(a + 6, b - a - 6));
                }
            }
            return 0;
        }
        #endregion

        /// <summary>
        /// 获取网站备案信息
        /// </summary>
        /// <param name="host">网站域名</param>
        /// <returns></returns>
        public static BeianInfo GetBeianInfo(string host)
        {
            BeianInfo beian = new BeianInfo();
            host = host.ToLower().Replace("http://", "").Replace("https://","").Replace("/","");
            string Html = Url.GetHtml("http://tool.chinaz.com/beian.aspx?s=" + host, "utf-8");

            Match match = new Regex("审核时间</td>(?<key>[\\s\\S]*?)</div>", RegexOptions.None).Match(Html);
            string res = match.Groups["key"].Value;

            Match m_beian = new Regex("<td align=\"center\">(?<cname>.*?)</td>[\\s\\S]*<td align=\"center\">(?<ctype>.*?)</td>[\\s\\S]*<td align=\"center\">(?<bnum>.*?)</td>[\\s\\S]*<td align=\"center\">(?<sitename>.*?)</td>[\\s\\S]*<td align=\"center\">(?<ctype>.*?)</td>[\\s\\S]*<td align=\"center\">(?<atime>.*?)</td>", RegexOptions.None).Match(res);
            if (m_beian.Success)
            {
                beian.AutitTime = m_beian.Groups["atime"].Value.ToDateTime();
                beian.BeianNumber = m_beian.Groups["bnum"].Value.TrimHTML();
                beian.CompanyName = m_beian.Groups["cname"].Value;
                beian.CompanyType = m_beian.Groups["ctype"].Value;
                beian.IndexUrl = m_beian.Groups["surl"].Value;
                beian.SiteName = m_beian.Groups["sitename"].Value;
                
            }

            return beian;

        }
    }
}
