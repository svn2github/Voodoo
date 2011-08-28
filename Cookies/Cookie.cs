using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Voodoo.Cookies
{
    public class Cookies
    {
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string key)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[key]!=null)
            {
                return System.Web.HttpContext.Current.Request.Cookies[key];
            }
            return null;
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void SetCookie(HttpCookie cookie)
        {
            System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 删除指定
        /// Cookie
        /// </summary>
        /// <param name="Name"></param>
        public static void Remove(string Name)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[Name] != null)
            {
               System.Web.HttpCookie cookie= System.Web.HttpContext.Current.Request.Cookies[Name];
               cookie.Expires = DateTime.Now.AddDays(-1);
               cookie.Values.Clear();
               System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }

        /// <summary>
        /// 清空Cookie
        /// </summary>
        public static void Clear()
        {
            HttpContext context = HttpContext.Current;

            List<string> cookiesToClear = new List<string>();
            foreach (string cookieName in context.Request.Cookies)
            {
                HttpCookie cookie = context.Request.Cookies[cookieName];
                cookiesToClear.Add(cookie.Name);
            }

            foreach (string name in cookiesToClear)
            {
                HttpCookie cookie = new HttpCookie(name, string.Empty);
                cookie.Expires = DateTime.Today.AddYears(-1);

                context.Response.Cookies.Set(cookie);
            }
        }
    }
}
