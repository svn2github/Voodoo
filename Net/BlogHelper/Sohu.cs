using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

using System.Net;

namespace Voodoo.Net.BlogHelper
{
    public class Sohu
    {
        #region 公有变量
        public CookieContainer cookieContainer { get; set; }

        protected string UserName { get; set; }

        protected string Password { get; set; }

        #endregion

        #region 实例化
        public Sohu(CookieContainer cc)
        {
            this.cookieContainer = cc;
        }

        public Sohu(string Username, string password)
        {
            this.UserName = Username;
            this.Password = password;
        }
        #endregion

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            if (this.cookieContainer != null)
            {
                return;
            }

            string url = "http://passport.sohu.com/sso/login.jsp?userid="+this.UserName+"&password="+Voodoo.Security.Encrypt.Md5(this.Password)+"&appid=9999&persistentcookie=1&isSLogin=1&s=" + myDateTime.GetUnixTimestamp() + "&b=6&w=1280&pwdtype=1&v=26";

            var Result = Voodoo.Net.Url.PostGetCookieAndHtml(new NameValueCollection(),
                url,
                Encoding.GetEncoding("GBK"),
                new System.Net.CookieContainer()
                );
            this.cookieContainer = Result.cookieContainer;

        }
        #endregion

        #region  提交
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="Content">内容</param>
        /// <returns></returns>
        public bool Post(string Title, string Content)
        {
            return Post(Title, Content, "", "");
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="Content">内容</param>
        /// <param name="Tag">标签</param>
        /// <param name="Class">分类</param>
        /// <returns></returns>
        public bool Post(string Title, string Content,string Tag,string Class)
        {
            try
            {
                NameValueCollection nv = new NameValueCollection();
                nv.Add("allowComment", "0");
                nv.Add("categoryId", Class);
                nv.Add("content", Content);
                nv.Add("keywords", Tag);
                nv.Add("oper", "art_ok");
                nv.Add("title", Title);
                nv.Add("vcode", "");
                nv.Add("vcodeEn", "");

                var r2 = Voodoo.Net.Url.PostGetCookieAndHtml(nv,
                    "http://i.sohu.com/a/blog/home/entry/save.htm?_input_encode=UTF-8&_output_encode=UTF-8",
                    Encoding.UTF8,
                    this.cookieContainer,
                    "");

                return true;
            }
            catch
            {
                return false;
            }


        }
        #endregion

    }
}
