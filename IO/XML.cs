using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Data;
using System.Xml;
using Voodoo.XML;
using System.Collections;

namespace Voodoo.IO
{
    /// <summary>
    /// XML处理相关类,本类中所有路径请赋予相对路径
    /// 2010年4月9日 15:51:19
    /// </summary>
    public class XML
    {
        private static Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();

        #region 反序列化读取XML实体类型文件内容
        /// <summary>
        /// 反序列化读取XML实体类型文件内容
        /// </summary>
        /// <param name="Types">实体类型，例如typeof(Ws_blockSet)</param>
        /// <param name="FilePath">相对地址，例如"~/files.xml"</param>
        /// <returns>返回的实体</returns>
        public static object Read(Type Types, string FilePath)
        {
            if (System.IO.File.Exists(FilePath))
            {
                FileStream stream = null;
                object objectValue = string.Empty;
                stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(Types);
                objectValue = serializer.Deserialize(stream);
                stream.Flush();
                stream.Close();
                return objectValue;
            }
            return null;
        }
        #endregion

        #region 读取XML文件内容,返回DataSet
        /// <summary>
        /// 读取XML文件内容,返回DataSet 
        /// </summary>
        /// <param name="filePath">XML文件的相对地址，例如"~/files.xml"</param>
        /// <returns>返回DataSet </returns>
        public static DataSet Read(string filePath)
        {
            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            DataSet set = new DataSet();
            if (System.IO.File.Exists(filePath))
            {
                set.ReadXml(filePath);
            }
            return set;
        }
        #endregion

        #region 读取Xml文件内容 返回 DataView
        /// <summary>
        /// 读取Xml文件内容 返回 DataView 
        /// </summary>
        /// <param name="Path">XML文件的相对地址，例如"~/files.xml"</param>
        /// <returns>返回 DataView</returns>
        public static DataView ReadView(string filePath)
        {
            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            DataSet set = new DataSet();
            DataView view = null;
            if (System.IO.File.Exists(filePath))
            {
                set.ReadXml(filePath);
                if (set.Tables.Count > 0)
                {
                    view = new DataView(set.Tables[0]);
                }
            }
            return view;
        }
        #endregion

        #region 序列化保存XML实体类型文件
        /// <summary>
        /// 序列化保存XML实体类型文件
        /// </summary>
        /// <param name="Info">要序列化的实体</param>
        /// <param name="FilePath">绝对地址，例如"c:\files.xml"</param>
        public static void SaveSerialize(object Info, string FilePath)
        {
            SaveSerialize(Info.GetType(), Info, FilePath);
        }
        #endregion

        #region 序列化保存XML实体类型文件，重写
        /// <summary>
        /// 序列化保存XML实体类型文件，重写
        /// </summary>
        /// <param name="Types">要序列化的实体类型，例如typeof(Ws_blockSet)</param>
        /// <param name="Info">要序列化的实体</param>
        /// <param name="FilePath">相对地址，例如"~/files.xml"</param>
        public static void SaveSerialize(Type Types, object Info, string FilePath)
        {
            ////FilePath = System.Web.HttpContext.Current.Server.MapPath(FilePath);
            //Stream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            //new XmlSerializer(Types).Serialize(stream, Info);
            //stream.Flush();
            //stream.Close();
            File.Write(FilePath, Serialize(Info));
        }
        #endregion

        public static XmlSerializer GetSerializer(Type t)
        {
            int type_hash = t.GetHashCode();

            if (!serializer_dict.ContainsKey(type_hash))
                serializer_dict.Add(type_hash, new XmlSerializer(t));

            return serializer_dict[type_hash];
        }

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


        /// <summary>
        /// DataTable转换为XML
        /// </summary>
        /// <param name="dt">要进行转换的DataTable</param>
        /// <returns>返回XML源文件</returns>
        public static string DataTableToXML(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<" + dt.TableName + ">");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<item>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append("<" + dt.Columns[i].ColumnName + ">" + row[i].ToString() + "</" + dt.Columns[i].ColumnName + ">");
                }
                sb.Append("</item>");
            }
            sb.Append("</" + dt.TableName + ">");

