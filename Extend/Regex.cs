using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Voodoo
{
    public static class myRegex
    {
        #region 在字符串中查找匹配的字符串
        /// <summary>
        /// 在字符串中查找匹配的字符串，如在“<div id="wea">北京:晴转多云 1-17℃</div>”中查找"晴转多云 1-17℃".
        /// </summary>
        /// <param name="str">要查找的字符串，如：“<div id="wea">北京:晴转多云 1-17℃</div>”</param>
        /// <param name="startString">要查找数据之前的字符串，如：（<div id="wea">北京:）</param>
        /// <param name="endString">要查找数据之后的字符串，如：（</div>）</param>
        /// <returns></returns>
        public static string FindText(this string str, string startString, string endString)
        {
            string regex = startString + "(?<key>[\\w\\W]*?)" + endString;
            Regex r = new Regex(regex, RegexOptions.None);
            Match mc = r.Match(str);
            if (mc.Success)
            {
                return mc.Groups["key"].Value;
            }
            else
            {
                return "";
            }
            
        }
        #endregion

        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern);
        }

        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }

        public static int GetNumberFromTitle(this string title)
        {
            Match mc = new Regex("第[一二三四五六七八九〇零十百千万1234567890]+章|引子", RegexOptions.None).Match(title);
            if (mc.Success)
            {
                string str_chineseNumber = mc.Groups[0].Value;
                str_chineseNumber = str_chineseNumber.Replace("第", "").Replace("章", "").Replace("引子", "0");

                int result = 0;
                Match m = new Regex("[一二三四五六七八九]?十|[一二三四五六七八九]+百|[一二三四五六七八九]+千|[一二三四五六七八九]+万|[一二三四五六七八九]+", RegexOptions.None).Match(str_chineseNumber);
                while (m.Success)
                {
                    result += m.Groups[0].Value.Replace("一", "1")
                        .Replace("二", "2")
                        .Replace("三", "3")
                        .Replace("四", "4")
                        .Replace("五", "5")
                        .Replace("六", "6")
                        .Replace("七", "7")
                        .Replace("八", "8")
                        .Replace("九", "9")
                        .Replace("十", "0")
                        .Replace("百", "00")
                        .Replace("千", "000")
                        .Replace("万", "0000")
                        .ToInt32()
                        ;
                    m = m.NextMatch();
                }
                return result;

            }
            else
            {
                return -1;
            }
        }
    }
}
