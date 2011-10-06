using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Voodoo.Net.Connection
{
    public class Google
    {
        #region 公共变量
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string PassWord { get; set; }
        #endregion

        #region 私有变量


        protected WebInfo userCookie = new WebInfo();

        #endregion

        /// <summary>
        /// 类初始化
        /// </summary>
        /// <param name="userName">账号</param>
        /// <param name="passWord">密码</param>
        public Google(string userName, string passWord)
        {
            this.UserName = userName;
            this.PassWord = passWord;
        }

        public Result Login()
        {
            string loginUrl = "https://accounts.google.com/ServiceLogin?hl=zh-CN&continue=http://www.google.com.hk/";
            WebInfo loginWeb = Url.PostGetCookieAndHtml(new NameValueCollection(), loginUrl, Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "http://www.google.com.hk/");
            string html = loginWeb.Html;

            NameValueCollection nv = new NameValueCollection();
            nv.Add("continue", html.FindText("name=\"continue\" id=\"continue\" value=\"", "\""));
            nv.Add("dnConn", "");
            nv.Add("dsh", html.FindText("name=\"dsh\" id=\"dsh\" value=\"", "\""));
            nv.Add("Email", this.UserName);
            nv.Add("GALX", html.FindText("name=\"GALX\"[\\s\\S]*?value=\"", "\""));
            nv.Add("hl", html.FindText("name=\"hl\" id=\"hl\" value=\"", "\""));
            nv.Add("Passwd", this.PassWord);
            nv.Add("PersistentCookie", "yes");
            nv.Add("pstMsg", "1");
            nv.Add("rmShown", "1");
            nv.Add("secTok", "");
            nv.Add("signIn", "登录");
            nv.Add("timeStmp", "");

            WebInfo web = Url.PostGetCookieAndHtml(nv, "https://accounts.google.com/ServiceLoginAuth", Encoding.UTF8, loginWeb.cookieContainer, loginWeb.cookieCollection, loginUrl);


            Result r = new Result();

            if (web.Html.Contains("转接"))
            {
                r.Success = true;
                this.userCookie = web;
            }
            else
            {
                r.Success = false;
            }

            return r;


        }

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public Contact GetUserInfo()
        {

            WebInfo web = Url.PostGetCookieAndHtml(new NameValueCollection(), "https://plus.google.com/settings/", Encoding.UTF8, userCookie.cookieContainer, userCookie.cookieCollection, "http://www.google.com.hk");


            Contact c = new Contact();

            c.Email = this.UserName;

            //Gtalk
            c.Messanger = new List<Messanger>();
            Messanger gtalk = new Messanger();
            gtalk.MessagnerType = e_Messanger.GTalk;
            gtalk.MessangerNumber = this.UserName;
            c.Messanger.Add(gtalk);

            return c;

        }
        #endregion
    }
}
