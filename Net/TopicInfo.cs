using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo.Net
{
    /// <summary>
    /// 文章信息类
    /// </summary>
    public class TopicInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime time { get; set; }

        /// <summary>
        /// 新闻来源
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 是否已经发布
        /// </summary>
        public bool Posted { get; set; }

        /// <summary>
        /// 新闻类别
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 文章正文
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文章地址
        /// </summary>
        public string Url { get; set; }

        public string LCLass { get; set; }

        public int RuleID { get; set; }
    }
}
