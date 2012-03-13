using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

using System.Net;

namespace Voodoo.Net.BlogHelper
{
    public class Sina
    {
        #region 公有变量
        public CookieContainer cookieContainer { get; set; }

        protected string UserName { get; set; }

        protected string Password { get; set; }

        #endregion

        #region 实例化
        public Sina(CookieContainer cc)
        {
            this.cookieContainer = cc;
        }

        public Sina(string Username, string password)
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

            string u1 = "http://login.sina.com.cn/sso/prelogin.php?entry=miniblog&callback=sinaSSOController.preloginCallBack&user=" + this.UserName + "&client=ssologin.js(v1.3.12)";
            string h1 = Voodoo.Net.Url.GetHtml(u1).Replace("sinaSSOController.preloginCallBack(", "").Replace(")", "");
            string sina_retcode = h1.FindString("\"retcode\":(?<key>.*?),");
            string sina_servertime = h1.FindString("\"servertime\":(?<key>.*?),");
            string sina_pcid = h1.FindString("\"pcid\":\"(?<key>.*?)\",");
            string sina_nonce = h1.FindString("\"nonce\":\"(?<key>.*?)\"");

            //开始wsse加密
            string p1 = Voodoo.Security.Encrypt.SHA1(this.Password);
            p1 = Voodoo.Security.Encrypt.SHA1(p1);
            p1 = Voodoo.Security.Encrypt.SHA1(p1 + sina_servertime + sina_nonce);

            //准备登陆
            string loginUrl = "http://login.sina.com.cn/sso/login.php?client=ssologin.js(v1.3.12)";
            NameValueCollection nv = new NameValueCollection();

            nv.Add("service", "miniblog");
            nv.Add("client", "ssologin.js%28v1.3.12%29");
            nv.Add("entry", "miniblog");
            nv.Add("encoding", "utf-8");
            nv.Add("gateway", "1");
            nv.Add("savestate", "0");
            nv.Add("useticket", "1");
            nv.Add("username", this.UserName);
            nv.Add("servertime", sina_servertime);
            nv.Add("nonce", sina_nonce);
            nv.Add("pwencode", "wsse");
            nv.Add("password", p1);
            nv.Add("url", "http://www.aizr.net/");
            nv.Add("returntype", "META");
            nv.Add("ssosimplelogin", "1");
            string U2 = "http://login.sina.com.cn/sso/login.php?entry=miniblog&gateway=1&from=referer%3Awww_index&savestate=0&useticket=0&su=" + this.UserName.ToEnBase64() + "&service=sso&servertime=" + sina_servertime + "&nonce=" + sina_nonce + "&pwencode=wsse&sp=" + p1 + "&encoding=UTF-8&callback=sinaSSOController.loginCallBack&cdult=3&domain=sina.com.cn&returntype=TEXT&client=ssologin.js(v1.3.19)&_=" + myDateTime.GetUnixTime();
            //开始登陆

            var result = Voodoo.Net.Url.PostGetCookieAndHtml(new NameValueCollection(),
                //loginUrl + "&" + pars,
                U2,
                Encoding.GetEncoding("gbk"),
                new System.Net.CookieContainer(),
                "http://www.sina.com.cn");

            this.cookieContainer = result.cookieContainer;

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
            return Post(Title, Content, "", "1");
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
                var form = Voodoo.Net.Url.PostGetCookieAndHtml(new NameValueCollection(),
                    "http://control.blog.sina.com.cn/admin/article/article_add.php",
                    Encoding.UTF8,
                    this.cookieContainer,
                    "http://www.sina.com.cn"
                    );


                NameValueCollection aha = form.Html.SerializeForm("#editorForm");
                aha["blog_body"] = Content;
                aha["blog_title"] = Title;
                aha["blog_class"] = Class;
                aha["stag"] = Tag;

                var rPost = Voodoo.Net.Url.PostGetCookieAndHtml(aha,
                "http://control.blog.sina.com.cn/admin/article/article_post.php",
                Encoding.UTF8,
                this.cookieContainer,
                "http://control.blog.sina.com.cn/admin/article/article_add.php");

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
