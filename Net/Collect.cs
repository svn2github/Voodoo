using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Drawing;

namespace Voodoo.Net
{
    public class Collect
    {
        // Fields
        private bool _AutoRedirect = false;
        private CookieCollection _CookieGet = null;
        private CookieCollection _CookiePost = null;
        private Encoding _Encodings = Encoding.GetEncoding("gb2312");
        private string _Err = string.Empty;
        private string _GoUrl = string.Empty;
        private string _PostData = null;
        private WebProxy _ProxyInfo = null;
        private string _Referer = string.Empty;
        private string _ResHtml = string.Empty;
        private Image _ResImg;
        private long _ResImgLen = 0L;
        private string _StatusCode = string.Empty;
        private int _TimeOut = 0x2710;

        // Methods
        private HttpWebRequest CreateRequest()
        {
            HttpWebRequest request2 = null;
            request2 = (HttpWebRequest)WebRequest.Create(this._GoUrl);
            request2.Accept = "*/*";
            request2.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)";
            request2.AllowAutoRedirect = this._AutoRedirect;
            request2.CookieContainer = new CookieContainer();
            request2.Timeout = this._TimeOut;
            request2.Referer = this._Referer;
            if (this._ProxyInfo != null)
            {
                request2.Proxy = this._ProxyInfo;
            }
            if (this._CookiePost != null)
            {
                IEnumerator enumerator=null;
                try
                {
                    enumerator = this._CookiePost.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Cookie current = (Cookie)enumerator.Current;
                        current.Domain = new Uri(this._GoUrl).Host;
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }
                request2.CookieContainer.Add(this._CookiePost);
            }
            if ((this._PostData != null) & (this._PostData.Length > 0))
            {
                request2.ContentType = "application/x-www-form-urlencoded";
                request2.Method = "POST";
                byte[] bytes = this._Encodings.GetBytes(this._PostData);
                request2.ContentLength = bytes.Length;
                Stream requestStream = null;
                try
                {
                    requestStream = request2.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                catch (Exception exception1)
                {
                    //ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    this._Err = exception.Message.ToString();
                    //ProjectData.ClearProjectError();
                }
                finally
                {
                    if (requestStream != null)
                    {
                        requestStream.Close();
                        requestStream = null;
                    }
                }
            }
            return request2;
        }

        public void GetImage()
        {
            HttpWebRequest request = this.CreateRequest();
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                this._ResImg = Image.FromStream(response.GetResponseStream());
                this._ResImgLen = response.ContentLength;
                if (response.Cookies.Count > 0)
                {
                    this._CookieGet = response.Cookies;
                }
            }
            catch (Exception exception1)
            {
                //ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this._Err = exception.Message.ToString();
                //ProjectData.ClearProjectError();
            }
            response = null;
            request = null;
        }

        public void GetString()
        {
            HttpWebResponse response;
            HttpWebRequest request = this.CreateRequest();
            StreamReader reader = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), this._Encodings);
                this._ResHtml = reader.ReadToEnd();
                if (this._AutoRedirect)
                {
                    this._StatusCode = response.StatusCode.ToString();
                    if (this._StatusCode.Equals("302"))
                    {
                        this._ResHtml = response.Headers["location"];
                    }
                }
                if (response.Cookies.Count > 0)
                {
                    this._CookieGet = response.Cookies;
                }
            }
            catch (Exception exception1)
            {
                //ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this._Err = exception.Message.ToString();
                this._ResHtml = "";
                //ProjectData.ClearProjectError();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
            }
            response = null;
            request = null;
        }

        // Properties
        public bool AutoRedirect
        {
            set
            {
                this._AutoRedirect = value;
            }
        }

        public CookieCollection CookieGet
        {
            get
            {
                return this._CookieGet;
            }
        }

        public CookieCollection CookiePost
        {
            get
            {
                return this._CookiePost;
            }
            set
            {
                this._CookiePost = value;
            }
        }

        public Encoding Encodings
        {
            set
            {
                this._Encodings = value;
            }
        }

        public string Err
        {
            get
            {
                return this._Err;
            }
        }

        public string GoUrl
        {
            set
            {
                this._GoUrl = value;
            }
        }

        public string PostData
        {
            set
            {
                this._PostData = value;
            }
        }

        public WebProxy ProxyInfo
        {
            set
            {
                this._ProxyInfo = value;
            }
        }

        public string Referer
        {
            set
            {
                this._Referer = value;
            }
        }

        public string ResHtml
        {
            get
            {
                return this._ResHtml;
            }
            set
            {
                this._ResHtml = value;
            }
        }

        public Image ResImg
        {
            get
            {
                return this._ResImg;
            }
            set
            {
                this._ResImg = value;
            }
        }

        public long ResImgLen
        {
            get
            {
                return this._ResImgLen;
            }
            set
            {
                this._ResImgLen = value;
            }
        }

        public int Timeout
        {
            set
            {
                this._TimeOut = value;
            }
        }

    }
}
