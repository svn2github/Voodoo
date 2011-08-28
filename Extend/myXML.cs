using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Xml.Serialization;
using System.IO;

namespace Voodoo
{
    public static class myXML
    {
        #region Xml文件的nodelist转换为DataTable
        /// <summary>
        /// Xml文件的nodelist转换为DataTable
        /// </summary>
        /// <param name="nodelist"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this XmlNodeList nodelist)
        {
            DataTable dt = new DataTable();

            //为DataTable设置列
            for (int i=0;i<nodelist[0].Attributes.Count;i++)
            {
                DataColumn dc = new DataColumn(nodelist[0].Attributes[i].Name);
                dt.Columns.Add(dc);
            }

            
            for (int i = 0; i < nodelist.Count;i++ )
            {
                DataRow dr = dt.NewRow();//创建一个新的行DataRow
                for (int j = 0; j < nodelist[i].Attributes.Count; j++)//遍历属性，将属性值赋给DataRow
                {
                    dr[j] = nodelist[i].Attributes[j].Value;
                }
                if (nodelist[i].Attributes[0].Value.IsNullOrEmpty())//如果第一个属性值为空，则跳过，继续下一条
                {
                    continue;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion

        #region xml序列化成字符串
        /// <summary>
        /// xml序列化成字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>xml字符串</returns>
        public static string Serialize(object obj)
        {
            string returnStr = "";
            XmlSerializer serializer = GetSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            try
            {
                xtw = new System.Xml.XmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = System.Xml.Formatting.Indented;
                serializer.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                returnStr = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
                if (sr != null)
                    sr.Close();
                ms.Close();
            }
            return returnStr;

        }
        #endregion

        #region  反序列化XML
        /// <summary>
        /// 反序列化XML
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="s">xml源字符串</param>
        /// <returns></returns>
        public static object DeSerialize(Type type, string s)
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(s);
            try
            {
                XmlSerializer serializer = GetSerializer(type);
                return serializer.Deserialize(new MemoryStream(b));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 私有
        /// <summary>
        /// 私有属性
        /// </summary>
        private static Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();

        /// <summary>
        /// 获取序列化对象，私有方法
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static XmlSerializer GetSerializer(Type t)
        {
            int type_hash = t.GetHashCode();

            if (!serializer_dict.ContainsKey(type_hash))
                serializer_dict.Add(type_hash, new XmlSerializer(t));

            return serializer_dict[type_hash];
        }
        #endregion

    }
}
