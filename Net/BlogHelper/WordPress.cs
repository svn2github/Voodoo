using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

using System.Net;

namespace Voodoo.Net.BlogHelper
{
    public class WordPress
    {

        protected string UserName { get; set; }

        protected string Password { get; set; }

        protected string Url { get; set; }

        protected AlexJamesBrown.JoeBlogs.WordPressWrapper wp;
        
        #region 实例化


        public WordPress(string url,string Username, string password)
        {
            this.Url = url;
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
            if (!Url.Contains("xmlrpc.php"))
            {
                Url += "xmlrpc.php";
            }
            wp = new AlexJamesBrown.JoeBlogs.WordPressWrapper(Url, UserName, Password);
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
            return Post(Title, Content, "", "");
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
                wp.NewPost(new AlexJamesBrown.JoeBlogs.Structs.Post()
                {
                    categories = Tag.Split(',', ' ', ';'),
                    dateCreated = DateTime.Now,
                    description = Content,
                    title = Title
                }, true);

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
