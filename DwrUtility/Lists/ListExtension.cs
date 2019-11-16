using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace DwrUtility.Lists
{
    /// <summary>
    /// 
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            return ListHelper.ToList<T>(dt);
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            return ListHelper.ToDataTable(source);
        }

        /// <summary>
        /// 本列表排除另外一个列表数据
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TTarget">排除数据类型</typeparam>
        /// <typeparam name="TType">对比类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="target">排除数据</param>
        /// <param name="sourceFunc"></param>
        /// <param name="targetFunc"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Except<TSource, TTarget, TType>(this IEnumerable<TSource> source,
            IEnumerable<TTarget> target, Func<TSource, TType> sourceFunc, Func<TTarget, TType> targetFunc)
        {
            return ListHelper.Except(source, target, sourceFunc, targetFunc);
        }

        /// <summary>
        /// 分批循环数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size">分批大小</param>
        /// <param name="action">分批集合</param>
        public static void ForBatch<T>(this List<T> source, int size, Action<List<T>> action)
        {
            ListHelper.ForBatch(source, size, action);
        }

        /// <summary>
        /// 分批循环数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size">分批大小</param>
        /// <param name="action">分批集合, 第几条开始, 到第几条结束</param>
        public static void ForBatch<T>(this List<T> source, int size, Action<List<T>, int, int> action)
        {
            ListHelper.ForBatch(source, size, action);
        }

        /// <summary>
        /// 多线程遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">委托方法</param>
        public static void ForeachAsync<T>(this IEnumerable<T> source, Action<T> action) where T : new()
        {
            ListHelper.ForeachAsync(source, action);
        }

        /// <summary>
        /// 获取当前索引值的前一条记录
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index">当前索引值</param>
        /// <returns></returns>
        public static T GetPrevious<T>(this List<T> list, int index) where T : new()
        {
            return index - 1 < 0 ? default(T) : list[index - 1];
        }

        /// <summary>
        /// 获取当前索引值的后一条记录
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index">当前索引值</param>
        /// <returns></returns>
        public static T GetNext<T>(this List<T> list, int index) where T : new()
        {
            return index + 1 > list.Count - 1 ? default(T) : list[index + 1];
        }

        /// <summary>
        /// 获取当前索引值的前n条记录
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index">当前索引值</param>
        /// <param name="n">获取数量</param>
        /// <returns></returns>
        public static IEnumerable<T> GetPreviousList<T>(this IEnumerable<T> list, int index, int n = 1) where T : new()
        {
            if (n < 1)
            {
                return new List<T>();
            }

            var start = index - n;
            return list.Skip(start < 0 ? 0 : start).Take(start < 0 ? n + start : n);
        }

        /// <summary>
        /// 获取当前索引值的后n条记录
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index">当前索引值</param>
        /// <param name="n">获取数量</param>
        /// <returns></returns>
        public static IEnumerable<T> GetNextList<T>(this IEnumerable<T> list, int index, int n = 1) where T : new()
        {
            if (n < 1)
            {
                return new List<T>();
            }

            return list.Skip(index + 1).Take(n);
        }

        /// <summary>
        /// Source.Value赋值给List.Values
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">被设置Value字段值的列表</param>
        /// <param name="source">Id和Value对应的数据源</param>
        /// <param name="listIdField">List.Id字段，字符串集合（多个用split分割）</param>
        /// <param name="listValueField">List.Value被设置值字段</param>
        /// <param name="sourceIdField">Source.Id字段</param>
        /// <param name="sourceValueField">Source.Value字段</param>
        /// <param name="split">字符串分割符</param>
        public static IEnumerable<TList> SetListValuesByIdsString<TList, TSource>(this IEnumerable<TList> list, IEnumerable<TSource> source,
            Func<TList, string> listIdField, Expression<Func<TList, string>> listValueField, Func<TSource, Guid> sourceIdField,
            Func<TSource, string> sourceValueField, char split)
        {
            return ListHelper.SetListValues(new ListValueByIdsString<TList, TSource, Guid>()
            {
                List = list,
                Source = source,
                ListIdField = listIdField,
                ListValueField = listValueField,
                SourceIdField = sourceIdField,
                SourceValueField = sourceValueField,
                Split = split
            });
        }

        /// <summary>
        /// Source.Value赋值给List.Values
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">被设置Value字段值的列表</param>
        /// <param name="source">Id和Value对应的数据源</param>
        /// <param name="listIdField">List.Id字段，字符串集合（多个用split分割）</param>
        /// <param name="listValueField">List.Value被设置值字段</param>
        /// <param name="sourceIdField">Source.Id字段</param>
        /// <param name="sourceValueField">Source.Value字段</param>
        /// <param name="split">字符串分割符</param>
        public static IEnumerable<TList> SetListValuesByIdsString<TList, TSource>(this IEnumerable<TList> list, IEnumerable<TSource> source,
            Func<TList, string> listIdField, Expression<Func<TList, string>> listValueField, Func<TSource, int> sourceIdField,
            Func<TSource, string> sourceValueField, char split)
        {
            return ListHelper.SetListValues(new ListValueByIdsString<TList, TSource, int>()
            {
                List = list,
                Source = source,
                ListIdField = listIdField,
                ListValueField = listValueField,
                SourceIdField = sourceIdField,
                SourceValueField = sourceValueField,
                Split = split
            });
        }

        /// <summary>
        /// Source.Value赋值给List.Values
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">被设置Value字段值的列表</param>
        /// <param name="source">Id和Value对应的数据源</param>
        /// <param name="listIdField">List.Id字段，字符串集合（多个用split分割）</param>
        /// <param name="listValueField">List.Value被设置值字段</param>
        /// <param name="sourceIdField">Source.Id字段</param>
        /// <param name="sourceValueField">Source.Value字段</param>
        /// <param name="split">字符串分割符</param>
        public static IEnumerable<TList> SetListValuesByIdsString<TList, TSource>(this IEnumerable<TList> list, IEnumerable<TSource> source,
            Func<TList, string> listIdField, Expression<Func<TList, string>> listValueField, Func<TSource, long> sourceIdField,
            Func<TSource, string> sourceValueField, char split)
        {
            return ListHelper.SetListValues(new ListValueByIdsString<TList, TSource, long>()
            {
                List = list,
                Source = source,
                ListIdField = listIdField,
                ListValueField = listValueField,
                SourceIdField = sourceIdField,
                SourceValueField = sourceValueField,
                Split = split
            });
        }

        /// <summary>
        /// Source集合赋值给List.Value集合
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TIdType"></typeparam>
        /// <typeparam name="TValueType"></typeparam>
        /// <param name="list">被设置Value字段值的列表</param>
        /// <param name="source">Id和Value对应的数据源</param>
        /// <param name="listIdField">List.Id字段</param>
        /// <param name="listValueField">List.Value被设置值字段</param>
        /// <param name="sourceIdField">Source.Id字段</param>
        /// <param name="sourceValueField">Source.Value字段</param>
        /// <returns></returns>
        public static IEnumerable<TList> SetListValuesByIds<TList, TSource, TIdType, TValueType>(this IEnumerable<TList> list, IEnumerable<TSource> source,
            Func<TList, IEnumerable<TIdType>> listIdField, Expression<Func<TList, IEnumerable<TValueType>>> listValueField, Func<TSource, TIdType> sourceIdField, Func<TSource, TValueType> sourceValueField)
        {
            return ListHelper.SetListValues(new ListValueByIds<TList, TSource, TIdType, TValueType>()
            {
                List = list,
                Source = source,
                ListIdField = listIdField,
                ListValueField = listValueField,
                SourceIdField = sourceIdField,
                SourceValueField = sourceValueField
            });
        }

        /// <summary>
        /// Source.Value赋值给List.Value
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TIdType"></typeparam>
        /// <typeparam name="TValueType"></typeparam>
        /// <param name="list">被设置Value字段值的列表</param>
        /// <param name="source">Id和Value对应的数据源</param>
        /// <param name="listIdField">List.Id字段</param>
        /// <param name="listValueField">List.Value字段</param>
        /// <param name="sourceIdField">Source.Id字段</param>
        /// <param name="sourceValueField">Source.Value字段</param>
        /// <param name="useSourceFirstValue">一对多时（TSource有多个值）取值规则：true使用Source第一个值，false使用最后一个值</param>
        /// <returns></returns>
        public static IEnumerable<TList> SetListValue<TList, TSource, TIdType, TValueType>(this IEnumerable<TList> list, IEnumerable<TSource> source,
            Func<TList, TIdType> listIdField, Expression<Func<TList, TValueType>> listValueField, Func<TSource, TIdType> sourceIdField,
            Func<TSource, TValueType> sourceValueField, bool useSourceFirstValue)
        {
            return ListHelper.SetListValue(new ListValueById<TList, TSource, TIdType, TValueType>()
            {
                List = list,
                Source = source,
                ListIdField = listIdField,
                ListValueField = listValueField,
                SourceIdField = sourceIdField,
                SourceValueField = sourceValueField,
                UseSourceFirstValue = useSourceFirstValue
            });
        }

        /// <summary>
        /// 两列表数据相同（包含两列表重复数据的数量都一样）
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TEquals"></typeparam>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <param name="leftFunc"></param>
        /// <param name="rightFunc"></param>
        /// <param name="isBigData">是否是处理大数据</param>
        /// <returns></returns>
        public static bool IsEquals<TLeft, TRight, TEquals>(this IEnumerable<TLeft> leftList, IEnumerable<TRight> rightList, Func<TLeft, TEquals> leftFunc, Func<TRight, TEquals> rightFunc, bool isBigData)
        {
            var param = new ListComparerParam<TLeft, TRight, TEquals>(leftList, rightList, leftFunc, rightFunc);
            return ListHelper.IsEquals(param, isBigData);
        }

        /// <summary>
        /// 返回两列表差异数据集合（包含重复数据的数量对比）
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TEquals"></typeparam>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <param name="leftFunc"></param>
        /// <param name="rightFunc"></param>
        /// <param name="isBigData">是否是处理大数据</param>
        /// <returns></returns>
        public static ListDiffResult<TLeft, TRight, TEquals> GetDiffList<TLeft, TRight, TEquals>(this IEnumerable<TLeft> leftList, IEnumerable<TRight> rightList, Func<TLeft, TEquals> leftFunc, Func<TRight, TEquals> rightFunc, bool isBigData)
        {
            var param = new ListComparerParam<TLeft, TRight, TEquals>(leftList, rightList, leftFunc, rightFunc);
            return ListHelper.GetDiffList(param, isBigData);
        }

        /// <summary>
        /// 移除两边都是一条记录的数据
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TEquals"></typeparam>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <param name="leftFunc"></param>
        /// <param name="rightFunc"></param>
        /// <returns></returns>
        public static ListComparerParam<TLeft, TRight, TEquals> RemoveSameOneRecord<TLeft, TRight, TEquals>(this IEnumerable<TLeft> leftList, IEnumerable<TRight> rightList, Func<TLeft, TEquals> leftFunc, Func<TRight, TEquals> rightFunc)
        {
            var param = new ListComparerParam<TLeft, TRight, TEquals>(leftList, rightList, leftFunc, rightFunc);
            return ListHelper.RemoveSameOneRecord(param);
        }

        /// <summary>
        /// 是否有重复数据（TKey必须是匿名对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">TKey必须是匿名对象</typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static bool HasRepeat<T, TKey>(this IEnumerable<T> list, Func<T, TKey> key, IEqualityComparer<TKey> comparer = null)
        {
            return ListHelper.HasRepeat(list, key, comparer);
        }

        /// <summary>
        /// 获取重复TKey，多个重复TKey也是返回一个（TKey必须是匿名对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">TKey必须是匿名对象</typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static HashSet<TKey> GetRepeatKeys<T, TKey>(this IEnumerable<T> list, Func<T, TKey> key, IEqualityComparer<TKey> comparer = null)
        {
            return ListHelper.GetRepeatKeys(list, key, comparer);
        }

        /// <summary>
        /// 获取重复数据（TKey必须是匿名对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">TKey必须是匿名对象</typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetRepeatLists<T, TKey>(this IEnumerable<T> list, Func<T, TKey> key, IEqualityComparer<TKey> comparer = null)
        {
            return ListHelper.GetRepeatLists(list, key, comparer);
        }

        /// <summary>
        /// 去重（TKey必须是匿名对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">TKey必须是匿名对象</typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        [Obsolete]
        public static IEnumerable<TKey> ToDist<T, TKey>(this IEnumerable<T> list, Func<T, TKey> key, IEqualityComparer<TKey> comparer = null)
        {
            return ListHelper.ToDist(list, key, comparer);
        }

        /// <summary>
        /// 对象去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparer">对比委托</param>
        /// <returns></returns>
        public static IEnumerable<T> ToDist<T>(this IEnumerable<T> list, Func<T, T, bool> comparer) where T : new()
        {
            return ListHelper.ToDist(list, comparer);
        }

        /// <summary>
        /// 转Dictionary（去重处理，TKey必须是匿名对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">TKey必须是匿名对象</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="useFirstValue">重复使用值规则：true使用第一个值；false使用最后一个值</param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDict<T, TKey, TValue>(this IEnumerable<T> list, Func<T, TKey> key, Func<T, TValue> value, bool useFirstValue, IEqualityComparer<TKey> comparer = null)
        {
            return ListHelper.ToDict(list, key, value, useFirstValue, comparer);
        }

    }
}
