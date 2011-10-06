using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Voodoo.Net.Connection
{
    public class RenRen
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
        public RenRen(string userName, string passWord)
        {
            this.UserName = userName;
            this.PassWord = passWord;
        }

        public Result Login()
        {
            return log();
        }

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public Contact GetUserInfo()
        {
            string html = this.userCookie.Html;
            Match u1 = new Regex("首页</a>&nbsp;\\|<a href=\"(?<key>.*?)\">个人主页</a", RegexOptions.None).Match(html);
            if (u1.Success)
            {
                html = Url.PostGetCookieAndHtml(new NameValueCollection(), u1.Groups["key"].Value.Replace("&amp;", "&"), Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "").Html;
                u1 = new Regex("></a></td><td><a href=\"(?<key>.*?)\">详细资料</a", RegexOptions.None).Match(html);
                if (u1.Success)
                {
                    html = Url.PostGetCookieAndHtml(new NameValueCollection(), u1.Groups["key"].Value.Replace("&amp;", "&"), Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "").Html;
                }

            }

            Contact c = new Contact();
            c.WebSite = new List<string>();
            c.Messanger = new List<Messanger>();
            c.Phone = new List<PhoneNumber>();

            c.NickName = this.UserName;

            c.Email = this.UserName;

            //姓名
            Match m_ChineseName = new Regex("<title>[\\s\\S]*?-(?<key>.*?)</title>", RegexOptions.None).Match(html);
            if (m_ChineseName.Success)
            {
                c.Name = m_ChineseName.Groups["key"].Value.Trim();
            }

            //头像
            c.Photo = html.FindText("<img width=\"100\" align=\"center\" alt=\"\" src=\"", "\"></a></td></tr>");

            //性别
            Match m_Sex = new Regex("性别：(?<key>.*?)<br />", RegexOptions.None).Match(html);
            if (m_Sex.Success)
            {
                string sex = m_Sex.Groups["key"].Value.Trim();
                switch (sex)
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
            }

            //生日
            Match m_Birth = new Regex("生日：(?<key>.*?)<", RegexOptions.None).Match(html);
            if (m_Birth.Success)
            {
                string bitrh = m_Birth.Groups["key"].Value.Replace("年", "-").Replace("月", "-").Replace("日", "");
                c.BirthDay = bitrh.ToDateTime();
            }

            //地址
            Match m_Address = new Regex("家乡：(?<key>.*?)<br />", RegexOptions.None).Match(html);
            if (m_Address.Success)
            {
                Address a = new Address();
                a.AddressType = e_AddressType.老家;
                a.Country = "中国";
                a.address = m_Address.Groups["key"].Value;

                c.Address = a;
            }

            //QQ
            Match m_QQ = new Regex("QQ：(?<key>.*?)<br />", RegexOptions.None).Match(html);
            if (m_QQ.Success)
            {
                Messanger qq = new Messanger();
                qq.MessagnerType = e_Messanger.QQ;
                qq.MessangerNumber = m_QQ.Groups["key"].Value;

                c.Messanger.Add(qq);
            }

            //MSN
            Match m_MSN = new Regex("MSN：(?<key>.*?)<br />", RegexOptions.None).Match(html);
            if (m_MSN.Success)
            {
                Messanger msn = new Messanger();
                msn.MessagnerType = e_Messanger.MSN;
                msn.MessangerNumber = m_MSN.Groups["key"].Value;

                c.Messanger.Add(msn);
            }

            //手机
            Match m_Mobile = new Regex("手机号码：(?<key>.*?)<br />", RegexOptions.None).Match(html);
            if (m_Mobile.Success)
            {
                PhoneNumber p = new PhoneNumber();
                p.PhoneType = e_PhoneType.手机;
                p.Number = m_Mobile.Groups["key"].Value;

                c.Phone.Add(p);
            }

            //个人网站
            Match m_Site = new Regex("个人网站：(?<key>.*?)</div>", RegexOptions.None).Match(html);
            if (m_Site.Success)
            {
                c.WebSite.Add(m_Site.Groups["key"].Value);
            }
            return c;

        }
        #endregion

        #region 用户登录，私有
        /// <summary>
        /// 用户登录，私有
        /// </summary>
        /// <returns></returns>
        protected Result log()
        {
            string url = "http://3g.renren.com/login.do?autoLogin=true";

            NameValueCollection nv = new NameValueCollection();
            nv.Add("email", this.UserName);
            nv.Add("login", "登录");
            nv.Add("origURL", "");
            nv.Add("password", this.PassWord);

            WebInfo inf = Url.PostGetCookieAndHtml(nv, url, Encoding.UTF8, new System.Net.CookieContainer(), new System.Net.CookieCollection(), "http://3g.renren.com/wlogout.do?htf=734&sid=eLWROt8CIQoOW1YKYP1puu&for5m5&from=");



            Result r = new Result();

            if (inf.Html.Contains("退出"))
            {
                r.Success = true;
                this.userCookie = inf;
            }
            else
            {
                r.Success = false;
            }
            return r;

        }
        #endregion
    }
}
