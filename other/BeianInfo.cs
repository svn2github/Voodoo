using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo
{
    public class BeianInfo
    {
        /// <summary>
        /// 主办单位名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 主办单位性质
        /// </summary>
        public string CompanyType { get; set; }

        /// <summary>
        /// 备案号
        /// </summary>
        public string BeianNumber { get; set; }

        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 首页地址
        /// </summary>
        public string IndexUrl { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AutitTime { get; set; }
    }
}
