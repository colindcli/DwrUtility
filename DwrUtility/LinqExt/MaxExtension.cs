using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.LinqExt
{
    /// <summary>
    /// 
    /// </summary>
    public static class MaxExtension
    {
        private static T GetMaxValue<T>(IEnumerable<T> source)
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
            return source.Max();
        }

        private static TTarget GetMaxValue<TSource, TTarget>(IEnumerable<TSource> source, Func<TSource, TTarget> selector)
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
            return source.Max(selector);
        }

        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double MaxValue(this IEnumerable<double> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal MaxValue(this IEnumerable<decimal> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal? MaxValue(this IEnumerable<decimal?> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static decimal MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static double? MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource MaxValue<TSource>(this IEnumerable<TSource> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int MaxValue(this IEnumerable<int> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static double MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static float? MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int? MaxValue(this IEnumerable<int?> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long MaxValue(this IEnumerable<long> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static float MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static long? MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static long MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static int? MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static int MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float? MaxValue(this IEnumerable<float?> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double? MaxValue(this IEnumerable<double?> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long? MaxValue(this IEnumerable<long?> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float MaxValue(this IEnumerable<float> source)
        {
            return GetMaxValue(source);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MaxValue<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return GetMaxValue(source, selector);
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static decimal? MaxValue<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return GetMaxValue(source, selector);
        }
    }
}
