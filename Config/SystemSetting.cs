using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Voodoo.Config
{
    /// <summary>
    /// 数据库的链接字符串
    /// </summary>
    public class SystemSetting
    {
        #region 主数据库连接字符串
        /// <summary>
        /// 主数据库连接字符串
        /// </summary>
        public static string ForSunConntr
        {
            get
            {
                if (Voodoo.Cache.Cache.GetCache("ForSunConntr") == null)
                {
                    Voodoo.Cache.Cache.SetCache("ForSunConntr", Info.GetAppSetting("ForSunConntr"));
                }
                return Voodoo.Cache.Cache.GetCache("ForSunConntr").ToString();
            }
        }

        /// <summary>
        /// MySql数据库链接
        /// </summary>
        public static string MySqlConnStr
        {
            get
            {
                if (Voodoo.Cache.Cache.GetCache("MySqlConnStr") == null)
                {
                    Voodoo.Cache.Cache.SetCache("MySqlConnStr", Info.GetAppSetting("MySqlConnStr"));
                }
                return Voodoo.Cache.Cache.GetCache("MySqlConnStr").ToString();
            }
        }

        /// <summary>
        /// 数据库类型 分为MSSQL和MYSQL
        /// </summary>
        public static string DbType
        {
            get
            {
                if (Voodoo.Cache.Cache.GetCache("DbType") == null)
                {
                    Voodoo.Cache.Cache.SetCache("DbType", Info.GetAppSetting("DbType"));
                }
                return Voodoo.Cache.Cache.GetCache("DbType").ToString();
            }
        }
        #endregion



    }
}
