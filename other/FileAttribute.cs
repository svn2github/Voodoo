using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo
{
    /// <summary>
    /// 文件属性
    /// </summary>
    public class FileAttribute
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 是目录
        /// </summary>
        public bool IsDirectory { get; set; }
    }
}
