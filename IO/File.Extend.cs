using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.IO;

namespace Voodoo.IO
{
    public static class FileExt
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="PostFile"></param>
        /// <param name="Path"></param>
        /// <param name="CreateFolder"></param>
        public static void SaveAs(this HttpPostedFile PostFile,string Path, bool CreateFolder)
        {
            DirectoryInfo dir = new FileInfo(Path).Directory;

            if (CreateFolder && dir.Exists == false)
            {
                dir.Create();
            }

            PostFile.SaveAs(Path);
        }
    }
}
