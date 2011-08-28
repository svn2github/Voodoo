using System;

namespace Voodoo
{
    public class Result
    {
        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 成功或失败个数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// URL地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 联系人
        /// </summary>
        public Contact Contact { get; set; }
    }
}
