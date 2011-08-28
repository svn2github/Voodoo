using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Voodoo
{
    public static class Linq
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 扩展的IF
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <example>
        /// People people2 = new People { Name = "ldp615", IsHungry = true, IsThirsty = true, IsTired = true }
        ///        .If(p => p.IsHungry, p => p.Eat())
        ///        .If(p => p.IsThirsty, p => p.Drink())
        ///        .If(p => p.IsTired, p => p.Rest());
        ///    people2.Work();
        /// </example>
        public static T If<T>(this T t, Predicate<T> predicate, Action<T> action) where T : class
        {
            if (t == null) throw new ArgumentNullException();
            if (predicate(t)) action(t);
            return t;
        }

        /// <summary>
        /// Switch扩展方法
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input"></param>
        /// <param name="inputSource"></param>
        /// <param name="outputSource"></param>
        /// <param name="defaultOutput"></param>
        /// <example>
        /// string englishName = "apple";
        /// string chineseName = englishName.Switch(
        ///        new string[] { "apple", "orange", "banana", "pear" },
        ///        new string[] { "苹果", "桔子", "香蕉", "梨" },
        ///        "未知"
        ///        );
        /// </example>
        /// <returns></returns>
        public static TOutput Switch<TOutput, TInput>(this TInput input, IEnumerable<TInput> inputSource, IEnumerable<TOutput> outputSource, TOutput defaultOutput)
        {
            IEnumerator<TInput> inputIterator = inputSource.GetEnumerator();
            IEnumerator<TOutput> outputIterator = outputSource.GetEnumerator();

            TOutput result = defaultOutput;
            while (inputIterator.MoveNext())
            {
                if (outputIterator.MoveNext())
                {
                    if (input.Equals(inputIterator.Current))
                    {
                        result = outputIterator.Current;
                        break;
                    }
                }
                else break;
            }
            return result;
        }
    }

    #region Distinct扩展方法
    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;
        private IEqualityComparer<V> comparer;

        public CommonEqualityComparer(Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, V> keySelector)
            : this(keySelector, EqualityComparer<V>.Default)
        { }

        public bool Equals(T x, T y)
        {
            return comparer.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return comparer.GetHashCode(keySelector(obj));
        }
    }


    public static class DistinctExtensions
    {
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }

        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector, comparer));
        }
    }
    #endregion
}
