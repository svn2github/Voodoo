using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

using System.Net;
namespace Voodoo.Net.BlogHelper
{
    public class Neasy
    {
        #region 公有变量
        public CookieContainer cookieContainer { get; set; }

        protected string UserName { get; set; }

        protected string Password { get; set; }

        #endregion

        #region 实例化
        public Neasy(CookieContainer cc)
        {
            this.cookieContainer = cc;
        }

        public Neasy(string Username, string password)
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

            NameValueCollection nv = new NameValueCollection();
            nv.Add("password", this.Password);
            nv.Add("product", "163");
            nv.Add("selected", "");
            nv.Add("type", "1");
            nv.Add("ursname", "");
            nv.Add("username", this.UserName);

            string loginUrl = "https://reg.163.com/logins.jsp";

            var lResult = Voodoo.Net.Url.PostGetCookieAndHtml(nv,
                loginUrl,
                Encoding.UTF8,
                new System.Net.CookieContainer(),
                "http://www.163.com/"
                );
            this.cookieContainer = lResult.cookieContainer;

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
        public bool Post(string Title, string Content, string Tag, string Class)
        {
            try
            {


                var fResult = Voodoo.Net.Url.PostGetCookieAndHtml(new NameValueCollection(),
                "http://blog.163.com/u/" + this.UserName + "/blog/getBlog.do?username=" + this.UserName + "&myMailInfoWrite&fromblogurs",
                Encoding.UTF8,
                this.cookieContainer,
                "http://www.163.com/");

                NameValueCollection aha = fResult.Html.SerializeForm(".ztag");
                aha["HEContent"] = Content;
                aha["tag"] = Tag;
                aha["title"] = Title;


                string u_name = fResult.Html.FindString("userName:'(?<key>.*?)'");
                //发布
                var pResult = Voodoo.Net.Url.PostGetCookieAndHtml(aha,
                    "http://api.blog.163.com/" + u_name + "/editBlogNew.do?p=1&n=1",
                    Encoding.UTF8,
                    this.cookieContainer,
                    "http://blog.163.com");

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
