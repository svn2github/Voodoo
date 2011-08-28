using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo
{
    public static class @int
    {
        #region 将用户等级，如10级，转换为两个月亮两个星星这种形式
        /// <summary>
        /// 将用户等级，如10级，转换为两个月亮两个星星这种形式
        /// </summary>
        /// <param name="grade">要转换的级别</param>
        /// <param name="step">进制，如：4（即四个星星为一个月亮）</param>
        /// <returns>返回3,3,2这种形式的字符串，意为：两个太阳一个月亮</returns>
        public static string GradeToSymbol(this int grade,int step)
        {
            string sun = "3,";
            string mon = "2,";
            string sta = "1,";


            string gradeStr = "";

            if (grade <= 0)
            {
                gradeStr = "";
            }
            if (grade >= (step * step))
            {
                int count = grade / (step * step);
                grade = grade % (step * step);
                for (int i = 0; i < count; i++)
                {
                    gradeStr += sun;
                }
            }
            if (grade >= step)
            {
                int count = grade / step;
                grade = grade % step;
                for (int i = 0; i < count; i++)
                {
                    gradeStr += mon;
                }
            }
            if (grade >= 1)
            {
                int count = grade;
                for (int i = 0; i < count; i++)
                {
                    gradeStr += sta;
                }
            }


            return gradeStr;
        }
 #endregion

        #region 通过字节数取得文件大小
        /// <summary>
        /// 通过字节数取得文件大小
        /// </summary>
        /// <param name="fileSize">字节数</param>
        /// <returns>实际大小，如：800MB</returns>
        public static string ToFileSize(this int fileSize)
        {
            string size = "0B";
            if (fileSize < 1024)
            {
                size = fileSize.ToString() + "B";
            }
            else if (fileSize < 1024 * 1024)
            {
                size = Convert.ToString(fileSize / 1024) + "KB";
            }
            else if (fileSize < 1024 * 1024 * 1024)
            {
                size = Convert.ToString(fileSize / (1024 * 1024)) + "MB";
            }
            else //if (fileSize < 1099511627776)
            {
                size = Convert.ToString(fileSize / (1024 * 1024 * 1024)) + "GB";
            }
            return size;
        }
        #endregion

        #region 数字转换成带逗号分割的格式如：100,000,000
        /// <summary>
        /// 数字转换成带逗号分割的格式如：100,000,000
        /// </summary>
        /// <param name="Number">要进行转换的数字</param>
        /// <returns></returns>
        public static string ToLongNum(this int Number)
        {
            return Number.ToString("#,###");
        }
        #endregion

        #region 判断数字是否在数组中
        /// <summary>
        /// 判断数字是否在数组中
        /// </summary>
        /// <param name="number">要判断的数字</param>
        /// <param name="numbers">目标数组</param>
        /// <returns></returns>
        public static bool InArray(this int number, int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (number == numbers[i])
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 产生特定范围内的随机数字
        /// <summary>
        /// 产生特定范围内的随机数字
        /// </summary>
        /// <param name="min">下限</param>
        /// <param name="max">上线</param>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
       #endregion

        #region 获取页数
        /// <summary>
        /// 获取页数
        /// </summary>
        /// <param name="itemCount"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static int GetPageCount(this int itemCount,int PageSize)
        {
            return itemCount % PageSize > 0 ? itemCount / PageSize + 1 : itemCount / PageSize;
        }
        #endregion
    }
}
