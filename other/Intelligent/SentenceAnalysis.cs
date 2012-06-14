using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Voodoo.other.Intelligent
{
    /// <summary>
    /// 语义分析类
    /// </summary>
    public class SentenceAnalysis
    {
        #region 初始化
        protected string Sentense { get; set; }
        public SentenceAnalysis(string _Sentense)
        {
            Sentense = _Sentense;
        }
        #endregion

        #region 句子类型的判断
        /// <summary>
        /// 句子类型的判断
        /// </summary>
        public SentenceType Type
        {
            get
            {
                if (Regex.IsMatch(Sentense, "如何|怎么|什么|怎样|咋|how|何时"))
                {
                    return SentenceType.Question;
                }
                if (Regex.IsMatch(Sentense, "是不是|是否|能不能|能否|可不可以|吗|有没有|对不对"))
                {
                    return SentenceType.TrueFalse;
                }
                if (Regex.IsMatch(Sentense, "麻烦|请|帮我|please"))
                {
                    return SentenceType.Order;
                }
                return SentenceType.Normal;
            }
        }
        #endregion

        public DateTime Time
        {
            get
            {
                var t = new List<LocalTime>();
                t.Add(new LocalTime() { Name = "大前天", Time = DateTime.Now.AddDays(-3) });
                t.Add(new LocalTime() { Name = "前天", Time = DateTime.Now.AddDays(-2) });
                t.Add(new LocalTime() {   Name="昨天", Time=DateTime.Now.AddDays(-1)  });
                t.Add(new LocalTime() { Name = "今天", Time = DateTime.Now });
                t.Add(new LocalTime() { Name = "明天", Time = DateTime.Now.AddDays(1) });
                t.Add(new LocalTime() { Name = "后天", Time = DateTime.Now.AddDays(2) });
                t.Add(new LocalTime() { Name = "大后天", Time = DateTime.Now.AddDays(3) });


                return DateTime.Now;
            }
        }
    }

    public class LocalTime
    {
        public string Name { get; set; }

        public DateTime Time { get; set; }
    }

    /// <summary>
    /// 句子类型，疑问、祈使、陈述
    /// </summary>
    public enum SentenceType
    {
        Question,
        Order,
        Normal,
        TrueFalse
    }
}
