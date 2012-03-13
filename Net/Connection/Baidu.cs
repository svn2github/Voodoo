using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Voodoo.Net.Connection
{
    public class Baidu
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
        public Baidu(string userName, string passWord)
        {
            this.UserName = userName;
            this.PassWord = passWord;
        }

        public Result Login()
        {
            string loginUrl = Url.PostGetCookieAndHtml(new NameValueCollection(), "http://m.baidu.com/", Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "")
                .Html
                .FindText("输入法[\\s\\S]*?<a href=\"", "\">登录</a>");

            NameValueCollection nv = new NameValueCollection();
            nv.Add("login_username", this.UserName);
            nv.Add("login_loginpass", this.PassWord);
            nv.Add("login_save", "0");
            nv.Add("aaa", "登录");
            nv.Add("login", "yes");
            nv.Add("can_input", "0");
            nv.Add("u", "");
            nv.Add("tpl", "");
            nv.Add("tn", "");
            nv.Add("pu", "");
            nv.Add("ssid", "0");
            nv.Add("from", "0");
            nv.Add("bd_page_type", "1");
            nv.Add("uid", "");

            WebInfo web = Url.PostGetCookieAndHtml(nv, loginUrl, Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "http://m.baidu.com");


            Result r = new Result();
            if (web.Html.Contains("登录成功"))
            {
                r.Success = true;
                this.userCookie = web;
            }
            else
            {
                r.Success = false;
            }

            this.userCookie = web.Click("<div class=\"ftr\">[\\w\\W]*?<a href=\"(?<key>.*?)\">百度首页</a><br/>", Encoding.UTF8);


            return r;
        }



        public Contact GetUserInfo()
        {
            WebInfo web = userCookie.Click("酷站</a>&nbsp;&nbsp;<a href=\"(?<key>.*?)\">更多</a><br/>", Encoding.UTF8);

            web = web.Click("主题</a>[\\w\\W]*?<a href=\"(?<key>.*?)\">空间</a>", Encoding.UTF8);
            web = web.Click("我的好友</a>\\|<a href=\"(?<key>.*?)\">我的档案</a>", Encoding.UTF8);

            string html = web.Html;

            Contact c = new Contact();

            //头像
            c.Photo = html.FindText("<div class=\"i\"><img src=\"", "\" alt=\"头像\"/>");

            //姓名
            c.Name = html.FindText("<strong>", "</strong>");

            //昵称
            c.NickName = this.UserName;

            //性别
            string str_sex = html.FindText("</strong><br/><br/>", "&#160;&#160;");
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

            //生日
            c.BirthDay = html.FindText("&#160;&#160;", "&#160;").ToDateTime();


            //地址
            Address a = new Address();
            a.AddressType = e_AddressType.老家;
            a.address = html.FindText("出生地:", "<br/>");
            a.Country = "中国";

            string[] arr_add = a.address.Split('-');
            a.Province = arr_add[0];
            if (arr_add.Length>1)
            {
                a.City = arr_add[1];
            }
            if (arr_add.Length>2)
            {
                a.District = arr_add[2];
            }
            c.Address = a;

            c.Remark = html.FindText("个人简介:", "<br/>");

            return c;

        }
    }
}
