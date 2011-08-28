using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo
{
    public static class myDouble
    {
        #region 精确到小数点后digit位
        /// <summary>
        /// 精确到小数点后digit位
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digit">小数点后的位数</param>
        /// <returns></returns>
        public static string ToFix(this decimal value, int digit)
        {
            return decimal.Round(decimal.Parse(value.ToString()), digit).ToString();
        }
        #endregion

        #region 转换成百分数
        /// <summary>
        /// 转换成百分数
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <param name="fixedNum">精确到小数点后的位数</param>
        /// <returns></returns>
        public static string ToPercent(this double num,int fixedNum)
        {
            return Convert.ToDecimal(num * 100).ToFix(fixedNum).ToString() + "%";
        }
        #endregion

        #region 转换成百分数
        /// <summary>
        /// 转换成百分数
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <param name="fixedNum">精确到小数点后的位数</param>
        /// <returns></returns>
        public static string ToPercent(this decimal num, int fixedNum)
        {
            return Convert.ToDecimal(num * 100).ToFix(fixedNum).ToString() + "%";
        }


        #endregion

        #region 转换成千分数
        /// <summary>
        /// 转换成千分数
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <param name="fixedNum">精确到小数点后的位数</param>
        /// <returns></returns>
        public static string ToTcent(this double num, int fixedNum)
        {
            return Convert.ToDecimal(num * 1000).ToFix(fixedNum).ToString() + "‰";
        }
        #endregion

        #region 转换成千分数
        /// <summary>
        /// 转换成千分数
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <param name="fixedNum">精确到小数点后的位数</param>
        /// <returns></returns>
        public static string ToTcent(this decimal num, int fixedNum)
        {
            return Convert.ToDecimal(num * 1000).ToFix(fixedNum).ToString() + "‰";
        }


        #endregion


    }
}
