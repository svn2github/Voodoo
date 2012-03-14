using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo.Net.BlogHelper
{
    /// <summary>
    /// 帖子
    /// </summary>
    public class Post
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
