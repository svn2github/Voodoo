using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo
{
    public static class @bool
    {
        #region 将布尔类型的状态转换为“是”、“否”的字符串
        /// <summary>
        /// 将布尔类型的状态转换为“是”、“否”的字符串
        /// </summary>
        /// <param name="b">true或者false</param>
        /// <example>
        /// bool IsGood=false;<br/>
        /// string str_Good=IsGood.ToChinese();//结果为“否”
        ///  </example>
        public static string ToChinese(this bool b)
        {
            if (b)
            {
                return "是";
            }
            return "否";
        }
        #endregion
    }
}
