using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.Text.RegularExpressions;

namespace Voodoo
{
    /// <summary>
    /// object类型的相关操作
    /// 2010年8月16日 15:43:40 由kuibono 修改
    /// </summary>
    public static class obj
    {

        public static string ListToJsonStr(this IEnumerable<object> lst)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(WS.RequestString("jsoncallback") + "([");
            foreach (object o in lst)
            {
                ArrayList list = new ArrayList();

                PropertyInfo[] fieldinfo = o.GetType().GetProperties();

                foreach (PropertyInfo info in fieldinfo)
                {
                    ListItem listitem = new ListItem(info.Name, info.GetValue(o, null).ToS());
                    list.Add(listitem);
                }
                sb.Append("{");

                for (int i = 0; i < list.Count; i++)
                {

                    ListItem li = (ListItem)list[i];

                    sb.Append("\"" + li.Text.Replace("\"", "\\\"").Replace("'", "\\'") + "\":");
                    sb.Append("\"" + li.Value.Replace("\"", "\\\"").Replace("'", "\\'") + "\"");

                    if (i != list.Count - 1)
                    {
                        sb.Append(",");
                    }

                }
                sb.Append("},");

            }
            sb = sb.TrimEnd(',');

            sb.Append("])");
            return sb.ToS();
        }

        #region 实体转换为JSON
        /// <summary>
        /// 实体转换为JSON,带jsoncallback参数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ToJsonStr(this object model)
        {
            ArrayList list = new ArrayList();

            PropertyInfo[] fieldinfo = model.GetType().GetProperties();

            foreach (PropertyInfo info in fieldinfo)
            {
                ListItem listitem = new ListItem(info.Name, info.GetValue(model, null).ToS());
                list.Add(listitem);
            }

            StringBuilder sb = new StringBuilder();
            if (System.Web.HttpContext.Current.Request.QueryString["jsoncallback"] != null)
            {
                sb.Append(System.Web.HttpContext.Current.Request.QueryString["jsoncallback"] + "(");
            }

            sb.Append("{");

            for (int i = 0; i < list.Count; i++)
            {

                ListItem li = (ListItem)list[i];

                sb.Append("\"" + li.Text.Replace("\"", "\\\"").Replace("'", "\\'") + "\":");
                sb.Append("\"" + li.Value.Replace("\"", "\\\"").Replace("'", "\\'") + "\"");

                if (i != list.Count - 1)
                {
                    sb.Append(",");
                }

            }
            sb.Append("}");
            if (System.Web.HttpContext.Current.Request.QueryString["jsoncallback"] != null)
            {
                sb.Append(")");
            }


            return sb.ToString();

        }

        /// <summary>
        /// 实体转换为JSON,没有jsoncallback参数
        /// </summary>
        /// <returns></returns>
        public static string ToJson(this object model)
        {
            ArrayList list = new ArrayList();
            
            PropertyInfo[] fieldinfo = model.GetType().GetProperties();

            foreach (PropertyInfo info in fieldinfo)
            {
                ListItem listitem = new ListItem(info.Name, info.GetValue(model, null).ToS());
                list.Add(listitem);
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            for (int i = 0; i < list.Count; i++)
            {

                ListItem li = (ListItem)list[i];

                sb.Append("\"" + li.Text.Replace("\"", "\\\"").Replace("'", "\\'") + "\":");
                sb.Append("\"" + li.Value.Replace("\"", "\\\"").Replace("'", "\\'") + "\"");

                if (i != list.Count - 1)
                {
                    sb.Append(",");
                }

            }
            sb.Append("}");


            return sb.ToString();
        }

        public static void ResponseJson(this object model)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write(model.ToJson());
        }
        #endregion

        #region 将数据表转换成JSON类型串
        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">要转换的数据表</param>
        /// <param name="dispose">数据表转换结束后是否dispose掉</param>
        /// <returns></returns>
        public static StringBuilder DataTableToJson(this System.Data.DataTable dt, bool dt_dispose)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[\r\n");

            //数据表字段名和类型数组
            string[] dt_field = new string[dt.Columns.Count];
            int i = 0;
            string formatStr = "{{";
            string fieldtype = "";
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                dt_field[i] = dc.Caption.ToLower().Trim();
                formatStr += "'" + dc.Caption.ToLower().Trim() + "':";
                fieldtype = dc.DataType.ToString().Trim().ToLower();
                if (fieldtype.IndexOf("int") > 0 || fieldtype.IndexOf("deci") > 0 ||
                    fieldtype.IndexOf("floa") > 0 || fieldtype.IndexOf("doub") > 0 ||
                    fieldtype.IndexOf("bool") > 0)
                {
                    formatStr += "{" + i + "}";
                }
                else
                {
                    formatStr += "'{" + i + "}'";
                }
                formatStr += ",";
                i++;
            }

            if (formatStr.EndsWith(","))
                formatStr = formatStr.Substring(0, formatStr.Length - 1);//去掉尾部","号

            formatStr += "}},";

