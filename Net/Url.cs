using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Collections.Specialized;


namespace Voodoo.Net
{
    /// <summary>
    /// URL处理相关类
    /// </summary>
    public static class Url
    {


        #region 远程获取url地址的页面源代码
        /// <summary>
        /// 远程获取url地址的页面源代码
        /// </summary>
        /// <param name="url">要获取页面的URL</param>
        /// <returns>返回HTML代码</returns>
        public static string GetHtml(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "www.svnhost.cn";

                //WebProxy proxy = new WebProxy();                                      //定義一個網關對象
                //proxy.Address = new Uri("http://120.203.214.148:80");              //網關服務器:端口
                ////proxy.Credentials = new NetworkCredential("f3210316", "6978233");      //用戶名,密碼
                //request.Proxy = proxy;

                request.Timeout = 20000;
                request.AllowAutoRedirect = true;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (request != null)
                {
                    request = null;
                }
            }
            return string.Empty;
        }
        #endregion

        #region 远程获取url地址的页面源代码
        /// <summary>
        /// 远程获取url地址的页面源代码
        /// </summary>
        /// <param name="url">要获取页面的URL</param>
        /// <returns>返回HTML代码</returns>
        public static string GetHtml(string url, string ucoid)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.2 Safari/535.11";


                request.Timeout = 60000;
                request.AllowAutoRedirect = true;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)// && response.ContentLength < 1024 * 1024)
                {
                    string c = ucoid;
                    string html = "";
                    if (ucoid.IsNullOrEmpty())
                    {
                        c = response.ContentType.Replace("text/html", "").Replace("charset=", "").Replace(";", "").Trim();
                        if (c.IsNullOrEmpty())//为空
                        {
                            c = "gb2312";
                            reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(c));
                            html = reader.ReadToEnd();
                            if (html.IsNullOrEmpty())
                            {
                                WebClient wc = new WebClient();

                                html = wc.DownloadString(url);
                                wc.Dispose();
                            }

                            string charset = new Regex("charset=(?<key>.*?)\"", RegexOptions.None).Match(html).Groups["key"].Value.Replace("\"", "");
                            if (c == charset)
                            {
                                return html;
                            }
                            c = charset;
                            if (c.IsNullOrEmpty())
                            {
                                return "没找到任何合适的编码格式";
                            }
                            html = GetHtml(url, c);
                        }
                        else
                        {
                            html = GetHtml(url, c);
                        }
                    }
                    else//输入了编码
                    {
                        reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(c));
                        html = reader.ReadToEnd();
                        if (html.IsNullOrEmpty())
                        {
                            WebClient wc = new WebClient();
                            byte[] webData = wc.DownloadData(url);
                            html = Encoding.GetEncoding(c).GetString(webData);
                            wc.Dispose();
                        }
                    }
                    return html;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (request != null)
                {
                    request = null;
                }
            }
            return string.Empty;
        }
        #endregion

        #region 远程获取url地址的页面源代码
        /// <summary>
        /// 远程获取url地址的页面源代码
        /// </summary>
        /// <param name="url">要获取页面的URL</param>
        /// <returns>返回HTML代码</returns>
        public static string GetHtml(string url, string ucoid, CookieContainer cc)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "www.svnhost.cn";
                request.CookieContainer = cc;



                request.Timeout = 20000;
                request.AllowAutoRedirect = true;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    if (ucoid == "utf-8")
                    {
                        reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                    }
                    else if (ucoid == "gb2312")
                    {
                        reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
                    }
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (request != null)
                {
                    request = null;
                }
            }
            return string.Empty;
        }


        #endregion

        #region  获取Alexa排名
        /// <summary>
        /// 获取Alexa排名
        /// </summary>
        /// <param name="host">网站地址</param>
        /// <returns>排名序号</returns>
        public static int Alexa(this string host)
        {
            string html = GetHtml("http://data.alexa.com/data/?cli=10&dat=snba&ver=7.0&url=" + host);
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

        #region 将数据提交到指定URL
        /// <summary>
        /// 将数据提交到指定URL
        /// </summary>
        /// <param name="postVars">要提交的表单数据</param>
        /// <param name="Url">目标URL</param>
        /// <returns>URL响应出的字符串</returns>
        public static string Post(System.Collections.Specialized.NameValueCollection postVars, string Url, System.Text.Encoding encode)
        {
            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            try
            {
                byte[] byRemoteInfo = WebClientObj.UploadValues(Url, "Post", postVars);

                string sRemoteInfo = encode.GetString(byRemoteInfo); //System.Text.Encoding.Default.GetString(byRemoteInfo);
                return sRemoteInfo;
            }
            catch
            {
                return "";
            }
        }

        public static string Post(System.Collections.Specialized.NameValueCollection postVars, string Url)
        {
            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            try
            {
                byte[] byRemoteInfo = WebClientObj.UploadValues(Url, "Post", postVars);

                string sRemoteInfo = System.Text.Encoding.Default.GetString(byRemoteInfo);
                return sRemoteInfo;
            }
            catch
            {
                return "";
            }
        }

        public static string Post(System.Collections.Specialized.NameValueCollection postVars, string Url, System.Text.Encoding encode, CookieContainer cc)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                //request.SendChunked = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2)";
                request.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                // request.TransferEncoding = "gzip, deflate";
                request.Referer = "http://www.njxsw.com/forum-44-1.html";
                //request.Connection = "Keep-Alive";
                request.CookieContainer = cc;
                request.Method = "post";
                request.Timeout = 99999999;



                string param = "";

                for (int i = 0; i < postVars.Count; i++)
                {
                    param += postVars.Keys[i] + "=" + postVars[i];
                    if (i != postVars.Count - 1)
                    {
                        param += "&";
                    }
                }

                byte[] bs = encode.GetBytes(param);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bs.Length;


                request.Timeout = 20000;
                request.AllowAutoRedirect = true;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {

                    reader = new StreamReader(response.GetResponseStream(), encode);
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (request != null)
                {
                    request = null;
                }
            }
            return string.Empty;
        }



        public static string Post(System.Collections.Specialized.NameValueCollection postVars, string Url, System.Text.Encoding encode, CookieContainer cc, string Accept, string Referer, string UserAgent)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.UserAgent = UserAgent;
                request.Accept = Accept;
                request.Referer = Referer;
                request.CookieContainer = cc;
                request.Method = "post";
                //request.Expect = null;


                string param = "";

                for (int i = 0; i < postVars.Count; i++)
                {
                    param += postVars.Keys[i] + "=" + postVars[i];
                    if (i != postVars.Count - 1)
                    {
                        param += "&";
                    }
                }

                byte[] bs = encode.GetBytes(param);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bs.Length;


                request.Timeout = 60000;
                request.AllowAutoRedirect = true;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                response = (HttpWebResponse)request.GetResponse();


                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {

                    reader = new StreamReader(response.GetResponseStream(), encode);
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (request != null)
                {
                    request = null;
                }
            }
            return string.Empty;
        }



        public static CookieCollection PostAndGetCookie(System.Collections.Specialized.NameValueCollection postVars, string Url, Encoding encode, CookieContainer cookieContainer)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2)";
                request.Referer = "http://www.njxsw.com//";
                request.CookieContainer = cookieContainer;
                request.Method = "post";

                string param = "";

                for (int i = 0; i < postVars.Count; i++)
                {
                    param += postVars.Keys[i] + "=" + postVars[i];
                    if (i != postVars.Count - 1)
                    {
                        param += "&";
                    }
                }

                byte[] bs = encode.GetBytes(param);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bs.Length;


                request.Timeout = 20000;
                request.AllowAutoRedirect = true;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    reader = new StreamReader(response.GetResponseStream(), encode);
                    string html = reader.ReadToEnd();
                    return response.Cookies;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (request != null)
                {
                    request = null;
                }
            }
            return null;

        }

        /// <summary>
        /// 提交数据获取Cookie
        /// </summary>
        /// <param name="postVars"></param>
        /// <param name="Url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static CookieCollection PostAndGetCookie(System.Collections.Specialized.NameValueCollection postVars, string Url, Encoding encode)
        {
            return PostAndGetCookie(postVars, Url, encode, new CookieContainer());
        }

        /// <summary>
        /// 提交数据获取Cookie和Html
        /// </summary>
        /// <param name="postVars"></param>
        /// <param name="Url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static WebInfo PostGetCookieAndHtml(System.Collections.Specialized.NameValueCollection postVars, string Url, Encoding encode)
        {
            return PostGetCookieAndHtml(postVars, Url, encode, new CookieContainer());
        }

        public static WebInfo PostGetCookieAndHtml(System.Collections.Specialized.NameValueCollection postVars, string Url, Encoding encode, CookieContainer cookieContainer)
        {
            return PostGetCookieAndHtml(postVars, Url, encode, cookieContainer,"http://soso888.net");
        }

        /// <summary>
        /// 提交数据获取Cookie和Html
        /// </summary>
        /// <param name="postVars"></param>
        /// <param name="Url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static WebInfo PostGetCookieAndHtml(System.Collections.Specialized.NameValueCollection postVars, string Url, Encoding encode, CookieContainer cookieContainer,CookieCollection cookieCollection,string Refer)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2)";
                request.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.112 Safari/535.1";
                request.Referer = Refer;
                request.CookieContainer = cookieContainer;
                request.Method = "post";

                request.AllowAutoRedirect = false;
                //request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Accept = "*/*";
                


                string param = "";

                for (int i = 0; i < postVars.Count; i++)
                {
                    param += postVars.Keys[i] + "=" + postVars[i];
                    if (i != postVars.Count - 1)
                    {
                        param += "&";
                    }
                }

                byte[] bs = encode.GetBytes(param);

                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bs.Length;


                request.Timeout = 60000;
                //request.AllowAutoRedirect = true;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

 
                if ((response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found) && response.ContentLength < 1024 * 1024)
                {

                    reader = new StreamReader(response.GetResponseStream(), encode);
                    string html = reader.ReadToEnd();

                    WebInfo web = new WebInfo();
                    web.statusCode = response.StatusCode;
                    web.WebUrl = Url;
                    if (response.StatusCode == HttpStatusCode.Found)
                    {
                        web.url = response.Headers["location"];
                    }


                    CookieCollection newCC = response.Cookies;
                    foreach (System.Net.Cookie c in newCC)
                    {
                        
                        if (!cookieCollection.Exist(c))
                        {
                            cookieCollection.Add(c);
                        }
                    }
  
                    try
                    {
                        web.cookieCollection = cookieCollection;
                        web.cookieContainer = cookieCollection.ToCookieContainer();
                    }
                    catch (System.Exception e)
                    {
                    	
                    }

                    web.Html = html;

                    while (web.statusCode == HttpStatusCode.Found)
                    {
                        web = PostGetCookieAndHtml(new NameValueCollection(), web.url, encode, cookieContainer,cookieCollection, Url);
                    }
                    

                    return web;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (request != null)
                {
                    request = null;
                }
            }
            return null;
        }

        public static WebInfo PostGetCookieAndHtml(System.Collections.Specialized.NameValueCollection postVars, string Url, Encoding encode, CookieContainer cookieContainer, string Refer)
        {
            return PostGetCookieAndHtml(postVars, Url, encode, cookieContainer, Refer, true);
        }

        public static WebInfo PostGetCookieAndHtml(System.Collections.Specialized.NameValueCollection postVars, string Url, Encoding encode, CookieContainer cookieContainer, string Refer,bool AllowRedirect)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2)";
                request.Referer = Refer;
                request.CookieContainer = cookieContainer;

                request.Method = "post";
                request.AllowAutoRedirect = AllowRedirect;
                //request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;


                string param = "";

                for (int i = 0; i < postVars.Count; i++)
                {
                    param += postVars.Keys[i] + "=" + postVars[i];
                    if (i != postVars.Count - 1)
                    {
                        param += "&";
                    }
                }

                byte[] bs = encode.GetBytes(param);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bs.Length;


                request.Timeout = 60000;
                //request.AllowAutoRedirect = true;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                response = (HttpWebResponse)request.GetResponse();


                if ((response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found) && response.ContentLength < 1024 * 1024)
                {

                    reader = new StreamReader(response.GetResponseStream(), encode);
                    string html = reader.ReadToEnd();


                    WebInfo web = new WebInfo();
                    web.statusCode = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Found)
                    {
                        web.url = response.Headers["location"];
                    }

                    web.WebUrl = Url;
                    web.url = response.ResponseUri.AbsoluteUri;

                    CookieCollection cookieCollection = new CookieCollection();
                    CookieCollection newCC = response.Cookies;
                    foreach (System.Net.Cookie c in newCC)
                    {

                        if (!cookieCollection.Exist(c))
                        {
                            cookieCollection.Add(c);
                        }
                    }

                    cookieContainer.Add(response.Cookies);
                    web.cookieContainer = cookieContainer;
                    web.cookieCollection = cookieCollection;
                    web.Html = html;

                    return web;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (request != null)
                {
                    request = null;
                }
            }
            return null;
        }
        #endregion

        #region 分析html中的标题、时间、正文
        /// <summary>
        /// 分析html中的标题、时间、正文
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        public static TopicInfo AnalysisArticle(string Html)
        {
            Html = Html.ToLower();
            Html = Html.TrimScript();

            Html = Regex.Replace(Html, "<img[\\s\\S]*?>", "");
            Html = Regex.Replace(Html, "<style>[\\s\\S]*?</style>", "");
            TopicInfo topic = new TopicInfo();


            #region 获取标题
            //分析标题
            List<string> titles = new List<string>();
            List<ContentAndWeight> w_title = new List<ContentAndWeight>();

            string title_content = new Regex("<title.*?>(?<key>[\\w\\W]*?)</title>", RegexOptions.IgnoreCase).Match(Html).Groups["key"].Value;

            string window_title = new Regex("<title>(?<key>[\\w\\W]*?)([-_–-]+|</title>)", RegexOptions.IgnoreCase).Match(Html).Groups["key"].Value;

            titles.Merge(new Regex("<meta name=\"keywords\" content=\"(?<key>.*?)\"", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));
            w_title.Add(new ContentAndWeight { Content = new Regex("<meta name=\"keywords\" content=\"(?<key>.*?)\"", RegexOptions.IgnoreCase).Match(Html).Groups["key"].Value.ToS(), Weigth = 10 });

            titles.Merge(new Regex("(?<key><td.*?>.*?</td>)", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));

            titles.Merge(new Regex("(?<key><div.*?>.*?</div>)", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));

            titles.Merge(new Regex("(?<key><h.*?>.*?</h.*?>)", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));

            titles.Merge(new Regex("(?<key><span.*?>.*?</span>)", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));


            foreach (string str in titles)
            {
                ContentAndWeight w = new ContentAndWeight();
                w.Content = str;
                w.Weigth = 0;
                w_title.Add(w);
            }

            foreach (ContentAndWeight cw in w_title)
            {
                if (cw.Content.IndexOf("title") > -1 || cw.Content.IndexOf("head") > -1)
                {
                    cw.Weigth++;
                }

                if (cw.Content.IndexOf("<b>") > -1)
                {
                    cw.Weigth++;
                }
                if (cw.Content.IndexOf("<strong>") > -1)
                {
                    cw.Weigth++;
                }

                if (cw.Content.Length > 3 && cw.Weigth < 35)
                {
                    cw.Weigth += 5;

                }
            }

            string result_title = "";
            int weight = 0;
            foreach (ContentAndWeight cw in w_title)
            {
                if (cw.Weigth > weight)
                {
                    result_title = cw.Content;
                    weight = cw.Weigth;
                }

            }


            if (window_title.Length == 0)
            {
                result_title = title_content;
            }
            else
            {
                result_title = window_title.Trim();
            }

            if (result_title.Trim().Length == 0)
            {

            }

            topic.Title = result_title;

            #endregion

            #region 匹配网站名

            List<string> site = new List<string>();
            List<ContentAndWeight> w_site = new List<ContentAndWeight>();

            site.Merge(new Regex("[-_－](?<key>[^a-zA-Z]{2,20})</title>", RegexOptions.IgnoreCase).Match(Html.Replace(" ", "")).GetMatchList("key"));
            site.Merge(new Regex("<metaname=\"source\"content=\"(?<key>.*?)\"", RegexOptions.IgnoreCase).Match(Html.Replace(" ", "")).GetMatchList("key"));
            site.Merge(new Regex(@"来源[:：]+[\s\S](?<key>[^a-zA-Z]{2,20})", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));
            site.Merge(new Regex(@"(?<key>[u4e00-u9fa5]{2,20})[^u4e00-u9fa5]版权所有", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));

            foreach (string str in site)
            {
                w_site.Add(new ContentAndWeight { Content = str, Weigth = 0 });
            }

            foreach (ContentAndWeight cw in w_site)
            {
                cw.Weigth += cw.Content.CountString("网");
                cw.Weigth += cw.Content.CountString("新闻");
                cw.Weigth -= cw.Content.CountString("。");
            }


            string site_result = "";
            int site_weight = 0;
            foreach (ContentAndWeight cw in w_site)
            {
                if (cw.Weigth > site_weight)
                {
                    site_result = cw.Content;
                    site_weight = cw.Weigth;
                }
            }

            topic.From = site_result;

            #endregion

            #region 获取时间
            //获取时间
            List<string> time = new List<string>();
            List<ContentAndWeight> w_time = new List<ContentAndWeight>();

            time.Merge(new Regex("<td.*?>(?<key>[\\w\\W]*?)</td>", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));
            time.Merge(new Regex("<div.*?>(?<key>[\\w\\W]*?)</div>", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));
            time.Merge(new Regex("<span.*?>(?<key>[\\w\\W]*?)</span>", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));

            foreach (string str in time)
            {
                ContentAndWeight w = new ContentAndWeight();
                w.Content = str.TrimHTML();
                w.Weigth = 0;
                w_time.Add(w);
            }

            foreach (ContentAndWeight w in w_time)
            {

                if (w.Content.CountString("来源") > 0)
                {
                    w.Weigth++;
                }
                if (w.Content.CountString("时间") > 0)
                {
                    w.Weigth++;
                }

                w.Weigth += Regex.Matches(w.Content, @"\d{2,4}年\d{1,2}月\d{1,2}日", RegexOptions.IgnoreCase).Count;
                w.Weigth += Regex.Matches(w.Content, @"\d{2,4}/\d{1,2}/\d{1,2}", RegexOptions.IgnoreCase).Count;
                w.Weigth += Regex.Matches(w.Content, @"\d{2,4}-\d{1,2}-\d{1,2}", RegexOptions.IgnoreCase).Count;
            }

            string time_result = "";
            int t_weight = 0;
            foreach (ContentAndWeight w in w_time)
            {
                if (w.Weigth > t_weight)
                {
                    time_result = w.Content;
                    t_weight = w.Weigth;
                }
            }
            time_result = Regex.Match(time_result, @"(?<key>\d{2,4}年\d{1,2}月\d{1,2}日|\d{2,4}/\d{1,2}/\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2})", RegexOptions.IgnoreCase).Groups[0].Value;


            topic.time = time_result.ToDateTime();

            #endregion

            #region 获取正文  取消
            ////获取正文

            //List<string> content = new List<string>();
            //List<ContentAndWeight> w_content = new List<ContentAndWeight>();


            //content.Merge(new Regex("<td.*?>(?<key>[\\w\\W]*?)</td>", RegexOptions.IgnoreCase).Match(Html).GetMatchList("key"));
            //content.Merge(Html.GetElementByTagName("div"));



            //foreach (string str in content)
            //{
            //    ContentAndWeight w = new ContentAndWeight();
            //    w.Content = str;
            //    w.Weigth = 0;
            //    w_content.Add(w);
            //}



            //foreach (ContentAndWeight cw in w_content)
            //{
            //    //直接否定
            //    if (cw.Content.Trim().Length < 5)
            //    {
            //        cw.Weigth = -100;
            //        continue;
            //    }

            //    string cw_lower = cw.Content.ToLower();

            //    if (cw_lower.CountString("<p>") > 0)
            //    {
            //        cw.Weigth++;
            //    }
            //    if (cw_lower.CountString("<br") > 0)
            //    {
            //        cw.Weigth++;
            //    }
            //    if (cw_lower.CountString("，") + cw_lower.CountString("。") > 5)
            //    {
            //        cw.Weigth++;
            //    }
            //    if (cw_lower.CountString("，") + cw_lower.CountString("&nbsp;") > 5)
            //    {
            //        cw.Weigth++;
            //    }
            //    if (cw_lower.CountString("class=\"clear\"") > 0)
            //    {
            //        cw.Weigth++;
            //    }




            //    //标签密度
            //    int Tag= 100000 * Regex.Match(cw.Content, "<.*?>").Groups.Count / cw.Content.Length;

            //    if (Tag > 800)
            //    {
            //        cw.Weigth -= 2;
            //    }
            //    if (Tag > 400)
            //    {
            //        cw.Weigth -= 1;
            //    }

            //    //文本密度
            //    int Context = 1000 * cw.Content.Length / cw.Content.Length;
            //    if (Context < 5)
            //    {
            //        cw.Weigth--;
            //    }
            //    if (Context > 20)
            //    {
            //        cw.Weigth ++;
            //    }
            //    if (Context > 10)
            //    {
            //        cw.Weigth++;
            //    }

            //    if (cw.Content.CountString("上一篇") + cw.Content.CountString("下一篇") > 0)
            //    {
            //        cw.Weigth = -100;
            //    }

            //    if (cw.Content.CountString("版权所有") + cw.Content.CountString("copyright") + cw.Content.CountString("icp备") > 0)
            //    {
            //        cw.Weigth--;
            //    }

            //    if (cw.Content.CountString("声明") > 0)
            //    {
            //        cw.Weigth--;
            //    }
            //    if (cw.Content.CountString("<li") > 0)
            //    {
            //        cw.Weigth--;
            //    }
            //}




            //string content_result = "";
            //int c_weight = -50;
            //foreach (ContentAndWeight cw in w_content)
            //{
            //    if (cw.Weigth > c_weight)
            //    {
            //        content_result = cw.Content;
            //        c_weight = cw.Weigth;
            //    }
            //}


            ////content_result = content_result.TrimHTML();
            //content_result = content_result.Replace("　　", "\n");
            //topic.Content = content_result;


            #endregion

            string _Content = Regex.Replace(Html, "&.*?;", " ");
            _Content = Regex.Replace(_Content, "<[/]?a.*?>", "");
            //_Content = Regex.Replace(_Content, @"<td.*?></td>", "");
            //_Content = Regex.Replace(_Content, @"<tr.*?>\\s</tr>", "");
            //_Content = Regex.Replace(_Content, "<th.*?>[\\w\\W].*?</th>", "");
            //_Content = Regex.Replace(_Content, "<table.*?>\\s</table>", "");
            _Content = Regex.Replace(_Content, "<[/]?font.*?>", "");
            _Content = Regex.Replace(_Content, "<br/?>", "");
            _Content = Regex.Replace(_Content, "</?center>", "");
            _Content = Regex.Replace(_Content, "</?strong>", "");
            _Content = Regex.Replace(_Content, "</?span.*?>", "");
            _Content = Regex.Replace(_Content, "<li.*?>[\\w\\W]*?</li>", "");
            string result = "";
            int notfit = 0;
            Match m_content = new Regex(@"<[pP]{1}.*?>[^<>]{20,}[\w\W]*?[^<>]{20,}</[pP]{1}>", RegexOptions.None).Match(_Content);
            while (m_content.Success)
            {
                notfit = Regex.Matches(m_content.Groups[0].Value, "[a-zA-Z0-9\\s<>\\!\\./\\-\":;=]{20,}").Count;
                if (notfit == 0)
                {
                    result += m_content.Groups[0].Value;
                }
                m_content = m_content.NextMatch();
            }
            if (result.IsNullOrEmpty())//不适用P的情况
            {
                m_content = new Regex(@"<div.*?>[^<>]{20,}[\w\W]*?[^<>]{20,}</div>", RegexOptions.None).Match(_Content.ToLower());
                while (m_content.Success)
                {
                    notfit = Regex.Matches(m_content.Groups[0].Value, "[a-zA-Z0-9\\s<>\\!\\./\\-\":;=]{100,}").Count;
                    if (notfit == 0)
                    {
                        result += m_content.Groups[0].Value;
                    }
                    m_content = m_content.NextMatch();
                }
            }
            if (result.IsNullOrEmpty())//不适用div的情况
            {
                m_content = new Regex(@"<td.*?>[^<>]{20,}[\w\W]*?[^<>]{20,}</td>", RegexOptions.None).Match(_Content.ToLower());
                while (m_content.Success)
                {
                    notfit = Regex.Matches(m_content.Groups[0].Value, "[a-zA-Z0-9\\s<>\\!\\./\\-\":;=]{100,}").Count;
                    if (notfit == 0)
                    {
                        result += m_content.Groups[0].Value;
                    }
                    m_content = m_content.NextMatch();
                }
            }

            topic.Content = result;

            return topic;


        }
        #endregion

        #region 根据域名获取网站名
        /// <summary>
        /// 根据域名获取网站名
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string GetSiteNameByDomain(string domain)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("s", domain);

            string html = Post(nvc, "http://tool.chinaz.com/beian.aspx", Encoding.UTF8, new CookieContainer(),
                "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5",
                "http://tool.chinaz.com/beian.aspx",
                "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.204 Safari/534.16");

            if (html.CountString("查询异常，") > 0)
            {
                return GetSiteNameByDomain(domain);
            }
            html = Regex.Replace(html, "[\\s]{1,}", "");
            Match m = new Regex("<tdalign=\"center\">审核时间</td></tr><tralign=\"center\"style=\"background-color:#FFFFFF;\"><tdalign=\"center\">.*?</td><tdalign=\"center\">.*?</td><tdalign=\"center\">.*?</td><tdalign=\"center\">(?<key>.*?)</td>").Match(html);

            if (!m.Success)
            {
                return domain;
            }
            
            return m.Groups["key"].Value;

        }
        #endregion

        #region 下载文件到本地目录
        /// <summary>
        /// 下载文件到本地目录
        /// </summary>
        /// <param name="url">网络文件URL</param>
        /// <param name="path">下载到本地的路径</param>
        public static void DownFile(string url, string path)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(url, path);

        }
        #endregion

        #region WebClient上传文件至服务器
        /// <summary>
        /// WebClient上传文件至服务器
        /// </summary>
        /// <param name="fileNamePath">文件名，全路径格式</param>
        /// <param name="uriString">服务器文件夹路径</param>
        /// <param name="IsAutoRename">是否自动按照时间重命名</param>
        public static void UpLoadFile(string fileNamePath, string uriString, bool IsAutoRename)
        {
            WebClient wc = new WebClient();
            wc.UploadFile(uriString, fileNamePath);

        }
        #endregion

    }

    #region 用户抓取操作返回数据类，如Cookie，HTML代码等
    /// <summary>
    /// 用户抓取操作返回数据类，如Cookie，HTML代码等
    /// </summary>
    public class WebInfo
    {
        /// <summary>
        /// Cookie
        /// </summary>
        public CookieContainer cookieContainer { get; set; }

        /// <summary>
        /// Html源文件
        /// </summary>
        public string Html { get; set; }

        public System.Net.HttpStatusCode statusCode { get; set; }

        public string url { get; set; }

        public CookieCollection cookieCollection { get; set; }

        public string WebUrl { get; set; }
    }
    #endregion

    public class ContentAndWeight
    {
        public int Weigth { get; set; }

        public string Content { get; set; }
    }
}
