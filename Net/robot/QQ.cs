using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Voodoo.Net.robot
{
    public class QQ
    {
        public static System.Net.CookieContainer c1 = new System.Net.CookieContainer();

        /// <summary>
        /// 判断是否需要输入验证码
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public static string NeedVcode(string Username)
        {
            string url = "http://ptlogin2.qq.com/check?appid=1003903&uin=" + Username;
            //WebInfo info = Url.PostGetCookieAndHtml(new System.Collections.Specialized.NameValueCollection(),
            //    url,
            //    Encoding.UTF8,
            //    new System.Net.CookieContainer(),
            //    "http://web2.qq.com/");

            string resultHtml = Url.GetHtml(url);
            //c1 = info.cookieContainer;

            Match m = new Regex("[,*]{1}'(?<key>.*?)'\\)", RegexOptions.None).Match(resultHtml);

            return m.Groups["key"].Value;
        }

        private static string getVcodeimage(string Username, string code)
        {
            return "http://captcha.qq.com/getimage?aid=1003903&uin=" + Username + "&vc_type=" + code;
        }

        /// <summary>
        /// 登录账号
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="Vcode"></param>
        public static void Login(string Username, string Password, string Vcode)
        {


            string pass = Encrypt(Password, Vcode);

            WebInfo web = Url.PostGetCookieAndHtml(new System.Collections.Specialized.NameValueCollection(),
                "http://ptlogin2.qq.com/login?u=" + Username + "&p=" + pass + "&verifycode=" + Vcode + "&webqq_type=10&remember_uin=1&login2qq=1&aid=1003903&u1=http%3A%2F%2Fweb2.qq.com%2Floginproxy.html%3Flogin2qq%3D1%26webqq_type%3D10&h=1&ptredirect=0&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert&mibao_css=m_webqq",
                Encoding.UTF8,
                c1,
                "http://ui.ptlogin2.qq.com/cgi-bin/login?target=self&style=5&mibao_css=m_webqq&appid=1003903&enable_qlogin=0&no_verifyimg=1&s_url=http%3A%2F%2Fweb2.qq.com%2Floginproxy.html&f_url=loginerroralert&strong_login=1&login_state=10&t=20110615001");
        }










        /// <summary>
        /// 计算网页上QQ登录时密码加密后的结果
        /// </summary>
        /// <param name="pwd" />QQ密码</param>
        /// <param name="verifyCode" />验证码</param>
        /// <returns></returns>
        public static String Encrypt(string pwd, string verifyCode)
        {
            return (md5(md5_3(pwd).ToUpper() + verifyCode.ToUpper())).ToUpper();
        }
        /// <summary>
        /// 计算字符串的三次MD5
        /// </summary>
        /// <param name="s" /></param>
        /// <returns></returns>
        private static String md5_3(String s)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

            bytes = md5.ComputeHash(bytes);
            bytes = md5.ComputeHash(bytes);
            bytes = md5.ComputeHash(bytes);

            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0');
        }
        /// <summary>
        /// 计算字符串的一次MD5
        /// </summary>
        /// <param name="s" /></param>
        /// <returns></returns>
        private static String md5(String s)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

            bytes = md5.ComputeHash(bytes);

            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0');
        }
    }
}