            i = 0;
            object[] objectArray = new object[dt_field.Length];
            foreach (System.Data.DataRow dr in dt.Rows)
            {

                foreach (string fieldname in dt_field)
                {   //对 \ , ' 符号进行转换 
                    objectArray[i] = dr[dt_field[i]].ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'");
                    switch (objectArray[i].ToString())
                    {
                        case "True":
                            {
                                objectArray[i] = "true"; break;
                            }
                        case "False":
                            {
                                objectArray[i] = "false"; break;
                            }
                        default: break;
                    }
                    i++;
                }
                i = 0;
                stringBuilder.Append(string.Format(formatStr, objectArray));
            }
            if (stringBuilder.ToString().EndsWith(","))
                stringBuilder.Remove(stringBuilder.Length - 1, 1);//去掉尾部","号

            if (dt_dispose)
                dt.Dispose();

            return stringBuilder.Append("\r\n]");
        }
        #endregion

        #region 类型转换为SByte
        /// <summary>
        /// 类型转换为SByte
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static SByte ToSByte(this object o)
        {
            try
            {
                return Convert.ToSByte(o);
            }
            catch
            {
                return Convert.ToSByte(0);
            }
            
        }
        #endregion

        #region 类型转换为uint16
        /// <summary>
        /// 类型转换为uint16
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static ushort ToUInt16(this object o)
        {
            try
            {
                return Convert.ToUInt16(o);
            }
            catch
            {
                return ushort.MinValue;
            }
        }
        #endregion

        #region 类型转换为int16
        /// <summary>
        /// 类型转换为uint16
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static short ToInt16(this object o)
        {
            try
            {
                return Convert.ToInt16(o);
            }
            catch
            {
                return short.MinValue;
            }
        }
        #endregion

        #region 对象转换为string，失败返回空字符串
        /// <summary>
        /// 对象转换为string，失败返回空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToS(this object str)
        {
            if (str == null)
            {
                return "";
            }
            try
            {
                return Convert.ToString(str);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region CookieCollection 转换为CookieContainer
        /// <summary>
        ///CookieCollection ×ª»»ÎªCookieContainer
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static CookieContainer ToCookieContainer(this CookieCollection collection)
        {
            CookieContainer cc = new CookieContainer();
            foreach (Cookie cookie in collection)
            {
                cc.Add(cookie);
            }
            return cc;

        }
        #endregion

        /// <summary>
        /// 判断指定CookieCollection是否存在指定的Cookie
        /// </summary>
        /// <param name="cookieCollection"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static bool Exist(this CookieCollection cookieCollection, Cookie cookie)
        {

            foreach (Cookie c in cookieCollection)
            {
                if (c==cookie)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetMatchList(this Match m,string key)
        {
            List<string> result = new List<string>();

            while (m.Success)
            {
                result.Add(m.Groups[key].Value);
                m = m.NextMatch();
            }
            return result;
        }


        public static List<string> Merge(this List<string> str,List<string> newStr)
        {
            foreach (string s in newStr)
            {
                str.Add(s);
            }
            return str;
        }


        //public static DataTable Replace(this DataTable dt, string OldString, string NewString)
        //{
        //    foreach(DataRow row in dt.Rows)
        //    {
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            row[i] = row[i].ToString().Replace(OldString, NewString);
        //        }
        //    }
        //    return dt;
        //}

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class, new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(TResult);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            List<TResult> oblist = new List<TResult>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                TResult ob = new TResult();
                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }

        #region 序列化为xml字符串
        /// <summary>
        /// 序列化为xml字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToXML(this object obj)
        {
            return Voodoo.IO.XML.Serialize(obj);
        }
        #endregion

        #region 反序列化字符串为对象
        /// <summary>
        /// 反序列化字符串为对象
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static object DeSerializeTo(this string str,Type type)
        {
            return Voodoo.IO.XML.DeSerialize(type, str);
        }
        #endregion

        #region 缓存LIST<>
        /// <summary>
        /// 缓存LIST<>
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<TResult> AsCache<TResult>(this IQueryable<TResult> dt) where TResult : class, new()
        {
            string catch_Name = "LIST_" + typeof(TResult).ToS();

            if (Voodoo.Cache.Cache.GetCache(catch_Name) == null)
            {
                Voodoo.Cache.Cache.SetCache(catch_Name,dt.ToList());
            }
            return (List<TResult>)Voodoo.Cache.Cache.GetCache(catch_Name);
        }

        public static List<TResult> AsCache<TResult>(this IQueryable<TResult> dt,string Key) where TResult : class, new()
        {
            string catch_Name = Key;

            if (Voodoo.Cache.Cache.GetCache(catch_Name) == null)
            {
                Voodoo.Cache.Cache.SetCache(catch_Name, dt.ToList());
            }
            return (List<TResult>)Voodoo.Cache.Cache.GetCache(catch_Name);
        }
        #endregion

        public static List<int> ToInt32(this List<string> list)
        {
            List<int> result = new List<int>();
            foreach (string str in list)
            {
                result.Add(str.ToInt32());
            }
            return result;
        }
        public static List<long> ToInt64(this List<string> list)
        {
            List<long> result = new List<long>();
            foreach (string str in list)
            {
                result.Add(str.ToInt64());
            }
            return result;
        }

        
    }
}
