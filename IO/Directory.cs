using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Voodoo.IO
{
    public class myDirectory
    {
        #region 设定一个目录（本身及其内文件及子目录中所有内容）为普通文件
        /// <summary>
        /// 设定一个目录（本身及其内文件及子目录中所有内容）为普通文件
        /// 不含任何特殊属性
        /// </summary>
        /// <param name="path">绝对路径</param>
        public static bool SetDirAttrNormal(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists == false)
            {
                return false;
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
            foreach (DirectoryInfo dir1 in dir.GetDirectories())
            {
                dir1.Attributes = FileAttributes.Normal;
                SetDirAttrNormal(dir1.FullName);
            }
            return true;
        }
        #endregion

        #region 删除一个文件夹内的“所有”内容
        /// <summary>
        /// 删除一个文件夹内的“所有”内容
        /// 如果 IsOnlyFile 则文件夹的目录结构被保持，文件被删除（Explore 中查看为“包含n个子文件夹0个文件”）
        /// 否则 该文件夹为全空（Explore 中查看为“包含0个子文件夹0个文件”）
        /// </summary>
        /// <param name="path">文件夹全路径</param>
        /// <param name="IsOnlyFile"></param>
        /// <returns></returns>
        public static bool DeleteChild(string path, bool IsOnlyFile)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists == false)
            {
                return false;
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }
            foreach (DirectoryInfo dir1 in dir.GetDirectories())
            {
                if (IsOnlyFile)
                {
                    DeleteChild(dir1.FullName, IsOnlyFile);
                }
                else
                {
                    SetDirAttrNormal(dir1.FullName);
                    dir1.Delete();
                }
            }
            return true;
        }
        #endregion

        public static bool DeleteDir(string path)
        {
            //DirectoryInfo dir = new DirectoryInfo(path);
            //if (dir.Exists == false)
            //{
            //    return false;
            //}
            //foreach (FileInfo file in dir.GetFiles())
            //{
            //    file.Attributes = FileAttributes.Normal;
            //    file.Delete();
            //}
            //foreach (DirectoryInfo dir1 in dir.GetDirectories())
            //{

            //    DeleteDir(dir1.FullName);
            //    SetDirAttrNormal(dir1.FullName);
            //    dir1.Delete();

            //}
            Directory.Delete(path, true);
            return true;

        }

        /// <summary>
        /// 获取目录中的所有文件（包含子目录）
        /// </summary>
        /// <param name="Path">目录</param>
        /// <returns></returns>
        public static List<string> GetFileList(string Path)
        {
            string PyPath = System.Web.HttpContext.Current.Server.MapPath(Path);
            List<string> result = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(PyPath);
            if (dir.Exists == false)
            {
                return null;
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                result.Add(file.Name);
            }
            List<string> childFiles = new List<string>();
            foreach (DirectoryInfo dir1 in dir.GetDirectories())
            {

                childFiles = GetFileList(Path + "\\" + dir1.Name);
                foreach (string str in childFiles)
                {
                    result.Add(dir1.Name + "/" + str);
                }
            }
            return result;
        }

        #region 根据指定的绝对路径文件名删除一个文件
        /// <summary>
        /// 根据指定的绝对路径文件名删除一个文件
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
                return true;
            }
            return false;
        }
        #endregion

        #region 根据指定的文件全路径获取byte[]内容
        /// <summary>
        /// 根据指定的文件全路径获取byte[]内容
        /// </summary>
        /// <param name="m_fileName">文件全路径</param>
        /// <returns></returns>
        public static byte[] GetBytesByFileName(string m_fileName)
        {
            FileStream fs = System.IO.File.OpenRead(m_fileName);
            byte[] input = new Byte[fs.Length];
            fs.Read(input, 0, (int)(input.Length));
            fs.Close();
            return input;
        }
        #endregion
    }
}
