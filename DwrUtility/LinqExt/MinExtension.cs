using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.LinqExt
{
    /// <summary>
    /// 
    /// </summary>
    public static class MinExtension
    {
        private static T GetMinValue<T>(IEnumerable<T> source)
        {
            if (source == null)
            {
                return default(T);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            if (!source.Any())
            {
                return default(T);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return source.Min();
        }

        private static TTarget GetMinValue<TSource, TTarget>(IEnumerable<TSource> source, Func<TSource, TTarget> selector)
        {
            if (source == null)
            {
                return default(TTarget);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            if (!source.Any())
            {
                return default(TTarget);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return source.Min(selector);
        }

        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double MinValue(this IEnumerable<double> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal MinValue(this IEnumerable<decimal> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal? MinValue(this IEnumerable<decimal?> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static decimal MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static double? MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource MinValue<TSource>(this IEnumerable<TSource> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int MinValue(this IEnumerable<int> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static double MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static float? MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int? MinValue(this IEnumerable<int?> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long MinValue(this IEnumerable<long> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static float MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static long? MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static long MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static int? MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static int MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float? MinValue(this IEnumerable<float?> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double? MinValue(this IEnumerable<double?> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long? MinValue(this IEnumerable<long?> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float MinValue(this IEnumerable<float> source)
        {
            return GetMinValue(source);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MinValue<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return GetMinValue(source, selector);
        }
        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static decimal? MinValue<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return GetMinValue(source, selector);
        }
    }
}
