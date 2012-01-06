using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo.IO
{
    /// <summary>
    /// 日志输出类
    /// </summary>
    public class Log
    {
        public string Folder { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="folder"></param>
        public Log(string folder)
        {
            Folder = folder;
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="Content"></param>
        public void Debug(string Content)
        {
            File.Write(Folder + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", Content + Environment.NewLine, System.IO.FileMode.Append);
        }

        public void Debug(string Format, params string[] pars)
        {
            string Content = string.Format(Format, pars);
            Debug(Content);
        }
    }
}
