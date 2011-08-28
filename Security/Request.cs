using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Voodoo.Security
{
    /// <summary>
    /// Request类
    /// 2010年4月12日
    /// </summary>
    public class Request
    {


        #region
        /// <summary> 
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址 ，适用多层代理 
        /// </summary> 
        public static string IPAddress()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            if (!string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理 
                    if (result.IndexOf(".") == -1) //没有"."肯定是非IPv4格式 
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") >= 0)
                        {
                            //有","，估计多个代理。取第一个不是内网的IP。 
                            result = result.Replace(" ", "").Replace("'", "").Replace(";", "");
                            string[] temparyip = result.Split(',');
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i]))
                                {
                                    if (!temparyip[i].StartsWith("10.") && !temparyip[i].StartsWith("192.168") && !temparyip[i].StartsWith("172.16."))
                                    {
                                        result = temparyip[i]; //找到不是内网的地址
                                        break;
                                    }
                                }
                            }
                        }
                        if (!IsIPAddress(result)) //代理即是IP格式 
                        {
                            result = null; //代理中的内容 非IP，取IP k
                        }
                    }
                }
            }
            else
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                result = "000.000.000.000";
            }
            return result;
        }
        #endregion
        #region bool IsIPAddress(str1) 判断是否是IP格式
        /**/
        /// <summary> 
        /// 判断是否是IP地址格式 0.0.0.0 
        /// </summary> 
        /// <param name="str1">待判断的IP地址</param> 
        /// <returns>true or false</returns> 
        public static bool IsIPAddress(string str1)
        {
            if (string.IsNullOrEmpty(str1)) { return false; }
            if (str1.Length < 7 || str1.Length > 15) { return false; }
            string regformat = @"^\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
        #endregion


        #region 获取Int64类型IP
        /// <summary>
        /// 获取ToInt64类型IP
        /// </summary>
        /// <param name="Go"></param>
        /// <returns></returns>
        public static long GetIpLong(bool Go)
        {
            string[] strArray = IP().ToString().Split('.');
            return Convert.ToInt64(strArray[0]) * 256 * 256 * 256 + Convert.ToInt64(strArray[1]) * 256 * 256 + Convert.ToInt64(strArray[2]) * 256 + Convert.ToInt64(strArray[3]);
        }
        #endregion

        #region 获取IP地址
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string IP()
        {
            return IPAddress();
        }
        #endregion

        #region IP数据类型转成Int64
        /// <summary>
        /// IP数据类型转成Int64
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static long IpToLong(string Str)
        {
            string[] strArray = Str.Split('.');
            return Convert.ToInt64(strArray[0]) * 256 * 256 * 256 + Convert.ToInt64(strArray[1]) * 256 * 256 + Convert.ToInt64(strArray[2]) * 256 + Convert.ToInt64(strArray[3]);
        }
        #endregion

        #region 获取客户端是否使用的 HTTP Get 数据传输方法
        /// <summary>
        /// 获取客户端是否使用的 HTTP Get 数据传输方法
        /// </summary>
        /// <returns></returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }
        #endregion

        #region 获取客户端是否使用的 HTTP POST 数据传输方法
        /// <summary>
        /// 获取客户端是否使用的 HTTP POST 数据传输方法
        /// </summary>
        /// <returns></returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        #endregion

        #region 判断请求的 URL 是否为搜索引擎
        /// <summary>
        /// 判断请求的 URL 是否为搜索引擎
        /// </summary>
        /// <returns></returns>
        public static bool IsSearchEngines()
        {
            string[] strArray = new string[] { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom" };
            string str = UrlReferrer().ToLower();
            int num3 = strArray.Length - 1;
            for (int i = 0; i <= num3; i++)
            {
                if (str.IndexOf(strArray[i]) > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 获取有关当前请求的 URL 的信息
        /// <summary>
        /// 获取有关当前请求的 URL 的信息
        /// </summary>
        /// <returns></returns>
        public static string Url()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        #endregion

        #region 获取有关客户端上次请求的 URL 的信息，该请求链接到当前的 URL
        /// <summary>
        /// 获取有关客户端上次请求的 URL 的信息，该请求链接到当前的 URL
        /// </summary>
        /// <returns></returns>
        public static string UrlReferrer()
        {
            return HttpContext.Current.Request.UrlReferrer.ToString();
        }
        #endregion
    }
}