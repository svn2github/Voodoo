using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo.Net
{
    /// <summary>
    /// 采集规则类
    /// </summary>
    public class CollectionRules
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        public string Site { set; get; }

        /// <summary>
        /// 网站域名
        /// </summary>
        public string Domain { set; get; }

        /// <summary>
        /// 标题正则表达式
        /// </summary>
        public string title { set; get; }


        /// <summary>
        /// 正文正则表达式
        /// </summary>
        public string body { set; get; }

        /// <summary>
        /// 新闻更新时间
        /// </summary>
        public string time { set; get; }

        /// <summary>
        /// 网站编码
        /// </summary>
        public string charset { set; get; }
    }
}
