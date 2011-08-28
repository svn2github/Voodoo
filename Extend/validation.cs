using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Voodoo
{
    /// <summary>
    /// 验证格式的相关类
    /// </summary>
    public static class validation
    {
        #region 验证是否数字
        /// <summary>
        /// 验证是否数字
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsNumeric(this string str)
        {
            return MachRegex(@"^[-]?\d+[.]?\d*$", str);
        }
        #endregion

        #region  字符串是否为日期
        /// <summary>
        /// 字符串是否为日期
        /// </summary>
        /// <param name="s">要进行判断的字符串</param>
        /// <returns>是日期格式:true 不是：false</returns>
        public static bool IsDateTime(this string s)
        {
            try
            {
                Convert.ToDateTime(s);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 验证字符串是否是整数
        /// <summary>
        /// 验证字符串是否是整数
        /// </summary>
        /// <param name="s">要进行验证的字符串</param>
        /// <returns>整数：true  非整数:false</returns>
        public static bool IsInt(this string s)
        {
            try
            {
                Convert.ToInt32(s);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 验证URL
        /// <summary>
        /// 验证URL
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsUrl(this string str)
        {
            return MachRegex(@"^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$", str);
        }
        #endregion

        #region 验证IP地址
        /// <summary>
        /// 验证IP地址
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsIpAddress(this string str)
        {
            return MachRegex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$", str);
        }
        #endregion

        #region 验证邮编
        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsZipCode(this string str)
        {
            return MachRegex(@"^[0-9]{6}$", str);
        }
        #endregion

        #region 验证是否汉字
        /// <summary>
        /// 验证是否汉字
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsChineseChar(this string str)
        {
            return MachRegex(@"^[\u4e00-\u9fa5]{0,}$", str);
        }
        #endregion

        #region 验证是否Email地址
        /// <summary>
        /// 验证是否Email地址
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsEmail(this string str)
        {
            return MachRegex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", str);
        }
        #endregion

        #region 验证是否电话号码
        /// <summary>
        /// 验证是否电话号码
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsTelNumber(this string str)
        {
            return MachRegex(@"^((\d{3,4}-)|\d{3.4}-)?\d{7,8}$", str);
        }
        #endregion

        #region 是否是手机号码
        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="val"></param>
        public static bool IsMobile(this string val)
        {
            return Regex.IsMatch(val, @"^1[358]\d{9}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证英文字母
        /// <summary>
        /// 验证英文字母
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsLatinChar(this string str)
        {
            return MachRegex(@"^[A-Za-z]+$", str);
        }
        #endregion

        #region  身份证有效性验证
        /// <summary>  
        /// 身份证验证  
        /// </summary>  
        /// <param name="Id">身份证号</param>  
        /// <returns></returns>  
        public static bool IsIDCard(this string Id)
        {

            if (Id.Length == 18)
            {

                bool check = CheckIDCard18(Id);

                return check;

            }

            else if (Id.Length == 15)
            {

                bool check = CheckIDCard15(Id);

                return check;

            }

            else
            {

                return false;

            }

        }

        /// <summary>  
        /// 18位身份证验证  
        /// </summary>  
        /// <param name="Id">身份证号</param>  
        /// <returns></returns>  
        private static bool CheckIDCard18(string Id)
        {

            long n = 0;

            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {

                return false;//数字验证  

            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {

                return false;//省份验证  

            }

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");

            System.DateTime time = new System.DateTime();

            if (System.DateTime.TryParse(birth, out time) == false)
            {

                return false;//生日验证  

            }

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');

            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');

            char[] Ai = Id.Remove(17).ToCharArray();

            int sum = 0;

            for (int i = 0; i < 17; i++)
            {

                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());

            }

            int y = -1;

            Math.DivRem(sum, 11, out y);

            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {

                return false;//校验码验证  

            }

            return true;//符合GB11643-1999标准  

        }

        /// <summary>  
        /// 15位身份证验证  
        /// </summary>  
        /// <param name="Id">身份证号</param>  
        /// <returns></returns>  
        private static bool CheckIDCard15(string Id)
        {

            long n = 0;

            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {

                return false;//数字验证  

            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {

                return false;//省份验证  

            }

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");

            System.DateTime time = new System.DateTime();

            if (System.DateTime.TryParse(birth, out time) == false)
            {

                return false;//生日验证  

            }

            return true;//符合15位身份证标准  

        }
        #endregion

        #region 验证用户密码。正确格式为：以字母开头，长度在6~18之间，只能包含字符、数字和下划线。
        /// <summary>
        /// 验证用户密码。正确格式为：以字母开头，长度在6~18之间，只能包含字符、数字和下划线。
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsStandardPassword(this string str)
        {
            return MachRegex(@"^[a-zA-Z]\w{5,17}$", str);
        }
        #endregion

        #region 验证bbs帐号是否符合标准
        /// <summary>
        /// 验证bbs帐号是否符合标准
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsBBSUserName(this String str)
        {
            return MachRegex(@"([_A-Za-z0-9]{6,16})|([\u4E00-\u9FA5_A-Za-z0-9]{3,16})", str);
        }
        #endregion

        #region 对于数据库是否安全，不包含^%&',;=?$\"等字符
        /// <summary>
        /// 对于数据库是否安全，不包含^%&',;=?$\"等字符
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        public static bool IsSQLSafeChar(this string str)
        {
            return !MachRegex(@"[^%&',;=?$\x22]+", str);
        }
        #endregion

        #region 验证字符串是否符合正则表达式MachRegex
        /// <summary>
        /// 验证字符串是否符合正则表达式MachRegex
        /// </summary>
        /// <param name="regex">正则表达式</param>
        /// <param name="str">字符串</param>
        /// <returns>是否符合 true 或者 false</returns>
        private static bool MachRegex(string regex, string str)
        {
            Regex reg = new Regex(regex);
            return reg.IsMatch(str);
        }
        #endregion

        #region 验证是否合法域名
        /// <summary>
        /// 验证是否合法域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsHost(this string url)
        {
            string pattern = @"^[0-9a-zA-Z-]*\.(com\.tw|com\.cn|com\.hk|net\.cn|org\.cn|gov\.cn|ac\.cn|bj\.cn|sh\.cn|tj\.cn|cq\.cn|he\.cn|sx\.cn|nm\.cn|ln\.cn|jl\.cn|hl\.cn|js\.cn|zj\.cn|ah\.cn|fj\.cn|jx\.cn|sd\.cn|ha\.cn|hb\.cn|hn\.cn|gd\.cn|gx\.cn|hi\.cn|sc\.cn|gz\.cn|yn\.cn|xz\.cn|sn\.cn|gs\.cn|qh\.cn|nx\.cn|xj\.cn|tw\.cn|hk\.cn|mo\.cn|com|net|org|biz|info|cn|mobi|name|sh|ac|io|tw|hk|ws|travel|us|tm|cc|tv|la|in|asia|me|net\.ru)$";
            return Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
