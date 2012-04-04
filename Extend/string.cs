using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Collections.Specialized;

namespace Voodoo
{
    public static partial class @string
    {

        #region 字符串转换为Pascal格式
        /// <summary>
        /// 字符串转换为Pascal格式
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <returns>返回Pascal格式字符串</returns>
        /// <example>输入myString,返回MyString这种字符串</example>
        public static string ToPascal(this string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }
        #endregion

        #region 字符串转换为camel格式
        /// <summary>
        /// 字符串转换为camel格式
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <returns></returns>
        public static string ToCamel(this string s)
        {
            return s.Substring(0, 1).ToLower() + s.Substring(1);
        }
        #endregion

        #region 字符串转换为数字
        /// <summary>
        /// 字符串转换为 Int32格式
        /// </summary>
        /// <param name="self"></param>
        /// <returns>int类型字符串，出错返回int.MinValue</returns>
        public static int ToInt32(this object self)
        {
            try
            {
                return Convert.ToInt32(self);
            }
            catch
            {
                return int.MinValue;
            }
        }

        public static int ToInt32(this object self, int defaultvalue)
        {
            try
            {
                return Convert.ToInt32(self);
            }
            catch
            {
                return defaultvalue;
            }
        }
        /// <summary>
        /// 字符串转换为 Int64格式
        /// </summary>
        /// <param name="self"></param>
        /// <returns>long类型字符串，出错返回int.MinValue</returns>
        public static long ToInt64(this object self)
        {
            try
            {
                return Convert.ToInt64(self);
            }
            catch
            {
                return int.MinValue;
            }
        }
        #endregion

        #region IP地址转换为秘密的IP地址
        /// <summary>
        /// IP地址转换为秘密的IP地址
        /// </summary>
        /// <param name="ipAddress">如：202.195.224.100</param>
        /// <returns>返回202.195.224.*类型的字符串</returns>
        public static string ipSecret(this string ipAddress)
        {
            string[] ips = ipAddress.Split('.');
            StringBuilder sb_screcedIp = new StringBuilder();
            int ipsLength = ips.Length;
            for (int i = 0; i < ipsLength - 1; i++)
            {
                sb_screcedIp.Append(ips[i].ToString() + ".");

            }
            sb_screcedIp.Append("*");
            return sb_screcedIp.ToString();
        }
        #endregion

