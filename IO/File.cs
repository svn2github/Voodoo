using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Voodoo.IO
{
    /// <summary>
    /// 文件处理相关类,本类中所有路径请赋予相对路径
    /// 2010年4月9日 15:51:19 Kuibono创建
    /// </summary>
    public partial class File
    {

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <returns></returns>
        public static bool Delete(string Path)
        {
            try
            {
                FileInfo file = new FileInfo(Path);
                if (file.Exists)
                {
                    file.Delete();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region 文件是否存在
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="Path">文件的相对路径</param>
        /// <returns>true 存在  ，false 不存在</returns>
        public static bool Exists(string Path)
        {
            return System.IO.File.Exists(Path);
        }
        #endregion

        #region 读取文件内容
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="Path">文件的相对路径</param>
        /// <returns></returns>
        public static string Read(string Path)
        {
            string str = string.Empty;
            if (Exists(Path))
            {
                StreamReader reader = new StreamReader(Path, Encoding.GetEncoding("GB2312"));
                str = reader.ReadToEnd();
                reader.Close();
                return str;
            }
            return ("{WsErr}未找到该文件:" + Path + "!");
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="Path">相对路径</param>
        /// <param name="FileEncoding">文件的编码格式如: Encode.GB2312</param>
        /// <returns></returns>
        public static string Read(string Path, EnCode FileEncoding)
        {
            StreamReader reader;
            string str = string.Empty;
            if (!Exists(Path))
            {
                return ("{WsErr}未找到该文件:" + Path + "!");
            }
            switch (FileEncoding)
            {
                case EnCode.GB2312:
                    reader = new StreamReader(Path, Encoding.GetEncoding("GB2312"));
                    break;

                case EnCode.Big5:
                    reader = new StreamReader(Path, Encoding.GetEncoding("Big5"));
                    break;

                case EnCode.UTF8:
                    reader = new StreamReader(Path, Encoding.UTF8);
                    break;

                case EnCode.ASCII:
                    reader = new StreamReader(Path, Encoding.ASCII);
                    break;

                case EnCode.Unicode:
                    reader = new StreamReader(Path, Encoding.Unicode);
                    break;

                default:
                    reader = new StreamReader(Path, Encoding.Default);
                    break;
            }
            str = reader.ReadToEnd();
            reader.Close();
            return str;
        }
        #endregion

        #region 移动文件
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="OldFile">要移动的文件路径（相对）</param>
        /// <param name="NewFile">新的文件路径（相对）</param>
        public static void Move(string OldFile, string NewFile)
        {
            if (Exists(OldFile))
            {
                Delete(NewFile);
                FileInfo file = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(OldFile));
                file.MoveTo(System.Web.HttpContext.Current.Server.MapPath(NewFile));
                //File.Move(System.Web.HttpContext.Current.Server.MapPath(OldFile), System.Web.HttpContext.Current.Server.MapPath(NewFile));
            }
        }
        #endregion

        #region 文件拷贝
        /// <summary>
        /// 文件拷贝
        /// </summary>
        /// <param name="srcFilePath">源文件</param>
        /// <param name="newFilePath">新文件路径</param>
        public static void Copy(string srcFilePath, string newFilePath)
        {
            if (Exists(newFilePath))
            {
                Delete(newFilePath);
            }
            FileInfo file = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(srcFilePath));
            file.CopyTo(System.Web.HttpContext.Current.Server.MapPath(newFilePath), true);

            //设置属性
            FileInfo newF = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(newFilePath));
            newF.Attributes = FileAttributes.Normal;
            newF.Refresh();
        }
        #endregion

        #region 写入文件
        /// <summary>
        /// 写入文件...本功能会自动创建目录和文件
        /// </summary>
        /// <param name="Path">文件路径（相对）</param>
        /// <param name="fileContent">文件内容</param>
        public static void Write(string Path, string fileContent)
        {
            Write(Path, fileContent, "UTF-8");

        }

        public static void Write(string Path, string fileContent, string Encode)
        {
            FileInfo file = new FileInfo(Path);

            if (!Directory.Exists(file.DirectoryName))//目录是否存在
            {
                Directory.CreateDirectory(file.DirectoryName);//不存在则创建
            }



            FileStream stream = null;
            stream = new FileStream(Path, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(Encode));
            writer.Write(fileContent);
            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 在文件尾部写入文本
        /// </summary>
        /// <param name="Path">路径（相对）</param>
        /// <param name="FileName"></param>
        /// <param name="Str"></param>
        public static void AppendLine(string Path, string filename, string Str)
        {
            Path = Path;
            DirectoryInfo df = new DirectoryInfo(Path);
            if (!df.Exists)
            {
                System.IO.Directory.CreateDirectory(Path);
            }

            FileStream stream = null;
            stream = new FileStream(Path + filename, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("gb2312"));
            writer.WriteLine(Str);
            writer.Close();
            stream.Close();
        }


        /// <summary>
        /// 在文件尾部写入文本
        /// </summary>
        /// <param name="Path">路径（相对）</param>
        /// <param name="FileName"></param>
        /// <param name="Str"></param>
        public static void AppendLine(string Path, string fileContent)
        {
            FileInfo file = new FileInfo(Path);
            if (!file.Directory.Exists)
            {
                Directory.CreateDirectory(file.DirectoryName);
            }
            if (!file.Exists)
            {
                file.Create();
            }

            FileStream stream = null;
            stream = new FileStream(Path, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("gb2312"));
            writer.WriteLine(fileContent);
            writer.Close();
            stream.Close();
            writer.Dispose();
            stream.Dispose();
        }




        #endregion



        #region 文件的编码格式
        /// <summary>
        /// 文件的编码格式
        /// </summary>
        public enum EnCode
        {
            ASCII = 3,
            Big5 = 1,
            Defaults = 9,
            GB2312 = 0,
            Unicode = 4,
            UTF8 = 2
        }
        #endregion

    }
}
