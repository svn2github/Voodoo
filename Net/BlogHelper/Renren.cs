using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Specialized;

using System.Net;
namespace Voodoo.Net.BlogHelper
{
    public class Renren
    {
        #region 公有变量
        public CookieContainer cookieContainer { get; set; }

        protected string UserName { get; set; }

        protected string Password { get; set; }

        #endregion

        #region 实例化
        public Renren(CookieContainer cc)
        {
            this.cookieContainer = cc;
        }

        public Renren(string Username, string password)
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
            nv.Add("autoLogin", "true");
            nv.Add("domain", "renren.com");
            nv.Add("email", "kuibono@163.com");
            nv.Add("icode", "");
            nv.Add("key_id", "1");
            nv.Add("origURL", "http://www.renren.com/home");
            nv.Add("password", "123456");

            var lResult=Voodoo.Net.Url.PostGetCookieAndHtml(nv,
                "http://www.renren.com/PLogin.do",
                Encoding.UTF8,
                new CookieContainer());
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

                var fResult = Voodoo.Net.Url.GetHtml("http://blog.renren.com/NewEntry.do", "utf-8", this.cookieContainer);

                NameValueCollection nv = fResult.SerializeForm("#editorForm");

                nv["title"] = Title;
                nv["body"] = Title;

                var pResult = Voodoo.Net.Url.PostGetCookieAndHtml(nv,
                    "http://blog.renren.com/NewEntry.do",
                    Encoding.UTF8,
                    this.cookieContainer);

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
