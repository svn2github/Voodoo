using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Voodoo.Net.Connection
{
    public class Kaixin001
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
        public Kaixin001(string userName, string passWord)
        {
            this.UserName = userName;
            this.PassWord = passWord;
        }

        public Result Login()
        {
            string loginUrl = "http://wap.kaixin001.com/home/?id=";
            NameValueCollection nv = new NameValueCollection();
            nv.Add("email", this.UserName);
            nv.Add("password", this.PassWord);
            nv.Add("from", "kx");
            nv.Add("refuid", "0");
            nv.Add("refcode", "");
            nv.Add("bind", "");
            nv.Add("gotourl", "");
            nv.Add("from_kx_client", "0");
            nv.Add("login", " 登 录 ");

            WebInfo web = Url.PostGetCookieAndHtml(nv, loginUrl, Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "http://wap.kaixin001.com/");

            Result r = new Result();

            if (web.Html.Contains("退出"))
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

            WebInfo web = this.userCookie.Click("好友</a>&nbsp;<a href=\"(?<key>.*?)\" class=\"tdno\">设置</a>", Encoding.UTF8);
            web = web.Click("显示 <font color=\"#666666\">\\|</font> <a href=\"(?<key>.*?)\">资料</a>", Encoding.UTF8);
            Contact c = new Contact();

            string html=web.Html;
            //姓名
            c.Name = html.FindText("真实姓名：", "<a");

            //Email
            c.Email = this.UserName;

            //性别
            string str_Sex = html.FindText("性&nbsp;&nbsp;&nbsp;&nbsp;别：", "<a");
            switch (str_Sex)
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
            c.BirthDay = html.FindText("生&nbsp;&nbsp;&nbsp;&nbsp;日：", "<a").ToDateTime();

            //城市
            Address a = new Address();
            a.City = html.FindText("居住城市：", "<a");
            if (a.City!="")
            {
                c.Address = a;
            }
            return c;

        }
        #endregion
    }
}
