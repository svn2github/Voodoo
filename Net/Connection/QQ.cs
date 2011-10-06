using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Voodoo.Net.Connection
{
    public class QQ
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
        public QQ(string userName, string passWord)
        {
            this.UserName = userName;
            this.PassWord = passWord;
        }

        public Result Login()
        {
            string loginUrl = "http://pt.3g.qq.com/";
            string html = Url.PostGetCookieAndHtml(new NameValueCollection(), loginUrl, Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "").Html;

            string url = html.FindText("action=\"", "\"").Replace("&amp;", "&");

            NameValueCollection nv = new NameValueCollection();
            nv.Add("login_url", "http://pt5.3g.qq.com/s?aid=nLogin");
            nv.Add("q_from", "");
            nv.Add("loginTitle", "手机腾讯网");
            nv.Add("bid", "0");
            nv.Add("qq", this.UserName);
            nv.Add("pwd", this.PassWord);
            nv.Add("loginType", "3");
            nv.Add("loginsubmit", "登录");

            WebInfo web = Url.PostGetCookieAndHtml(nv, url, Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), loginUrl);

            web = web.Click(">星座</a> <a href=\"(?<key>.*?)\">微博</a>", Encoding.UTF8);

            Result r = new Result();
            if (web.Html.Contains("退出</a>"))
            {
                r.Success = true;
                this.userCookie = web;
            }
            else
            {
                r.Success = false;

            }
            return r;

            //web = web.Click("</strong>&#160;<a href=\"(?<key>.*?)\">个人资料</a>", Encoding.UTF8);



        }

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public Contact GetUserInfo()
        {
            WebInfo web = userCookie.Click("</strong>&#160;<a href=\"(?<key>.*?)\">个人资料</a>", Encoding.UTF8);

            Contact c = new Contact();

            //姓名
            c.Name = web.Html.FindText("个人信息[\\s\\S]*?<div class=\"cont-lists guset-br\">", "&nbsp;@").Trim();

            //昵称
            c.NickName = web.Html.FindText("&nbsp;@", "<br/>").Trim();

            //头像
            c.Photo = web.Html.FindText("头像[\\s\\S]*?<img src=\"", "\" alt=\"头像\"/>").Trim();

            //性别
            string[] other_Info = web.Html.FindText("个人信息", "我的勋章馆").FindText("<br/>", "<br/>").Trim().Split('/');

            string str_sex = other_Info[0];
            switch (str_sex)
            {
                case "男":
                    c.Sex = e_Sex.男;
                    break;
                case "女":
                    c.Sex = e_Sex.女;
                    break;
                default:
                    c.Sex = e_Sex.保密;
                    break;
            }

            //地址
            if (other_Info.Length>1)
            {
                string[] location = other_Info[1].Split('.');

                Address a = new Address();
                a.Province = location[0];
                if (location.Length>1)
                {
                    a.City = location[1];
                }

                a.address = other_Info[1];
            }

            //qq
            Messanger m = new Messanger();
            m.MessagnerType = e_Messanger.QQ;
            m.MessangerNumber = this.UserName;
            c.Messanger = new List<Messanger>();
            c.Messanger.Add(m);

            

            return c;

        }
        #endregion
    }
}