            return sb.ToS();
        }

        /// <summary>
        /// 将Xml文件转换为DataSet
        /// </summary>
        /// <param name="xmlSource">xml源文件</param>
        /// <returns>返回DataSet结果集</returns>
        public static DataSet CXmlFileToDataSetByXmlSource(string xmlSource)
        {
            DataSet ds = new DataSet();

            StringReader StrStream = null;
            XmlTextReader Xmlrdr = null;

            //读取文件中的字符流
            StrStream = new StringReader(ConvertHtmlToString(xmlSource));
            //获取StrStream中的数据
            Xmlrdr = new XmlTextReader(StrStream);
            //ds获取Xmlrdr中的数据
            ds.ReadXml(Xmlrdr);

            return ds;
     
        }

        /// <summary>
        /// 将HTML文件转换为字符串
        /// </summary>
        /// <param name="html">html源文件</param>
        /// <returns></returns>
        public static string ConvertHtmlToString(string html)
        {
            XmlTextWriter xmlWriter;
            string s;
            using (SgmlReader sgmlReader = new SgmlReader())
            {
                sgmlReader.DocType = "HTML";
                sgmlReader.InputStream = new StringReader(html);
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (xmlWriter = new XmlTextWriter(stringWriter))
                    {
                        while (!sgmlReader.EOF)
                        {
                            xmlWriter.WriteNode(sgmlReader, true);
                        }
                    }
                    s = stringWriter.ToString();
                }
            }


            return s;
        }

        /// <summary>
        /// 将DataSet的Table数据转换为字符串
        /// </summary>
        /// <param name="ds">dataset类型源文件</param>
        /// <returns></returns>
        public static string ConvertDataSetToString(DataSet ds)
        {
            DataTable dt;
            dt = ds.Tables["td"];
            int count = dt.Rows.Count;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<DiskInfo>");
            ArrayList arrayList = new ArrayList();
            int tr_Id = 0;
            int intFlag = 0;
            int j = 0;
            bool blFlag = true;

            for (int i = 0; i < count; i++)
            {
                tr_Id = tr_Id == dt.Rows[i]["tr_Id"].ToInt32() ? tr_Id : dt.Rows[i]["tr_Id"].ToInt32();//判断tr_Id值是否改变，改变说明是下一行的数据


                if (tr_Id == 0)
                {
                    arrayList.Add(dt.Rows[i]["td_Text"].ToString().Trim());
                    ++j;
                }
                else
                {
                    if (i % j == 0)
                    {
                        if (blFlag)
                        {
                            stringBuilder.Append("<item>");
                        }
                        else
                        {
                            stringBuilder.Append("</item>");
                        }
                        blFlag = false;

                        intFlag = 0;
                        if (string.IsNullOrEmpty(dt.Rows[i]["td_Text"].ToString().Trim()))
                        {
                            break;
                        }
                    }
                    stringBuilder.Append("<");
                    stringBuilder.Append(arrayList[intFlag]);
                    stringBuilder.Append(">");
                    stringBuilder.Append(dt.Rows[i]["td_Text"].ToString().Trim());
                    stringBuilder.Append("</");
                    stringBuilder.Append(arrayList[intFlag]);
                    stringBuilder.Append(">");
                    intFlag++;
                }
            }
            stringBuilder.Append("</DiskInfo>");
            return stringBuilder.ToString();
        }

        //// <summary>
        /// 读取Xml文件信息,并转换成DataSet对象
        /// </summary>
        /// <remarks>
        /// DataSet ds = new DataSet();
        /// ds = CXmlFileToDataSet("/XML/upload.xml");
        /// </remarks>
        /// <param name="xmlFilePath">Xml文件地址</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlFileToDataSet(string xmlFilePath)
        {
            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                string path = xmlFilePath;
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    //根据地址加载Xml文件
                    xmldoc.Load(path);

                    DataSet ds = new DataSet();
                    //读取文件中的字符流
                    StrStream = new StringReader(xmldoc.InnerXml);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }


        //// <summary>
        /// 读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="xmlFilePath">xml文江路径</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDataTable(string xmlFilePath)
        {
            return CXmlFileToDataSet(xmlFilePath).Tables[0];
        }
    }
}
