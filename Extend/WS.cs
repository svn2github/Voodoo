using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Voodoo.Net.XmlRpc;
using System.IO;

namespace Voodoo
{
    /// <summary>
    /// 猥琐类
    /// 我就是猥琐
    /// 该类包含一些底层常用的方法
    /// </summary>
    public class WS
    {
        #region 取得Request的int型值，Form优先于QueryString被取出
        /// <summary>
        /// 取得Request的int型值，任何一个错误均返回int.MinValue，Form优先于QueryString被取出
        /// </summary>
        /// <param name="parName">参数名称</param>
        /// <returns>如果获取不到，返回int.MinValue</returns>
        public static int RequestInt(string parName)
        {
            return RequestInt(parName, int.MinValue);
        }

        /// <summary>
        /// 取得Request的int型值，Form优先于QueryString被取出
        /// </summary>
        /// <param name="parName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>如果获取不到，返回默认值</returns>
        public static int RequestInt(string parName, int defaultValue)
        {

            try
            {
                return int.Parse(RequestString(parName));
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        public static int? RequestNullInt32(string parName)
        {
            try
            {
                return int.Parse(RequestString(parName));
            }
            catch
            {
                return null;
            }
        }

        public static long RequestLong(string parName)
        {
            return RequestLong(parName, long.MinValue);
        }
        public static long RequestLong(string parName, long defaultValue)
        {

            try
            {
                return long.Parse(RequestString(parName));
            }
            catch
            {
                return defaultValue;
            }
        }

        #region 取得Request的string值，任何错误均返回""，Form优先于QueryString被取出
        /// <summary>
        /// 取得Request的string值，任何错误均返回""，Form优先于QueryString被取出
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>键值</returns>
        public static string RequestString(object key)
        {
            return RequestString(key, "");
        }
        /// <summary>
        /// 取得Request的string值，Form优先于QueryString被取出
        /// </summary>
        /// <param name="key">参数名称</param>
        /// <param name="defaultValue">如果获取不到，返回的默认值</param>
        /// <returns>出a现错误返回默认值</returns>
        public static string RequestString(object key, string defaultValue)
        {
            if (HttpContext.Current.Request.Form[key.ToString()] != null)
            {
                return System.Web.HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Request.Form[key.ToString()].ToString()).ToString().ToSqlEnCode().TrimDbDangerousChar();
            }
            if (HttpContext.Current.Request.QueryString[key.ToString()] != null)
            {
                return System.Web.HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Request.QueryString[key.ToString()].ToString()).ToSqlEnCode().TrimDbDangerousChar();
            }
            return defaultValue;
        }

        #endregion 通过Request对象取得的值

        #region 得到当前域名
        /// <summary>
        /// 得到当前域名
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }
        #endregion

        #region 判断当前访问是否来自浏览器软件
        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region 判断是否来自搜索引擎链接
        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region 获得当前完整Url地址
        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        #endregion

        #region 获得当前页面的文件名称
        /// <summary>
        /// 获得当前页面的文件名称
        /// </summary>
        /// <returns>当前页面的文件名称</returns>
        public static string GetPageName()
        {
            try
            {
                string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
                return urlArr[urlArr.Length - 1].ToLower();
            }
            catch (System.Exception e)
            {
                return "";
            }

        }
        #endregion

        #region 获得当前页面客户端的IP
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {

            return Security.Request.IPAddress();
        }
        #endregion

        #region 保存用户上传的文件
        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }
        #endregion

        #region 生成指定数量的html空格符号
        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        public static string GetSpacesString(int spacesCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < spacesCount; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }
        #endregion

        #region 判断请求是否来自移动终端
        /// <summary>
        /// 判断请求是否来自移动终端
        /// </summary>
        /// <returns></returns>
        public static bool IsFromMobile()
        {
            if (HttpContext.Current.Request.UserAgent == null)
                return false;

            string[] SearchEngine = { "android", "nokia", "symbian", "ios", "ucweb", "wap", "up.browser", "sonyericsson", "mobile" };
            string tmpReferrer = HttpContext.Current.Request.UserAgent.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region 获取XML-RPC的ping请求
        /// <summary>
        /// 获取XML-RPC的ping请求
        /// </summary>
        /// <returns>方法调用</returns>
        public static methodCall RequestPing()
        {
            string request = "";
            using (Stream MyStream = HttpContext.Current.Request.InputStream)
            {
                byte[] _tmpData = new byte[MyStream.Length];
                MyStream.Read(_tmpData, 0, _tmpData.Length);
                request = Encoding.UTF8.GetString(_tmpData);
            }

            return (methodCall)Voodoo.IO.XML.DeSerialize(typeof(methodCall), request);
        }
        #endregion

        #region 获取当前app运行目录
        /// <summary>
        /// 获取当前app运行目录
        /// </summary>
        public static string BaseDirectory
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        #endregion
    }
}
