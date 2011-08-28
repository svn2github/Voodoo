using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo.Security
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public static class Client
    {

        #region 获取客户端信息
        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <returns></returns>
        public static string RemoteInfos()
        {
            string remark = "<br />电脑名：" + RemoteHostName +
                    "<br />帐号：" + RemoteUser +
                    "<br />系统信息：" + RemoteUserAgent +
                    "<br />客户端Cookie：" + RemoteCookie +
                    "<br />客户端语言:" + RemoteLanguage +
                    "<br />端口：" + RemotePort;
            return remark;
        }
        #endregion

        #region 客户端IP地址
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string RemoteAddress
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                catch
                {
                    return "";
                }

            }
        }

        /// <summary>
        /// 十进制客户端IP地址
        /// </summary>
        public static long IpAddressDecimal1
        {
            get
            {
                string[] ips = RemoteAddress.Split('.');
                if (ips.Length == 4)
                {
                    long dip = 0;
                    dip += Convert.ToInt64(ips[3]);
                    dip += Convert.ToInt64(ips[2]) * 256;
                    dip += Convert.ToInt64(ips[1]) * 256 * 256;
                    dip += Convert.ToInt64(ips[0]) * 256 * 256 * 256;
                    return dip;
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion

        #region  客户端主机名
        /// <summary>
        /// 客户端主机名
        /// </summary>
        /// <returns></returns>
        public static string RemoteHostName
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"].ToString();
                }
                catch
                {
                    return "";
                }
            }


        }
        #endregion

        #region 客户端用户名
        /// <summary>
        /// 客户端用户名
        /// </summary>
        /// <returns></returns>
        public static string RemoteUser
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_USER"].ToString();
                }
                catch
                {
                    return "";
                }
            }


        }
        #endregion

        #region 浏览器信息
        /// <summary>
        /// 浏览器信息
        /// </summary>
        /// <returns></returns>
        public static string RemoteUserAgent
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();
                }
                catch
                {
                    return "";
                }

            }


        }
        #endregion

        #region 客户端Cookie
        /// <summary>
        /// 客户端Cookie
        /// </summary>
        /// <returns></returns>
        public static string RemoteCookie
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_COOKIE"].ToString();
                }
                catch
                {
                    return "";
                }

            }


        }
        #endregion

        #region  客户端语言
        /// <summary>
        /// 客户端语言
        /// </summary>
        /// <returns></returns>
        public static string RemoteLanguage
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToString();
                }
                catch
                {
                    return "";
                }

            }

        }
        #endregion

        #region 客户端端口
        /// <summary>
        /// 客户端端口
        /// </summary>
        /// <returns></returns>
        public static string RemotePort
        {
            get
            {
                try
                {
                    return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_PORT"].ToString();
                }
                catch
                {
                    return "";
                }
            }


        }
        #endregion

        #region 当前页面地址
        /// <summary>
        /// 当前页面地址
        /// </summary>
        public static string Url
        {
            get
            {
                return System.Web.HttpContext.Current.Request.Url.ToString();
            }
        }
        #endregion

        #region  浏览器名
        /// <summary>
        /// 浏览器名
        /// </summary>
        public static string Browser
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.UserAgent.IndexOf("MSIE 6") > -1) return "Internet Explorer 6";
                if (System.Web.HttpContext.Current.Request.UserAgent.IndexOf("MSIE 7") > -1) return "Internet Explorer 7";
                if (System.Web.HttpContext.Current.Request.UserAgent.IndexOf("MSIE 8") > -1) return "Internet Explorer 8";
                if (System.Web.HttpContext.Current.Request.UserAgent.IndexOf("Chrome") > -1) return "Chrome 谷歌浏览器";
                if (System.Web.HttpContext.Current.Request.UserAgent.IndexOf("Firefox") > -1) return "Firefox 火狐浏览器";
                if (System.Web.HttpContext.Current.Request.UserAgent.IndexOf("Safari") > -1) return "苹果 Safari 浏览器";
                return "未知浏览器";
            }
        }
       #endregion

        #region  客户端安装了.NET Framework
        /// <summary>
        ///  客户端安装了.NET Framework
        /// </summary>
        public static bool HaveFramework
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.UserAgent.IndexOf(".NET CLR") > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
