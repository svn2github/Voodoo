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
            //首先访问百度首页 获取几个表单参数
            string h_index = Url.GetHtml("http://m.baidu.com/", "utf-8");

            string ssid = h_index.FindString("ssid=(?<key>.?)/");
            string from = h_index.FindString("/from=(?<key>.?)/");
            string bd_page_type = h_index.FindString("/bd_page_type=(?<key>.?)/");
            string uid = h_index.FindString("/uid=(?<key>.?)/");

            Result r = new Result();
            NameValueCollection nv = new NameValueCollection();
            nv.Add("login_username", UserName);
            nv.Add("login_loginpass", PassWord);
            nv.Add("login_save", "0");
            nv.Add("aaa", "登录");
            nv.Add("login", "yes");
            nv.Add("can_input", "0");
            nv.Add("u", "http://m.baidu.com/?action=login");
            nv.Add("tpl", "");
            nv.Add("tn", "");
            nv.Add("pu", ""); //sz@224_220 
            nv.Add("ssid", ssid);
            nv.Add("from", from);
            nv.Add("bd_page_type", bd_page_type);
            nv.Add("uid", uid); //wiaui_1314587381_5508

            userCookie = Url.PostGetCookieAndHtml(nv, "http://wappass.baidu.com/passport/", Encoding.UTF8);

            if (userCookie.Html.IndexOf("登录成功") > -1)
            {
                r.Success = true;
                //登录成功
                //继续去获取用户信息
            }
            else
            {
                r.Success = false;
            }

            return r;
        }
    }
}
