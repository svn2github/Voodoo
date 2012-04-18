using System;
using System.Collections.Generic;

namespace Voodoo.Net.XmlRpc
{
    public class methodCall
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string methodName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public List<param> @params { get; set; }
    }

    public class param
    {
        public string value { get; set; }

        public string type { get; set; }
    }

}