        #region 判断字符串是否在字符串数组
        /// <summary>
        /// 判断字符串是否在字符串数组中
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <param name="targrt">目标数组</param>
        /// <returns></returns>
        public static bool IsInArray(this string str, string[] targrt)
        {
            return targrt.Contains(str);
            //for (int i = 0; i < targrt.Length; i++)
            //{
            //    if (str == targrt[i])
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
        #endregion

        #region 字符串数组转换为数字数组
        /// <summary>
        /// 字符串数组转换为数字数组
        /// </summary>
        /// <param name="StringArray"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string[] StringArray)
        {
            int[] returnValue = new int[StringArray.Length];
            for (int i = 0; i < StringArray.Length; i++)
            {
                returnValue[i] = Convert.ToInt32(StringArray[i]);
            }
            return returnValue;

        }
        #endregion

        #region 去除HTML标记
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="html">要除去HTML标记的文本</param>
        /// <returns></returns>
        public static string TrimHTML(this string html)
        {
            string StrNohtml = html;
            StrNohtml = StrNohtml.TrimScript();
            StrNohtml = Regex.Replace(StrNohtml, "&.{1,6};", "", RegexOptions.IgnoreCase);
            StrNohtml = Regex.Replace(StrNohtml, "<p.*?>", Environment.NewLine, RegexOptions.IgnoreCase);
            StrNohtml = Regex.Replace(StrNohtml, "<br.*?>", Environment.NewLine, RegexOptions.IgnoreCase);
            StrNohtml = Regex.Replace(StrNohtml, "<script[\\w\\W].*?</script>", "", RegexOptions.IgnoreCase);
            StrNohtml = System.Text.RegularExpressions.Regex.Replace(StrNohtml, "<[^>]+>", "");
            StrNohtml = System.Text.RegularExpressions.Regex.Replace(StrNohtml, "&[^;]+;", "");
            return StrNohtml;

        }


        public static string TrimScript(this string html)
        {
            html = Regex.Replace(html, "<script[\\w\\W]*?</script>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<style[\\w\\W]*?</style>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<iframe[\\w\\W]*?</iframe>", "", RegexOptions.IgnoreCase);
            return html;
        }
        #endregion

        #region base64url编码处理

        public static string ToEnBase64(this string str)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str));
        }

        public static string ToUnBase64(this string str)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }

        #endregion

        #region 去除换行符
        /// <summary>
        /// 去除换行符
        /// </summary>
        /// <param name="str">要进行处理的字符串</param>
        /// <returns></returns>
        public static string TrimBR(this string str)
        {
            str = str.Replace("\n", "");
            str = str.Replace("\r", "");
            str = str.Replace("\t", "");
            return str;
        }
        #endregion

        #region 删除字符串尾部的回车/换行/空格
        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(this string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }
        #endregion

        #region SQL字符的剔除
        /// <summary>
        /// SQL字符的剔除
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToSqlEnCode(this string self)
        {
            return self.Replace("'", "''");
        }
        #endregion

        #region 生成以日期时间为基础的随机字符串
        /// <summary>
        /// 生成以日期时间为基础的随机字符串
        /// </summary>
        /// <returns></returns>
        public static string Getfilename()
        {
            Random number = new Random();
            //下面的number.Next(10000,99999).ToString()加入一个5位的在10000到99999之间的随机数
            //yyyyMMdd代码年月日。hhmmss代表小时分钟秒钟 。fff代表毫秒

            //暂时使用了GUID的那个文件名filenameGUID
            return DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + number.Next(10000, 99999).ToString();

        }
        #endregion

        #region 返回GUID
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        #region 剔除脚本注入代码
        /// <summary>
        /// 剔除脚本注入代码
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string TrimDbDangerousChar(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return "";
            }

            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            self = self.Replace("'", "''");
            self = regex1.Replace(self, ""); //过滤<script></script>标记 
            self = regex2.Replace(self, ""); //过滤href=javascript: (<A>) 属性 
            self = regex3.Replace(self, " _disibledevent="); //过滤其它控件的on...事件 
            self = regex4.Replace(self, ""); //过滤iframe 
            self = regex5.Replace(self, ""); //过滤frameset
            return self;
        }
        #endregion

        #region  验证字符串是否为空字符串
        /// <summary>
        /// 验证字符串是否为空字符串
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string self)
        {
            if (self == null || self.Length == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 变量转换为Byte,不成功则转换为0
        /// <summary>
        /// 变量转换为Byte,不成功则转换为0
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Byte ToByte(this object self)
        {
            try
            {
                return Convert.ToByte(self);
            }
            catch
            {
                return Convert.ToByte("0");
            }
        }
        #endregion

        #region 类型转换为DateTime
        /// <summary>
        /// 类型转换为DateTime
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object self)
        {
            return self.ToDateTime(new DateTime(2000, 1, 1));
        }

        public static DateTime ToDateTime(this object self, DateTime Default)
        {
            try
            {
                return Convert.ToDateTime(self.ToString().Replace("年", "-").Replace("月", "-").Replace("日", ""));
            }
            catch
            {
                return Default;
            }
        }
        #endregion

        #region 类型转换为Decimal
        /// <summary>
        /// 类型转换为Decimal
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this object self)
        {
            return ToDecimal(self, Decimal.MinValue);
        }

        /// <summary>
        /// 转换为Decimal类型 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="DefaultValue">转换失败的默认值</param>
        /// <returns></returns>
        public static Decimal ToDecimal(this object self, Decimal DefaultValue)
        {
            try
            {
                return Convert.ToDecimal(self.ToString());
            }
            catch
            {
                return DefaultValue;
            }
        }
        #endregion

        #region 转换为bool类型
        /// <summary>
        /// 转换为bool类型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object self)
        {
            if (self == null||self.ToS().IsNullOrEmpty())
            {
                return false;
            }

            if (self.ToString().ToLower() == "false")
            {
                return false;
            }
            if (self.ToString().ToLower() == "true")
            {
                return true;
            }
            if (self.ToString().ToInt32() == 0)
            {
                return false;
            }
            if (self.ToString().ToInt32() != 0)
            {
                return true;

            }
            try
            {
                return Convert.ToBoolean(self.ToString());
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region StringBuilder去除尾部符号
        /// <summary>
        /// StringBuilder去除尾部符号
        /// </summary>
        /// <param name="self"></param>
        /// <param name="c">要去除的符号</param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder self, char c)
        {
            string s = self.ToString();
            s = s.TrimEnd(c);
            StringBuilder sb = new StringBuilder();
            sb.Append(s);
            return sb;
        }
        #endregion

        #region 转换为char[]数组
        /// <summary>
        /// 转换为char[]数组
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static char[] ToCharArray(this string self)
        {
            int l = self.Length;
            char[] myCharArray = new char[l];
            for (int i = 0; i < l; i++)
            {
                myCharArray[i] = Convert.ToChar(self[i]);
            }
            return myCharArray;
        }
        #endregion

        #region 字符串转换为二进制数组 流 byte[]
        /// <summary>
        /// 字符串转换为二进制数组 流 byte[]
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>返回一个二进制数据流</returns>
        public static byte[] ToByteArray(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="Encode">编码格式</param>
        /// <returns>返回一个二进制数据流</returns>
        public static byte[] ToByteArray(this string str,Encoding Encode)
        {
            return Encode.GetBytes(str);
        }
        #endregion

        #region 根据设定长度截取字符串
        /// <summary>
        /// 根据设定长度截取字符串
        /// </summary>
        /// <param name="self"></param>
        /// <param name="charCount">保留字符的长度</param>
        /// <returns></returns>
        public static string CutString(this string self, int charCount)
        {
            if (self.Length > charCount)
            {
                return self.Substring(0, charCount) + "...";
            }
            else
            {
                return self;
            }
        }
        #endregion

        #region 返回随机字符串guid
        /// <summary>
        /// 返回随机字符串guid
        /// </summary>
        public static string guid
        {
            get
            {
                Guid g = new Guid();
                return g.ToString();
            }
        }
        #endregion

        #region 转换成double
        /// <summary>
        /// 转换成double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            try
            {
                return Convert.ToDouble(str.Trim());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region 20100526类型的时间转换成 2010年05月26日这种格式
        /// <summary>
        /// 20100526类型的时间转换成 2010年05月26日这种格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDateString(this string str)
        {
            if (str.IsInt() && str.Length == 8)
            {
                str = str.Insert(4, "年");
                str = str.Insert(7, "月");
                str += "日";
                return str;

            }
            else
            {
                return str;
            }
        }
        #endregion

        #region 生成随机的订单号
        /// <summary>
        /// 生成随机的订单号
        /// </summary>
        /// <returns></returns>
        public static string BuidDailNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        #endregion

        #region HTML代码转UBB
        /// <summary>
        /// HTML代码转UBB
        /// </summary>
        /// <param name="_Html"></param>
        /// <returns></returns>
        public static string HtmlToUbb(this string _Html)
        {
            _Html = Regex.Replace(_Html, "<br[^>]*>", "\r\n");
            _Html = Regex.Replace(_Html, @"<p.*>", "\r\n");
            _Html = Regex.Replace(_Html, "\\son[\\w]{3,16}\\s?=\\s*(['\"]).+?\\1", "");
            _Html = Regex.Replace(_Html, "<hr[^>]*>", "[hr]");
            _Html = Regex.Replace(_Html, @"<(\/)?blockquote([^>]*)>", "[$1blockquote]");
            _Html = Regex.Replace(_Html, "<img[^>]*smile=\"(\\d+)\"[^>]*>", "'[s:$1]");
            _Html = Regex.Replace(_Html, "<img[^>]*src=['\"\\s]*([^\\s'\"]+)[^>]*>", "[img]$1[/img]");
            _Html = Regex.Replace(_Html, "<a[^>]*href=['\"\\s]*([^\\s'\"]*)[^>]*>(.+?)<\\/a>", "[url=$1]$2[/url]");
            _Html = Regex.Replace(_Html, "<[^>]*?>", "");
            _Html = Regex.Replace(_Html, "&amp;", "&");
            _Html = Regex.Replace(_Html, "&nbsp;", " ");
            _Html = Regex.Replace(_Html, "&lt;", "<");
            _Html = Regex.Replace(_Html, "&gt;", ">");
            _Html = Regex.Replace(_Html, "<p>", "\r\n");
            _Html = Regex.Replace(_Html, "</p>", "\r\n");
            _Html = Regex.Replace(_Html, "<br .{0,2}>", "\r\n");
            _Html = Regex.Replace(_Html, "[\\r\\n]{3,}", "\r\n\r\n");
            _Html = Regex.Replace(_Html, "&mdash;", "-");
            _Html = Regex.Replace(_Html, "&hellip;", "。");
            //hellip;
            _Html = Regex.Replace(_Html, "&[\\w]{2,6};", " ");
            _Html = Regex.Replace(_Html, "[\\s\\n]{3,100}", "\r\n\r\n");
            return _Html;
        }
        #endregion


        #region UBB解码
        /// <summary>
        /// UBB解码
        /// </summary>
        /// <param name="chr"></param>
        /// <returns>返回解码后的HTML</returns>
        public static string UBBCode(this string chr)
        {
            if (chr == null)
                return "";
            chr = chr.Replace("<", "&lt");
            chr = chr.Replace(">", "&gt");
            chr = chr.Replace("\n", "<br>");
            chr = Regex.Replace(chr, @"<script(?<x>[^\>]*)>(?<y>[^\>]*)            \</script\>", @"&lt;script$1&gt;$2&lt;/script&gt;", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[url=(?<x>[^\]]*)\](?<y>[^\]]*)\[/url\]", @"<a href=$1  target=_blank>$2</a>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[url\](?<x>[^\]]*)\[/url\]", @"<a href=$1 target=_blank>$1</a>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[email=(?<x>[^\]]*)\](?<y>[^\]]*)\[/email\]", @"<a href=$1>$2</a>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[email\](?<x>[^\]]*)\[/email\]", @"<a href=$1>$1</a>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[flash](?<x>[^\]]*)\[/flash]", @"<OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=4,0,2,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=500 height=400><PARAM NAME=movie VALUE=""$1""><PARAM NAME=quality VALUE=high><embed src=""$1"" quality=high pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash' width=500 height=400>$1</embed></OBJECT>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[img](?<x>[^\]]*)\[/img]", @"<IMG SRC=""$1"" border=0>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[color=(?<x>[^\]]*)\](?<y>[^\]]*)\[/color\]", @"<font color=$1>$2</font>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[face=(?<x>[^\]]*)\](?<y>[^\]]*)\[/face\]", @"<font face=$1>$2</font>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[size=1\](?<x>[^\]]*)\[/size\]", @"<font size=1>$1</font>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[size=2\](?<x>[^\]]*)\[/size\]", @"<font size=2>$1</font>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[size=3\](?<x>[^\]]*)\[/size\]", @"<font size=3>$1</font>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[size=4\](?<x>[^\]]*)\[/size\]", @"<font size=4>$1</font>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[align=(?<x>[^\]]*)\](?<y>[^\]]*)\[/align\]", @"<align=$1>$2</align>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[fly](?<x>[^\]]*)\[/fly]", @"<marquee width=90% behavior=alternate scrollamount=3>$1</marquee>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[move](?<x>[^\]]*)\[/move]", @"<marquee scrollamount=3>$1</marquee>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[glow=(?<x>[^\]]*),(?<y>[^\]]*),(?<z>[^\]]*)\](?<w>[^\]]*)\[/glow\]", @"<table width=$1 style='filter:glow(color=$2, strength=$3)'>$4</table>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[shadow=(?<x>[^\]]*),(?<y>[^\]]*),(?<z>[^\]]*)\](?<w>[^\]]*)\[/shadow\]", @"<table width=$1 style='filter:shadow(color=$2, strength=$3)'>$4</table>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[b\](?<x>[^\]]*)\[/b\]", @"<b>$1</b>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[i\](?<x>[^\]]*)\[/i\]", @"<i>$1</i>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[u\](?<x>[^\]]*)\[/u\]", @"<u>$1</u>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[code\](?<x>[^\]]*)\[/code\]", @"<pre id=code><font size=1 face='Verdana, Arial' id=code>$1</font id=code></pre id=code>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[list\](?<x>[^\]]*)\[/list\]", @"<ul>$1</ul>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[list=1\](?<x>[^\]]*)\[/list\]", @"<ol type=1>$1</ol id=1>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[list=a\](?<x>[^\]]*)\[/list\]", @"<ol type=a>$1</ol id=a>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[\*\](?<x>[^\]]*)\[/\*\]", @"<li>$1</li>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[quote](?<x>.*)\[/quote]", @"<center>—— 以下是引用 ——<table border='1' width='80%' cellpadding='10' cellspacing='0' ><tr><td>$1</td></tr></table></center>", RegexOptions.IgnoreCase);

            return (chr);

        }
        #endregion

        #region IP转换为数字
        /// <summary>
        /// IP转换为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long IpToLong(this string str)
        {
            return Voodoo.Security.Request.IpToLong(str);
        }
        #endregion

        #region 清除UBB标签
        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="sDetail">帖子内容</param>
        /// <returns>帖子内容</returns>
        public static string ClearUBB(this string sDetail)
        {
            return Regex.Replace(sDetail, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 判断是否为base64字符串
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(this string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(this string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        #endregion



        #region 从HTML中获取文本,保留br,p,img
        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="HTML">html代码</param>
        /// <returns></returns>
        public static string GetTextFromHTML(this string HTML)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(HTML, "");
        }
        #endregion

        #region 删除最后一个字符
        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str">要操作的字符串</param>
        /// <returns></returns>
        public static string ClearLastChar(this string str)
        {
            return (str == "") ? "" : str.Substring(0, str.Length - 1);
        }
        #endregion

        #region 过滤 Json字符
        /// <summary>
        /// 过滤 Json字符
        /// </summary>
        /// <param name="sourceStr">要转换的字符串</param>
        /// <returns></returns>
        public static string FilterJsonChar(this string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "\\\b");
            sourceStr = sourceStr.Replace("\t", "\\\t");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\f", "\\\f");
            sourceStr = sourceStr.Replace("\r", "\\\r");
            return sourceStr.Replace("\"", "\\\"");
        }
        #endregion

        #region 普通字符串转换为JSON字符串
        /// <summary>
        /// 普通字符串转换为JSON字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToJson(this string str)
        {
            return str.StringToJson("data");
        }

        /// <summary>
        /// 普通字符串转换为JSON字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string StringToJson(this string str, string key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(WS.RequestString("jsoncallback"));
            sb.Append("(");
            sb.Append("{\"" + key + "\":\"");
            sb.Append(str);
            sb.Append("\"}");
            sb.Append(")");
            return sb.ToString();
        }
        #endregion

        #region 拉丁字符转换为utf8

        /// <summary>
        /// 拉丁字符转换为utf8
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string LatinToUtf8(this string str)
        {
            try
            {
                byte[] bytesStr = Encoding.GetEncoding("latin1").GetBytes(str);
                return Encoding.GetEncoding("GB2312").GetString(bytesStr);
            }
            catch
            {
                return str;
            }
        }
        #endregion

        #region Sql中的top 语句转换为limit语句，以适应Mysql数据库适用
        /// <summary>
        /// Sql中的top 语句转换为limit语句，以适应Mysql数据库适用
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SqlStrTopToLimit(this string str)
        {
            if (str.IndexOf("top") > -1)
            {
                string[] arraystring = str.Split(new string[] { "top", "from" }, StringSplitOptions.None);
                string num = Regex.Replace(arraystring[1].ToString(), @"[^\d.\d]", "");

                string cstr = arraystring[1].ToString().Replace(num, "");

                return arraystring[0].ToString() + " " + cstr + " from" + arraystring[2].ToString() + " limit 0, " + num;
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region HtmlDeCode
        public static string HtmlDeCode(this string str)
        {
            return System.Web.HttpContext.Current.Server.HtmlDecode(System.Web.HttpContext.Current.Server.UrlDecode(str)).Replace("''''", "''");
        }
        #endregion

        #region  UrlDecode
        /// <summary>
        /// UrlDecode
        /// </summary>
        /// <param name="str">要解码的地址字符串</param>
        /// <returns></returns>
        public static string UrlDecode(this string str)
        {
            return System.Web.HttpContext.Current.Server.UrlDecode(str);
        }

        public static string UrlDecode(this string str, System.Text.Encoding encode)
        {
            return System.Web.HttpUtility.UrlDecode(str, encode);
        }
        #endregion

        #region  UrlEncode
        /// <summary>
        /// UrlDecode
        /// </summary>
        /// <param name="str">要编码的地址字符串</param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return System.Web.HttpContext.Current.Server.UrlEncode(str);
        }

        public static string UrlEncode(this string str, System.Text.Encoding encode)
        {
            return System.Web.HttpUtility.UrlEncode(str, encode);
        }

        public static string UrlEncode(this string str, string encode)
        {
            return System.Web.HttpUtility.UrlEncode(str, System.Text.Encoding.GetEncoding(encode));
        }
        #endregion

        #region 补全字符串
        /// <summary>
        /// 使用指定字符补全字符串
        /// </summary>
        /// <example>
        /// 如1 通过"1".FillByStrings('0',3) 变成 "001"
        /// </example>
        /// <param name="str"></param>
        /// <param name="FillChar"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string FillByStrings(this string str, char FillChar, int Length)
        {
            if (str.Length < Length)
            {
                for (int i = 0; i <= Length - str.Length; i++)
                {
                    str = FillChar.ToString() + str;
                }
                return str;
            }
            else
            {
                return str;
            }
        }

        #endregion


        #region 转换成标准路径 （去除非法字符）
        /// <summary>
        /// 转换成标准路径 （去除非法字符）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToStandardPath(this string path)
        {

            path = path.Replace(":", "：");
            path = path.Replace("?", "？");
            path = path.Replace(">", "》");
            path = path.Replace("<", "《");
            path = path.Replace("*", "※");
            path = path.Replace("|", "‖");
            path = path.Replace("\"", "");

            return path;
        }
        #endregion

        #region 查找字符串出现的次数
        /// <summary>
        /// 查找字符串出现的次数
        /// </summary>
        /// <param name="str">长短文本</param>
        /// <param name="s">单词</param>
        /// <returns></returns>
        public static int CountString(this string str, string s)
        {
            return Regex.Matches(str, s, RegexOptions.IgnoreCase).Count;
        }
        #endregion

        #region 通过正则表达式在字符串里面查找子字符串
        /// <summary>
        /// 通过正则表达式在字符串里面查找子字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern">表达式</param>
        /// <returns></returns>
        public static string FindString(this string str, string pattern)
        {
            Regex reg = new Regex(pattern);
            Match m = reg.Match(str);
            if (m.Groups.Count == 0)
            {
                return "";
            }
            else
            {
                return m.Groups[1].Value;
            }
        }
        #endregion

        #region GetElementByTagName
        /// <summary>
        /// GetElementByTagName
        /// </summary>
        /// <param name="SourceHtml">Html代码</param>
        /// <param name="TagName">标签名称</param>
        /// <returns>返回标签的OuterHtml</returns>
        public static List<string> GetElementByTagName(this string SourceHtml, string TagName)
        {
            string pattern = string.Format(@"<{0}[^>]*>(?<key>[\w\W]*(((?'Open'<{0}[^>]*>)[\w\W]*)+((?'-Open'</{0}>)[\w\W]*)+)*(?(Open)(?!)))</{0}>", TagName);
            List<string> result = new List<string>();
            List<string> first_result = GetMatch(SourceHtml, pattern);
            foreach (string sub in first_result)
            {
                if (sub.CountString(string.Format("</{0}>", TagName)) > 0)
                {
                    result.Merge(GetElementByTagName(sub, TagName));
                }
            }
            result = result.Merge(first_result);

            return result;
        }
        #endregion

        #region 获取字符串中得匹配结果
        /// <summary>
        /// 获取字符串中得匹配结果
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>返回的结果集</returns>
        public static List<string> GetMatch(this string input, string pattern)
        {
            List<string> result = new List<string>();

            Match m = new Regex(pattern, RegexOptions.IgnoreCase).Match(input);
            while (m.Success)
            {
                result.Add(m.Groups["key"].Value);
                m = m.NextMatch();
            }
            return result;
        }
        #endregion

        #region 转换成UTF-8字符串
        /// <summary>
        /// 转换成UTF-8字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String toUtf8String(this string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c >= 0 && c <= 255)
                {
                    sb.Append(c);
                }
                else
                {
                    byte[] b;
                    try
                    {
                        b = Encoding.UTF8.GetBytes(c.ToString());
                    }
                    catch (Exception ex)
                    {
                        b = new byte[0];
                    }
                    for (int j = 0; j < b.Length; j++)
                    {
                        int k = b[j];
                        if (k < 0) k += 256;

                        sb.Append("%" + Convert.ToString(k, 16).ToUpper());
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 字符串编码转换
        /// <summary>
        /// 字符串编码转换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldCoding">原编码</param>
        /// <param name="newEncoding">新编码</param>
        /// <returns></returns>
        public static string ConvertEnCoding(this string str, Encoding oldCoding, Encoding newEncoding)
        {
            return newEncoding.GetString(oldCoding.GetBytes(str));
        }
        #endregion

        #region 点击访问WebInfo的某项内容
        /// <summary>
        /// 点击访问WebInfo的某项内容
        /// </summary>
        /// <param name="web"></param>
        /// <param name="parttern">正则表达式</param>
        /// <param name="encode">页面编码</param>
        /// <returns></returns>
        public static Voodoo.Net.WebInfo Click(this Voodoo.Net.WebInfo web, string parttern, Encoding encode)
        {
            string html = web.Html;
            Match m = new Regex(parttern, RegexOptions.None).Match(html);
            if (m.Success)
            {
                string url = m.Groups["key"].Value.Replace("&amp;", "&");
                if (!url.Contains("http"))
                {
                    string uri = web.WebUrl.Substring(0, web.WebUrl.LastIndexOf('/'));
                    if (url[0] == '/')
                    {
                        url = "http://" + new Uri(web.WebUrl).Host + url;
                    }
                    else
                    {
                        url = uri + "/" + url;
                    }
                }
                web = Voodoo.Net.Url.PostGetCookieAndHtml(new System.Collections.Specialized.NameValueCollection(), url, encode, web.cookieContainer, web.cookieCollection, web.url);
                return web;
            }
            else
            {
                return new Voodoo.Net.WebInfo();
            }
        }
        #endregion

        #region 替换关键字为红色
        /// <summary>
        /// 替换关键字为红色
        /// </summary>
        /// <param name="keycontent">原始内容</param>
        /// <param name="k">关键字，支持多关键字</param>
        /// <returns>String</returns>
        public static string Highlightkeywords(this string keycontent, string k)
        {
            return keycontent.Highlightkeywords(k, "#F00");
        }
        #endregion

        #region 关键字高亮
        /// <summary>
        /// 关键字高亮
        /// </summary>
        /// <param name="keycontent">原始内容</param>
        /// <param name="k">关键字，支持多关键字</param>
        /// <param name="Color">高亮颜色 如:Red, #F00</param>
        /// <returns></returns>
        public static string Highlightkeywords(this string keycontent, string k, string Color)
        {
            string resultstr = keycontent;
            if (k.Trim().IndexOf(' ') > 0)
            {
                string[] myArray = k.Split(' ');
                for (int i = 0; i < myArray.Length; i++)
                {
                    resultstr = resultstr.Replace(myArray[i].ToString(), "<font color=\"" + Color + "\">" + myArray[i].ToString() + "</font>");
                }
                return resultstr;
            }
            else
            {
                return resultstr.Replace(k, "<font color=\"" + Color + "\">" + k + "</font>");
            }
        }
        #endregion

        #region 获取文件名
        /// <summary>
        /// 根据文件路径获取文件名
        /// </summary>
        /// <param name="str">路径</param>
        /// <returns></returns>
        public static string GetFileNameFromPath(this string str)
        {
            return System.IO.Path.GetFileName(str);
        }

        /// <summary>
        /// 根据文件路径获取文件扩展名
        /// </summary>
        /// <param name="str">路径</param>
        /// <returns></returns>
        public static string GetFileExtNameFromPath(this string str)
        {
            return System.IO.Path.GetExtension(str);
        }
        #endregion

        #region Url参数转换为POST所用的NameValueCollection
        /// <summary>
        /// Url参数转换为POST所用的NameValueCollection
        /// </summary>
        /// <param name="param">参数，如：name=张三&sex=男</param>
        /// <returns></returns>
        public static System.Collections.Specialized.NameValueCollection ParamToNameValueCollection(this string param)
        {
            System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
            string[] pars = param.Split('&');
            foreach (string str in pars)
            {
                string[] pas = str.Split('=');
                if (pas.Length > 1)
                {
                    nv.Add(pas[0], pas[1]);
                }
                else
                {
                    nv.Add(pas[0], "");
                }
            }

            return nv;
        }
        #endregion

        #region 页面地址和域名合并
        /// <summary>
        /// 页面地址和域名合并
        /// </summary>
        /// <param name="page">页面地址，如：/news/22.html</param>
        /// <param name="domain">网站地址，如：http://www.aizr.net</param>
        /// <returns></returns>
        public static string AppendToDomain(this string relaURL, string baseURL)
        {
            Uri u = new Uri(baseURL);
            try
            {
                Uri ur = new Uri(relaURL);
                if (ur.IsAbsoluteUri) return relaURL;
            }
            catch { }
            if (!u.IsAbsoluteUri) return "";
            if (baseURL.LastIndexOf(".") > baseURL.LastIndexOf("/"))    //是文件
                baseURL = baseURL.Substring(0, baseURL.LastIndexOf("/"));
            if (!baseURL.EndsWith("/"))
                baseURL += "/";
            if (relaURL.StartsWith("/")) //是绝对路径
            {
                int p = baseURL.IndexOf("/", 7);
                baseURL = baseURL.Substring(0, p);
                if (!baseURL.EndsWith("/"))
                    baseURL += "/";
                relaURL = relaURL.Remove(0, 1);
            }
            if (relaURL.StartsWith("../")) //多余的相对路径
            {
                string finalUrl = baseURL + relaURL;
                while (Regex.IsMatch(finalUrl, "/[^/]*?/../"))
                {
                    finalUrl = Regex.Replace(finalUrl, "/[^/]*?/../", "/");
                }

                return finalUrl;
            }
            return baseURL + relaURL;
            //if (page.IsNullOrEmpty())
            //{
            //    return domain;
            //}
            //if (page.ToLower().Contains("http://"))
            //{
            //    return page;
            //}
            //if (domain.ToLower().Contains("http") == false)
            //{
            //    domain = "http://" + domain;
            //}
            //if (page.Contains("/") == false)
            //{
            //    return domain.Substring(0, domain.LastIndexOf('/')+1) + page;
            //}
            //if (page[0] == '/')
            //{
            //    return "http://" + new Uri(domain).Host + page;
            //}
            //else
            //{
            //    return domain + page;
            //}
        }
        #endregion

        #region  替换同义词
        /// <summary>
        /// 替换同义词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSynonyms(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return "";
            }

            string[][] Synonyms = { 
                                  
                                      new string[]{"的","地"},
                                      new string[]{"一","1"},
                                      new string[]{"二","2"},
                                      new string[]{"三","3"},
                                      new string[]{"四","4"},
                                      new string[]{"五","5"},
                                      new string[]{"六","6"},
                                      new string[]{"七","7"},
                                      new string[]{"八","8"},
                                      new string[]{"九","9"},
                                      new string[]{"零","0"},
                                      new string[]{"壹","1"},
                                      new string[]{"贰","2"},
                                      new string[]{"叁","3"},
                                      new string[]{"肆","4"},
                                      new string[]{"伍","5"},
                                      new string[]{"陆","6"},
                                      new string[]{"柒","7"},
                                      new string[]{"捌","8"},
                                      new string[]{"玖","9"},
                                      new string[]{"〇","0"},
                                      new string[]{"第",""},
                                      new string[]{"章",""},
                                      new string[]{"引子",""},
                                      new string[]{"    ",""},
                                      new string[]{"\r",""},
                                      new string[]{"\n",""},
                                      new string[]{"。","."},
                                      new string[]{"，",","},
                                      new string[]{" ",""},
                                  };

            str = str.ToLower();
            foreach (string[] ss in Synonyms)
            {
                str = str.Replace(ss[0], ss[1]);
            }

            return str;
        }
        #endregion

        #region 获取两个字符串的相似度
        /// <summary>
        /// 获取两个字符串的相似度
        /// </summary>
        /// <param name="sourceString">第一个字符串</param>
        /// <param name="str">第二个字符串</param>
        /// <returns></returns>
        public static decimal GetSimilarityWith(this string sourceString, string str)
        {
            //参考：http://www.cnblogs.com/eaglet/articles/854804.html
            //公式：相似度=Kq*q/(Kq*q+Kr*r+Ks*s) (Kq > 0 , Kr>=0,Ka>=0) 公式2
            //设置Kq = 2 ,Kr=Ks=1 Kq Kr Ks 为 q r s 的权重
            //设q是字符串1和字符串2中都存在的单词的总数，s是字符串1中存在，字符串2中不存在的单词总数，r是字符串2中存在，字符串1中不存在的单词总数

            //处理同义词
            //同义词当成近义词，如“我的”和“我地” 相似度并不是100%
            string _sourceString = sourceString.ReplaceSynonyms();
            string _str = str.ReplaceSynonyms();

            decimal Kq = 2;
            decimal Kr = 1;
            decimal Ks = 1;

            char[] _ss = _sourceString.ToCharArray();
            char[] _st = _str.ToCharArray();

            char[] ss = sourceString.ToCharArray();
            char[] st = str.ToCharArray();

            //获取交集数量

            int _q = _ss.Intersect(_st).Count();
            int _s = _ss.Length - _q;
            int _r = _st.Length - _q;

            int q = ss.Intersect(st).Count();
            int s = ss.Length - q;
            int r = st.Length - q;

            //原字符串相似度
            decimal sourWeight = Kq * q / (Kq * q + Kr * r + Ks * s);

            decimal simWeight = Kq * _q / (Kq * _q + Kr * _r + Ks * _s);

            return (sourWeight + simWeight) / 2;
        }
        #endregion 获取两个字符串的相似度

        #region  获取html字符串控件的属性
        /// <summary>
        /// 获取html字符串控件的属性
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="AttrName"></param>
        /// <returns></returns>
        public static string GetHtmlElementAttribute(this string ele,string AttrName)
        {
            return ele.FindString(string.Format("{0}=\"(?<key>.*?)\"|{0}=(?<key>.*?)", AttrName));
        }
        #endregion 

        #region 序列化HTML表单
        /// <summary>
        /// 序列化HTML表单
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NameValueCollection SerializeForm(this string html, string id)
        {
            string strForm = "";
            if (id[0] == '#')
            {
                strForm = html.GetMatch(string.Format("<form[\\s\\S]*?id=\"{0}\"[\\s\\S]*?>(?<key>[\\s\\S]*?)</form>", id.Replace("#", "")))[0];
            }
            else if (id[0] == '.')
            {
                strForm = html.GetMatch(string.Format("<form[\\s\\S]*?class=\"{0}\"[\\s\\S]*?>(?<key>[\\s\\S]*?)</form>", id.Replace(".", "")))[0];
            }
            else if (id[0] == '@')
            {
                strForm = html.GetMatch(string.Format("<form[\\s\\S]*?name=\"{0}\"[\\s\\S]*?>(?<key>[\\s\\S]*?)</form>", id.Replace("@", "")))[0];
            }
            else
            {
                strForm = html.GetMatch(string.Format("<form[\\s\\S]*?id=\"{0}\"[\\s\\S]*?>(?<key>[\\s\\S]*?)</form>", id.Replace("#", "")))[0];
            }

            Match match_Input = new Regex("<input .*?>|<textarea[\\w\\W]*?</textarea>").Match(strForm);

            NameValueCollection nv = new NameValueCollection();
            while (match_Input.Success)
            {
                string ele = match_Input.Groups[0].Value.ToS();
                if ((ele.GetHtmlElementAttribute("type") == "checkbox" || ele.GetHtmlElementAttribute("type") == "radio") && ele.GetHtmlElementAttribute("checked") != "checked")
                {
                    match_Input = match_Input.NextMatch();
                    continue;
                }
                else
                {
                    nv.Add(ele.GetHtmlElementAttribute("name"), ele.GetHtmlElementAttribute("value"));
                }
                match_Input = match_Input.NextMatch();

            }

            return nv;
        }
        #endregion

        #region 为空字符串赋值
        /// <summary>
        /// 为空字符串赋值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="Value">值</param>
        /// <returns></returns>
        public static string IsNull(this string str, string Value)
        {
            if (str.IsNullOrEmpty())
            {
                str = Value;
            }
            return str;
        }
        #endregion
    }
}
