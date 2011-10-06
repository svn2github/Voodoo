using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Voodoo.Net.Connection
{
    public class Sina
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
        public Sina(string userName, string passWord)
        {
            this.UserName = userName;
            this.PassWord = passWord;
        }

        public Result Login()
        {
            string loginUrl = "http://3g.sina.com.cn/prog/wapsite/sso/login_submit.php?rand=1152891893&backURL=http%3A%2F%2Fweibo.cn%2Fdpool%2Fttt%2Fhome.php&backTitle=%D0%C2%C0%CB%CE%A2%B2%A9&vt=4&revalid=2&ns=1";

            string html = Url.PostGetCookieAndHtml(new NameValueCollection(),
                "http://3g.sina.com.cn/prog/wapsite/sso/login.php?ns=1&revalid=2&backURL=http%3A%2F%2Fweibo.cn%2Fdpool%2Fttt%2Fhome.php%3Fs2w%3Dlogin&backTitle=%D0%C2%C0%CB%CE%A2%B2%A9&vt=4",
                Encoding.UTF8,
                new System.Net.CookieContainer(),
                new System.Net.CookieCollection(),
                "http://weobo.cn"
                ).Html;

            NameValueCollection nv = new NameValueCollection();
            nv.Add("mobile", this.UserName);
            nv.Add(html.FindText("<input type=\"password\" name=\"", "\" size=\"30\" value=\"\"/>"), this.PassWord);
            nv.Add("remember", "on");
            nv.Add("backURL", html.FindText("<input type=\"hidden\" name=\"backURL\" value=\"", "\" />"));
            nv.Add("backTitle", html.FindText("<input type=\"hidden\" name=\"backTitle\" value=\"", "\" />"));
            nv.Add("backURL", "http://weibo.cn/dpool/ttt/home.php");
            nv.Add("vk", html.FindText("<input type=\"hidden\" name=\"vk\" value=\"", "\" />"));
            nv.Add("submit", "登录");

            WebInfo web = Url.PostGetCookieAndHtml(nv, loginUrl, Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "http://weibo.cn");

            Result r = new Result();

            if (web.Html.Contains("登录成功"))
            {
                r.Success = true;
                this.userCookie = web.Click("如果没有自动跳转,请<a href=\"(?<key>.*?)\">点击这里</a>", Encoding.UTF8);

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

            WebInfo web = this.userCookie.Click("头像[\\s\\S]*?<a href=\"(?<key>.*?)\">资料", Encoding.UTF8);

            string html = web.Html;
            Contact c = new Contact();

            //头像
            c.Photo = html.FindText("<div class=\"c\">[\\s\\S]*?act=portait[\\s\\S]*?<img src=\"", "\" alt=\"头像\" />");

            //昵称
            c.NickName = html.FindText("昵称</a>:", "<br/>");

            //姓名
            c.Name = c.NickName;

            //性别 
            string str_Sex = html.FindText("性别</a>:", "<br/>");
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

            //地区
            string[] location = html.FindText("地区</a>:", "<br/>").Split(',');
            Address a = new Address();
            a.Province = location[0];
            if (location.Length>1)
            {
                a.City = location[1];
            }
            c.Address = a;

            //生日
            c.BirthDay = html.FindText("生日</a>:", "<br/>").ToDateTime();

            return c;

            //网站
            c.WebSite = new List<string>();
            c.WebSite.Add(html.FindText("博客:", "<br/>"));

            //手机
            c.Phone = new List<PhoneNumber>();
            PhoneNumber p = new PhoneNumber();
            p.PhoneType = e_PhoneType.手机;
            p.Number = html.FindText("手机</a>:", "<br/>");
            if (p.Number!="")
            {
                c.Phone.Add(p);
            }

            //简介
            c.Remark = html.FindText("简介</a>:", "<br/>");

            //Email
            c.Email = this.UserName;

            return c;

        }
        #endregion
    }
}
