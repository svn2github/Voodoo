using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;

namespace Voodoo.Config
{
    /// <summary>
    /// Web.Config文件相关操作
    /// </summary>
    public class Info
    {
        #region 获取AppSetting值
        /// <summary>
        /// 获取AppSetting值
        /// </summary>
        /// <param name="Names">节点中的Key 如：<add Key='ConnStr'/></param>
        /// <returns>add 节点中的value值</returns>
        public static string GetAppSetting(string Names)
        {
            return ConfigurationManager.AppSettings[Names];

        }
        #endregion

        #region 设置AppSetting的值
        /// <summary>
        /// 设置AppSetting的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetAppSetting(string key, string value)
        {
            string XPath = "/configuration/appSettings/add[@key='?']";
            XmlDocument domWebConfig = new XmlDocument();

            domWebConfig.Load((HttpContext.Current.Server.MapPath("~/web.config")));
            XmlNode addKey = domWebConfig.SelectSingleNode((XPath.Replace("?", key)));
            if (addKey == null)
            {
                //Response.Write("<script>alert   (\"没有找到<add   key='" + key + "'   value=.../>的配置节\")</script>");
                return;
            }
            addKey.Attributes["value"].InnerText = value;
            domWebConfig.Save((HttpContext.Current.Server.MapPath("~/web.config")));
        }
        #endregion

        #region 获取connectionStrings节点数据
        /// <summary>
        /// 获取connectionStrings节点数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
        #endregion

        #region 根据节点名获取Web.Config文件中的某一节点
        /// <summary>
        /// 根据节点名获取Web.Config文件中的某一节点
        /// </summary>
        /// <param name="Names">节点的名字</param>
        /// <returns>返回整个节点</returns>
        public static object GetSection(string Names)
        {
            return ConfigurationManager.GetSection(Names);
        }
        #endregion
    }
}
