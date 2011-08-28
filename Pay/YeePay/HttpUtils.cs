using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace Voodoo.Pay.YeePay
{
    /// <summary>
    /// 通讯函数
    /// </summary>
    public abstract class HttpUtils
    {
        public static string encodingName = "gb2312";

        public HttpUtils()
        {

        }

        #region 通讯函数
        /// <summary>
        /// 通讯函数
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="para">请求参数</param>
        /// <param name="method">请求方式GET/POST</param>
        /// <returns></returns>
        public static string SendRequest(string url, string para, string method)
        {
            string strResult = "";

            if (url == null || url == "")
                return null;

            if (method == null || method == "")
                method = "GET";

            // GET方式
            if (method.ToUpper() == "GET")
            {
                try
                {
                    HttpWebRequest wrq = (HttpWebRequest)WebRequest.Create(url + para);
                    wrq.AllowAutoRedirect = false;
                    HttpWebResponse wrp = (HttpWebResponse)wrq.GetResponse();
                    if (((int)wrp.StatusCode) == 302)
                    {
                        url = wrp.Headers["Location"];
                        if (url.IndexOf("?") < 0)
                        {
                            para = "&" + para.Substring(1, para.Length - 1);
                        }
                        wrq = (HttpWebRequest)WebRequest.Create(url + para);

                    }
                    System.IO.StreamReader sr = new System.IO.StreamReader(wrp.GetResponseStream(), System.Text.Encoding.GetEncoding(encodingName));

                    strResult = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            // POST方式
            if (method.ToUpper() == "POST")
            {
                if (para.Length > 0 && para.IndexOf('?') == 0)
                {
                    para = para.Substring(1);
                }

                WebRequest req = WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (para != null)
                {
                    int i = 0, j;
                    while (i < para.Length)
                    {
                        j = para.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            UrlEncoded.Append(HttpUtility.UrlEncode(para.Substring(i, para.Length - i), System.Text.Encoding.GetEncoding(encodingName)));
                            break;
                        }
                        UrlEncoded.Append(HttpUtility.UrlEncode(para.Substring(i, j - i), System.Text.Encoding.GetEncoding(encodingName)));
                        UrlEncoded.Append(para.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                try
                {
                    WebResponse result = req.GetResponse();
                    Stream ReceiveStream = result.GetResponseStream();

                    Byte[] read = new Byte[512];
                    int bytes = ReceiveStream.Read(read, 0, 512);

                    while (bytes > 0)
                    {

                        // 注意：
                        // 下面假定响应使用 UTF-8 作为编码方式。
                        // 如果内容以 ANSI 代码页形式（例如，932）发送，则使用类似下面的语句：
                        //  Encoding encode = System.Text.Encoding.GetEncoding("shift-jis");
                        Encoding encode = System.Text.Encoding.GetEncoding(encodingName);
                        strResult += encode.GetString(read, 0, bytes);
                        bytes = ReceiveStream.Read(read, 0, 512);
                    }

                    return strResult;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return strResult;
        }
        #endregion

        #region 简化通讯函数
        /// <summary>
        /// GET方式通讯
        /// </summary>
        /// <param name="url"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string SendRequest(string url, string para)
        {
            return SendRequest(url, para, "GET");
        }
        #endregion

    }
}
