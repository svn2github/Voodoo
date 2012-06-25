using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo
{

    public static class myDateTime
    {

        #region 静态值
        /// <summary>
        /// 一秒毫秒数
        /// </summary>
        private const double SECONDSMILLISECONDS = 1000;
        /// <summary>
        /// 一分钟毫秒数
        /// </summary>
        private const double MINUTESMILLISECONDS = 60 * SECONDSMILLISECONDS;
        /// <summary>
        /// 一小时毫秒数
        /// </summary>
        private const double HOURMILLISECONDS = 60 * MINUTESMILLISECONDS;
        /// <summary>
        /// 一天毫秒数
        /// </summary>
        private const double DAYMILLISECONDS = 24 * HOURMILLISECONDS;
        #endregion



        #region "获取指定日期，0 20 40分的表名"
        public static string tablename(this System.DateTime dtime)
        {
            string tb = dtime.ToString("HH");
            if (dtime.Minute <= 19)
            {
                tb += "00";
            }

            else if (dtime.Minute >= 40)
            {
                tb += "40";
            }

            else
            {
                tb += "20";
            }
            return tb;
        }

        #endregion

        #region 取指定日期是一年中的第几周
        /// <summary> 
        /// 取指定日期是一年中的第几周 
        /// </summary> 
        /// <param name="dtime">给定的日期</param> 
        /// <returns>数字 一年中的第几周</returns> 
        /// <example>
        /// 
        /// int weekIndex=DateTime.Now.weekofyear();//返回当前日期为今年的第几周
        /// 
        /// </example>
        public static int weekofyear(this System.DateTime dtime)
        {
            int weeknum = 0;
            System.DateTime tmpdate = System.DateTime.Parse(dtime.Year.ToString() + "-1" + "-1");
            DayOfWeek firstweek = tmpdate.DayOfWeek;
            //if(firstweek) 
            for (int i = (int)firstweek + 1; i <= dtime.DayOfYear; i = i + 7)
            {
                weeknum = weeknum + 1;
            }
            return weeknum;
        }
        #endregion

        #region 获取某个月的天数
        /// <summary>
        /// 获取某个月的天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>天数</returns>
        /// <example>
        /// 
        /// int DateTime.Now.MonthDayCount();//获取当前时间的月份有多少天
        /// 
        /// </example>
        public static int MonthDayCount(this System.DateTime date)
        {
            return System.DateTime.DaysInMonth(Convert.ToInt32(date.ToString("yyyy")), Convert.ToInt32(date.ToString("MM")));
        }
        #endregion

        #region 上周的第一天
        /// <summary>
        /// 上周的第一天
        /// </summary>
        /// <returns>上周第一天的日期值</returns>
        public static System.DateTime LastWeekFirstDay()
        {
            return System.DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(System.DateTime.Now.DayOfWeek))) - 7);
        }
        #endregion

        #region 上周的最后一天
        /// <summary>
        /// 上周的最后一天
        /// </summary>
        /// <returns>上周的最后一天的日期值</returns>
        public static System.DateTime LastWeekLastDay()
        {
            return System.DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(System.DateTime.Now.DayOfWeek))) - 7);
        }
        public static System.DateTime LastWeekLastDay(this System.DateTime dt)
        {
            return System.DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(dt.DayOfWeek))) - 7);
        }
        #endregion

        #region 本周第一天
        /// <summary>
        /// 本周第一天
        /// </summary>
        /// <returns>本周第一天的日期值</returns>
        public static System.DateTime ThisWeekFirstDay()
        {
            return System.DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(System.DateTime.Now.DayOfWeek))));
        }
        #endregion

        #region 判断字符串是否2010-2-14~2010-2-23这种格式
        /// <summary>
        /// 判断字符串是否2010-2-14~2010-2-23这种格式
        /// </summary>
        /// <param name="Dates"></param>
        /// <returns>true 或 false</returns>
        public static bool IsDateArea(string Dates)
        {
            try
            {
                string[] dates = Dates.Split('~');
                Convert.ToDateTime(dates[0]);
                Convert.ToDateTime(dates[1]);
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
        #endregion

        #region 判断字符串是否是时间格式
        /// <summary>
        /// 判断字符串是否是时间格式
        /// </summary>
        /// <param name="Dates">要判断的日期</param>
        /// <returns>true 或 false</returns>
        public static bool IsDateTimeArea(string Dates)
        {
            try
            {
                Convert.ToDateTime(Dates);
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
        #endregion

        #region 本周最后一天
        /// <summary>
        /// 本周最后一天
        /// </summary>
        /// <returns>本周最后一天的日期值</returns>
        public static System.DateTime ThisWeekLastDay()
        {
            return System.DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(System.DateTime.Now.DayOfWeek))));
        }
        #endregion

        #region 上个月第一天
        /// <summary>
        /// 上个月第一天
        /// </summary>
        /// <returns>上个月第一天的日期值</returns>
        public static System.DateTime LastMonthFirstDay()
        {
            return System.DateTime.Parse(System.DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);
        }
        #endregion

        #region 上个月最后一天
        /// <summary>
        /// 上个月最后一天
        /// </summary>
        /// <returns>上个月最后一天的日期值</returns>
        public static System.DateTime LastMonthLastDay()
        {
            return System.DateTime.Parse(System.DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1);//这个月1号的前一天
        }
        public static System.DateTime LastMonthLastDay(this System.DateTime dt)
        {
            return System.DateTime.Parse(dt.ToString("yyyy-MM-01")).AddDays(-1);//这个月1号的前一天
        }
        #endregion

        #region 本月第一天
        /// <summary>
        /// 本月第一天
        /// </summary>
        /// <returns>本月第一天的日期值</returns>
        public static System.DateTime ThisMonthFirstDay()
        {
            return System.DateTime.Now.AddDays(-System.DateTime.Now.Day + 1);
        }
        #endregion

        #region 本月最后一天
        /// <summary>
        /// 本月最后一天
        /// </summary>
        /// <returns>本月最后一天的日期值</returns>
        public static System.DateTime ThisMonthLastDay()
        {
            return System.DateTime.Now.AddMonths(1).AddDays(-System.DateTime.Now.Day);
        }
        #endregion

        #region 日期是不是奇数天
        /// <summary>
        /// 日期是不是奇数天
        /// </summary>
        /// <param name="s">要进行判断的日期</param>
        /// <returns>奇数为true 偶数为false</returns>
        /// <remarks>
        /// 该方法并不是用来判断当前的天数是不是奇数，而是判断当前日期距离2010-01-01的天数是不是奇数
        /// </remarks>
        public static bool IsSingleDay(this System.DateTime s)
        {
            return (s - new System.DateTime(2010, 01, 01)).Days % 2 == 1;
        }
        #endregion

        #region 将时间字符串转换为2010/12/5这种格式
        /// <summary>
        /// 将时间字符串转换为2010/12/5这种格式
        /// </summary>
        /// <param name="dateTime">时间字符串</param>
        /// <returns>如：2010/12/5的格式</returns>
        public static string ConvertDateTime(string dateTime)
        {
            DateTime dt = Convert.ToDateTime(dateTime);
            StringBuilder sb_Datetime = new StringBuilder();
            sb_Datetime.Append(dt.Year.ToString().Remove(0, 2));
            sb_Datetime.Append("/");
            sb_Datetime.Append(dt.ToString("MM"));
            sb_Datetime.Append("/");
            sb_Datetime.Append(dt.ToString("dd"));
            return sb_Datetime.ToString();
        }
        #endregion

        #region 时间转换成 7/26这种短格式
        /// <summary>
        /// 时间转换成 7/26这种短格式
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns></returns>
        public static string ToShortChar(this DateTime dt)
        {
            return (dt.Day > 9 ? dt.Day.ToString() : "0" + dt.Day.ToString()) + "/" + (dt.Month > 9 ? dt.Month.ToString() : "0" + dt.Month.ToString());
        }
        #endregion

        #region 获取当前日期为周几
        /// <summary>
        /// 获取当前日期为周几
        /// </summary>
        /// <param name="date">要判断的日期</param>
        /// <returns>返回“周一”、“周二”这种字符</returns>
        public static string GetWeekDayName(this DateTime date)
        {
            int dayOfWeek = date.DayOfWeek.ToInt32();
            switch (dayOfWeek)
            {
                case 0:
                    return "周日";
                    break;
                case 1:
                    return "周一";
                    break;
                case 2:
                    return "周二";
                    break;
                case 3:
                    return "周三";
                    break;
                case 4:
                    return "周四";
                    break;
                case 5:
                    return "周五";
                    break;
                case 6:
                    return "周六";
                    break;
                default:
                    return "";
            }


        }
        #endregion

        #region 时间差显示字符串
        /// <summary>
        /// 时间差显示字符串
        /// </summary>
        /// <param name="st">开始时间</param>
        /// <param name="et">结束时间</param>
        /// <returns>如：刚刚，或者 2小时前</returns>
        public static string GetTimeDifference(DateTime st, DateTime et)
        {
            if (et < st)
                return " 刚刚";

            TimeSpan ts = et - st;

            string value = string.Empty;
            double milliseconds = ts.TotalMilliseconds;
            if (milliseconds >= DAYMILLISECONDS * 15)
            {
                value = st.ToString("yy/MM/dd hh:mm");
            }
            else if (milliseconds >= DAYMILLISECONDS)
            {
                value = ts.Days + " 天前";
            }
            else if (milliseconds >= HOURMILLISECONDS)
            {
                value = ts.Hours + " 小时前";
            }
            else if (milliseconds >= MINUTESMILLISECONDS)
            {
                value = ts.Minutes + " 分钟前";
            }
            else
            {
                value = ts.Seconds + " 秒前";
            }

            return value;
        }
        #endregion

        #region 获取对应日期的农历
        private static string[] TianGan = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        //地支 
        private static string[] DiZhi = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        //十二生肖 
        private static string[] ShengXiao = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        //农历日期 
        private static string[] DayName = {"*","初一","初二","初三","初四","初五", 
                                              "初六","初七","初八","初九","初十", 
                                              "十一","十二","十三","十四","十五", 
                                              "十六","十七","十八","十九","二十", 
                                              "廿一","廿二","廿三","廿四","廿五", 
                                              "廿六","廿七","廿八","廿九","三十"};

        //农历月份 
        private static string[] MonthName = { "*", "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };

        //公历月计数天 
        private static int[] MonthAdd = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
        //农历数据 
        private static int[] LunarData = {2635,333387,1701,1748,267701,694,2391,133423,1175,396438 
                                             ,3402,3749,331177,1453,694,201326,2350,465197,3221,3402 
                                             ,400202,2901,1386,267611,605,2349,137515,2709,464533,1738 
                                             ,2901,330421,1242,2651,199255,1323,529706,3733,1706,398762 
                                             ,2741,1206,267438,2647,1318,204070,3477,461653,1386,2413 
                                             ,330077,1197,2637,268877,3365,531109,2900,2922,398042,2395 
                                             ,1179,267415,2635,661067,1701,1748,398772,2742,2391,330031 
                                             ,1175,1611,200010,3749,527717,1452,2742,332397,2350,3222 
                                             ,268949,3402,3493,133973,1386,464219,605,2349,334123,2709 
                                             ,2890,267946,2773,592565,1210,2651,395863,1323,2707,265877};
        /// <summary> 
        /// 获取对应日期的农历 
        /// </summary> 
        /// <param name="dtDay">公历日期</param> 
        /// <returns></returns> 
        public static string GetLunarCalendar(this DateTime dtDay)
        {
            string sYear = dtDay.Year.ToString();
            string sMonth = dtDay.Month.ToString();
            string sDay = dtDay.Day.ToString();
            int year;
            int month;
            int day;
            try
            {
                year = int.Parse(sYear);
                month = int.Parse(sMonth);
                day = int.Parse(sDay);
            }
            catch
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
                day = DateTime.Now.Day;
            }

            int nTheDate;
            int nIsEnd;
            int k, m, n, nBit, i;
            string calendar = string.Empty;
            //计算到初始时间1921年2月8日的天数：1921-2-8(正月初一) 
            nTheDate = (year - 1921) * 365 + (year - 1921) / 4 + day + MonthAdd[month - 1] - 38;
            if ((year % 4 == 0) && (month > 2))
                nTheDate += 1;
            //计算天干，地支，月，日 
            nIsEnd = 0;
            m = 0;
            k = 0;
            n = 0;
            while (nIsEnd != 1)
            {
                if (LunarData[m] < 4095)
                    k = 11;
                else
                    k = 12;
                n = k;
                while (n >= 0)
                {
                    //获取LunarData[m]的第n个二进制位的值 
                    nBit = LunarData[m];
                    for (i = 1; i < n + 1; i++)
                        nBit = nBit / 2;
                    nBit = nBit % 2;
                    if (nTheDate <= (29 + nBit))
                    {
                        nIsEnd = 1;
                        break;
                    }
                    nTheDate = nTheDate - 29 - nBit;
                    n = n - 1;
                }
                if (nIsEnd == 1)
                    break;
                m = m + 1;
            }
            year = 1921 + m;
            month = k - n + 1;
            day = nTheDate;
            //return year + "-" + month + "-" + day;

            if (k == 12)
            {
                if (month == LunarData[m] / 65536 + 1)
                    month = 1 - month;
                else if (month > LunarData[m] / 65536 + 1)
                    month = month - 1;
            }
            //年
            calendar = "";// year + "年";
            //生肖 
            calendar += ShengXiao[(year - 4) % 60 % 12].ToString() + "年 ";
            // //天干 
            calendar += TianGan[(year - 4) % 60 % 10].ToString();
            // //地支 
            calendar += DiZhi[(year - 4) % 60 % 12].ToString() + " ";

            //农历月 
            if (month < 1)
                calendar += "闰" + MonthName[-1 * month].ToString() + "月";
            else
                calendar += MonthName[month].ToString() + "月";

            //农历日 
            calendar += DayName[day].ToString() + "日";

            return calendar;

        }
        public static DateTime YinToDate(string ip)
        {
            string[] sp = ip.Split(' ');
            if (sp.Length < 3)
                return new DateTime();
            else
            {
                ip = sp[2];
                for (int i = 1; i < DayName.Length; i++)
                {
                    ip = ip.Replace(DayName[i], i.ToString("00"));
                }
                for (int i = 1; i < MonthName.Length; i++)
                {
                    ip = ip.Replace(MonthName[i], i.ToString("00"));
                }
                string d = DateTime.Now.Year.ToString() + "-" + ip.Replace("月", "-").Replace("日", "").Replace("闰", "");
                //throw new Exception(d);
                return DateTime.Parse(d);
            }

        }
        #endregion

        #region 获取Unix时间戳
        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <returns></returns>
        public static double GetUnixTimestamp()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = DateTime.UtcNow - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static double GetUnixTime()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = DateTime.UtcNow - origin;
            return Math.Floor(diff.TotalSeconds);

        }
        #endregion

        #region 转换为Unix时间戳
        /// <summary>
        /// 转换为Unix时间戳
        /// </summary>
        /// <param name="dt">要转换的时间</param>
        /// <returns></returns>
        public static double ToUnixTimestamp(this DateTime dt)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = dt - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        #endregion

        #region char类型的时间转换成标准时间
        /// <summary>
        /// char类型的时间转换成标准时间
        /// </summary>
        /// <param name="date">输入如“20100523”类型字符串</param>
        /// <returns></returns>
        /// <example>
        /// 
        /// DateTime myDate="20100513".DateTimeCharToDateTime();
        /// 
        /// </example>
        public static DateTime DateTimeCharToDateTime(this string date)
        {
            if (!date.IsNumeric())
            {
                return DateTime.MinValue;
            }

            return new DateTime(date.Substring(0, 4).ToInt32(), date.Substring(4, 2).ToInt32(), date.Substring(6, 2).ToInt32());
        }
        #endregion

        #region 天干地支相关
        public static string TianGanOfYear(this DateTime tm)
        {
            string Arr = "甲乙丙丁戊己庚辛壬癸";
            return Arr[(tm.Year - 4) % 10].ToS();
        }

        public static string TianGanOfMonth(this DateTime tm)
        {
            string Arr = "甲乙丙丁戊己庚辛壬癸";
            return Arr[((tm.Year - 4) * 2 + tm.Month) % 10].ToS();
        }

        public static string TianGanOfDay(this DateTime tm)
        {
            string Arr = "癸甲乙丙丁戊己庚辛壬";
            return Arr[(4 * (tm.Year / 100) + tm.Year / 100 / 4 + 5 * (tm.Year % 100) + tm.Year % 100 / 4 + 3 * (tm.Month + 1) / 5 + tm.Day - 3) % 10].ToS();

        }

        public static string DiZhiOfYear(this DateTime tm)
        {
            string Arr = "子丑寅卯辰巳午未申酉戌亥";
            if (tm.Year < 2000)
            {
                return Arr[(tm.Year % 100) % 12].ToS();
            }
            else
            {
                return Arr[(tm.Year % 100 + 4) % 12].ToS();
            }
        }
        public static string DizhiOfMonth(this DateTime tm)
        {
            string Arr = "丑寅卯辰巳午未申酉戌亥子";
            return Arr[tm.Month - 1].ToS();
        }
        public static string DizhiOfDay(this DateTime tm)
        {
            string Arr = "亥子丑寅卯辰巳午未申酉戌";

            int C = tm.Year / 100;
            int y = tm.Year % 100;
            int M = tm.Month;
            int d = tm.Day;
            int i = tm.Month % 2 == 0 ? 6 : 0;

            int Z = 8 * C + (C / 4) + 5 * y + (y / 4) + (3 * (M + 1) / 5) + d + 7 + i;

            return Arr[Z % 12].ToS();
        }

        public static string ShuXiangOfYear(this DateTime tm)
        {
            string Arr = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
            if (tm.Year < 2000)
            {
                return Arr[(tm.Year % 100) % 12].ToS();
            }
            else
            {
                return Arr[(tm.Year % 100 + 4) % 12].ToS();
            }
        }
        #endregion

        #region 获取星座
        /// <summary>
        /// 获取星座
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetConstellations(this DateTime dt)
        {
            string[] Constellations = new string[] { "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座", "摩羯座", "水瓶座", "双鱼座" };
            int i = Convert.ToInt32(dt.ToString("MMdd"));
            int j;
            if (i >= 321 && i <= 419)
                j = 0;
            else if (i >= 420 && i <= 520)
                j = 1;
            else if (i >= 521 && i <= 621)
                j = 2;
            else if (i >= 622 && i <= 722)
                j = 3;
            else if (i >= 723 && i <= 822)
                j = 4;
            else if (i >= 823 && i <= 922)
                j = 5;
            else if (i >= 923 && i <= 1023)
                j = 6;
            else if (i >= 1024 && i <= 1121)
                j = 7;
            else if (i >= 1122 && i <= 1221)
                j = 8;
            else if (i >= 1222 || i <= 119)
                j = 9;
            else if (i >= 120 && i <= 218)
                j = 10;
            else if (i >= 219 && i <= 320)
                j = 11;
            else
            {
                return "未知星座";
            }
            return Constellations[j];
        }
        #endregion

        #region 获取幸运石
        /// <summary>
        /// 获取幸运石
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetBirthStone(this DateTime dt)
        {
            string[] BirthStones  = new string[] { "钻石", "蓝宝石", "玛瑙", "珍珠", "红宝石", "红条纹玛瑙", "蓝宝石", "猫眼石", "黄宝石", "土耳其玉", "紫水晶", "月长石，血石" };
            int i = Convert.ToInt32(dt.ToString("MMdd"));
            int j;
            if (i >= 321 && i <= 419)
                j = 0;
            else if (i >= 420 && i <= 520)
                j = 1;
            else if (i >= 521 && i <= 621)
                j = 2;
            else if (i >= 622 && i <= 722)
                j = 3;
            else if (i >= 723 && i <= 822)
                j = 4;
            else if (i >= 823 && i <= 922)
                j = 5;
            else if (i >= 923 && i <= 1023)
                j = 6;
            else if (i >= 1024 && i <= 1121)
                j = 7;
            else if (i >= 1122 && i <= 1221)
                j = 8;
            else if (i >= 1222 || i <= 119)
                j = 9;
            else if (i >= 120 && i <= 218)
                j = 10;
            else if (i >= 219 && i <= 320)
                j = 11;
            else
            {
                return "未知幸运石";
            }
            return BirthStones[j];
        }
        #endregion
    }

}
