using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Lists
{
    /// <summary>
    /// 列表数据帮助类
    /// </summary>
    public class ListHelper
    {
        #region ids -> values
        /// <summary>
        /// 给list集合字段Value赋值（list集合的Id字段是字符串分割的集合，所以赋值后Value字段也是字符串分割的集合）
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="param"></param>
        public static List<TList> SetListValues<TList, TSource>(ListValueByIdsString<TList, TSource, Guid> param)
        {
            var dict = param.Source.ToDict(param.SourceIdField, param.SourceValueField, true);
            var setValueField = typeof(TList).GetProperty(param.ListValueField.GetPropertyName());
            foreach (var item in param.List)
            {
                setValueField?.SetValue(item, string.Join(param.Split.ToString(), param.ListIdField.Invoke(item).Split(param.Split).Select(Guid.Parse).Select(p => dict.TryGetValue(p, out var value) ? value : null).Where(p => !string.IsNullOrWhiteSpace(p))));
            }

            return param.List;
        }

        /// <summary>
        /// 给list集合字段Value赋值（list集合的Id字段是字符串分割的集合，所以赋值后Value字段也是字符串分割的集合）
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="param"></param>
        public static List<TList> SetListValues<TList, TSource>(ListValueByIdsString<TList, TSource, int> param)
        {
            var dict = param.Source.ToDict(param.SourceIdField, param.SourceValueField, true);
            var setValueField = typeof(TList).GetProperty(param.ListValueField.GetPropertyName());
            foreach (var item in param.List)
            {
                setValueField?.SetValue(item, string.Join(param.Split.ToString(), param.ListIdField.Invoke(item).Split(param.Split).Select(int.Parse).Select(p => dict.TryGetValue(p, out var value) ? value : null).Where(p => !string.IsNullOrWhiteSpace(p))));
            }

            return param.List;
        }

        /// <summary>
        /// 给list集合字段Value赋值（list集合的Id字段是字符串分割的集合，所以赋值后Value字段也是字符串分割的集合）
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="param"></param>
        public static List<TList> SetListValues<TList, TSource>(ListValueByIdsString<TList, TSource, long> param)
        {
            var dict = param.Source.ToDict(param.SourceIdField, param.SourceValueField, true);
            var setValueField = typeof(TList).GetProperty(param.ListValueField.GetPropertyName());
            foreach (var item in param.List)
            {
                setValueField?.SetValue(item, string.Join(param.Split.ToString(), param.ListIdField.Invoke(item).Split(param.Split).Select(long.Parse).Select(p => dict.TryGetValue(p, out var value) ? value : null).Where(p => !string.IsNullOrWhiteSpace(p))));
            }

            return param.List;
        }
        #endregion

        #region List<id> -> List<Value>

        /// <summary>
        /// Source.Value集合赋值给List.Value集合
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TIdType"></typeparam>
        /// <typeparam name="TValueType"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<TList> SetListValues<TList, TSource, TIdType, TValueType>(ListValueByIds<TList, TSource, TIdType, TValueType> param)
        {
            var dict = param.Source.ToDict(param.SourceIdField, param.SourceValueField, true);
            var setValueField = typeof(TList).GetProperty(param.ListValueField.GetPropertyName());
            var defVal = default(TValueType);
            foreach (var item in param.List)
            {
                setValueField?.SetValue(item, param.ListIdField.Invoke(item).Select(p => dict.TryGetValue(p, out var value) ? value : defVal).ToList());
            }

            return param.List;
        }

        #endregion

        #region id -> value

        /// <summary>
        /// Source.Value赋值给List.Value
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TIdType"></typeparam>
        /// <typeparam name="TValueType"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<TList> SetListValue<TList, TSource, TIdType, TValueType>(ListValueById<TList, TSource, TIdType, TValueType> param)
        {
            var dict = param.Source.ToDict(param.SourceIdField, param.SourceValueField, param.UseSourceFirstValue);
            var setValueField = typeof(TList).GetProperty(param.ListValueField.GetPropertyName());
            foreach (var item in param.List)
            {
                if (dict.TryGetValue(param.ListIdField.Invoke(item), out var value))
                {
                    setValueField?.SetValue(item, value);
                }
            }

            return param.List;
        }

        #endregion

        #region 数据对比

        /// <summary>
        /// 两列表数据相同（包含两列表重复数据的数量都一样）
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TEquals"></typeparam>
        /// <param name="param"></param>
        /// <param name="isBigData">是否是处理大数据</param>
        /// <returns></returns>
        public static bool IsEquals<TLeft, TRight, TEquals>(ListComparerParam<TLeft, TRight, TEquals> param, bool isBigData)
        {
            if (param.LeftList.Count != param.RightList.Count)
            {
                return false;
            }

            if (isBigData)
            {
                param = RemoveSameOneRecord(param);
            }

            // 分组对比数据
            var lefts = param.LeftList.GroupBy(param.LeftFunc.Invoke).Select(p => new ListEquals<TEquals>()
            {
                Obj = p.Key,
                Total = p.Count()
            }).ToList();

            var rights = param.RightList.GroupBy(param.RightFunc.Invoke).Select(p => new ListEquals<TEquals>()
            {
                Obj = p.Key,
                Total = p.Count()
            }).ToList();

            if (lefts.Count != rights.Count)
            {
                return false;
            }

            var count = lefts.Join(rights, p => p.Obj, p => p.Obj, (p, q) => new { p, q }).Count(p => p.p.Total == p.q.Total);
            if (count != lefts.Count)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 返回两列表差异数据集合（包含重复数据的数量对比）
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TEquals"></typeparam>
        /// <param name="param"></param>
        /// <param name="isBigData">是否是处理大数据</param>
        public static ListDiffResult<TLeft, TRight, TEquals> GetDiffList<TLeft, TRight, TEquals>(ListComparerParam<TLeft, TRight, TEquals> param, bool isBigData)
        {
            if (isBigData)
            {
                param = RemoveSameOneRecord(param);
            }

            var lefts = param.LeftList.GroupJoin(param.RightList, param.LeftFunc.Invoke, param.RightFunc.Invoke, (p, q) => new { p, q }).Where(p => p.q.Count() != 1).ToList();
            var rights = param.RightList.GroupJoin(param.LeftList, param.RightFunc.Invoke, param.LeftFunc.Invoke, (p, q) => new { p, q }).Where(p => p.q.Count() != 1).ToList();

            var ls = lefts.Where(p => p.q.Count() > 1)
                .GroupBy(p => param.LeftFunc.Invoke(p.p), (p, q) => new { p, q })
                .Select(m => new ListRepeatResult<TLeft, TRight, TEquals>
                {
                    Object = m.p,
                    Lefts = m.q.Select(p => p.p).ToList(),
                    Rights = m.q.First().q.ToList()
                })
                .Where(p => p.Lefts.Count != p.Rights.Count);
            var rs = rights.Where(p => p.q.Count() > 1)
                .GroupBy(p => param.RightFunc.Invoke(p.p), (p, q) => new { p, q })
                .Select(m => new ListRepeatResult<TLeft, TRight, TEquals>
                {
                    Object = m.p,
                    Lefts = m.q.First().q.ToList(),
                    Rights = m.q.Select(p => p.p).ToList()
                })
                .Where(p => p.Lefts.Count != p.Rights.Count);

            var result = new ListDiffResult<TLeft, TRight, TEquals>
            {
                LeftDiffs = lefts.Where(p => !p.q.Any()).Select(p => p.p).ToList(),
                RightDiffs = rights.Where(p => !p.q.Any()).Select(p => p.p).ToList(),
                Repeats = ls.Union(rs).ToList()
            };

            return result;
        }

        /// <summary>
        /// 移除两边都是一条记录的数据
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TEquals"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static ListComparerParam<TLeft, TRight, TEquals> RemoveSameOneRecord<TLeft, TRight, TEquals>(ListComparerParam<TLeft, TRight, TEquals> param)
        {
            var leftDict = new Dictionary<TEquals, TLeft>();
            var leftRepeat = new List<TLeft>();
            foreach (var item in param.LeftList)
            {
                var key = param.LeftFunc.Invoke(item);
                if (!leftDict.ContainsKey(key))
                {
                    leftDict.Add(key, item);
                }
                else
                {
                    leftRepeat.Add(item);
                }
            }

            var rightDict = new Dictionary<TEquals, TRight>();
            var rightRepeat = new List<TRight>();
            foreach (var item in param.RightList)
            {
                var key = param.RightFunc.Invoke(item);
                if (!rightDict.ContainsKey(key))
                {
                    rightDict.Add(key, item);
                }
                else
                {
                    rightRepeat.Add(item);
                }
            }

            //重复的数据
            var repeats = leftRepeat.Select(param.LeftFunc.Invoke).Union(rightRepeat.Select(param.RightFunc.Invoke)).ToList();

            //左1右0
            var leftHas = new Dictionary<TEquals, TLeft>();
            foreach (var item in leftDict)
            {
                if (!rightDict.ContainsKey(item.Key))
                {
                    leftHas.Add(item.Key, item.Value);
                }
            }
            //左0右1
            var rightHas = new Dictionary<TEquals, TRight>();
            foreach (var item in rightDict)
            {
                if (!leftDict.ContainsKey(item.Key))
                {
                    rightHas.Add(item.Key, item.Value);
                }
            }

            //左边不同数据都取出来
            var listLeft = new List<TLeft>();
            listLeft.AddRange(leftRepeat);
            foreach (var key in repeats)
            {
                if (leftDict.ContainsKey(key))
                {
                    listLeft.Add(leftDict[key]);
                }
            }

            foreach (var item in leftHas.GroupJoin(repeats, p => p.Key, p => p, (p, q) => new { p, q }).Where(p => !p.q.Any()).Select(m => m.p).ToList())
            {
                listLeft.Add(leftDict[item.Key]);
            }

            //右边不同数据都取出来
            var listRight = new List<TRight>();
            listRight.AddRange(rightRepeat);
            foreach (var key in repeats)
            {
                if (rightDict.ContainsKey(key))
                {
                    listRight.Add(rightDict[key]);
                }
            }

            foreach (var item in rightHas.GroupJoin(repeats, p => p.Key, p => p, (p, q) => new { p, q }).Where(p => !p.q.Any()).Select(m => m.p).ToList())
            {
                listRight.Add(rightDict[item.Key]);
            }

            param.LeftList = listLeft;
            param.RightList = listRight;

            return param;
        }

        #endregion

        #region 重复数据处理

        /// <summary>
        /// 是否有重复数据（TKey必须是匿名对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">TKey必须是匿名对象</typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static bool HasRepeat<T, TKey>(List<T> list, Func<T, TKey> key, IEqualityComparer<TKey> comparer = null)
        {
            var hsSet = comparer == null ? new HashSet<TKey>() : new HashSet<TKey>(comparer);
            foreach (var item in list)
            {
                var success = hsSet.Add(key.Invoke(item));
                if (!success)
                {
                    return true;
                }
            }

            return false;
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
        public static HashSet<TKey> GetRepeatKeys<T, TKey>(List<T> list, Func<T, TKey> key, IEqualityComparer<TKey> comparer = null)
        {
            var hsSet = comparer == null ? new HashSet<TKey>() : new HashSet<TKey>(comparer);
            var items = comparer == null ? new HashSet<TKey>() : new HashSet<TKey>(comparer);
            foreach (var item in list)
            {
                var v = key.Invoke(item);
                var success = hsSet.Add(v);
                if (!success)
                {
                    items.Add(v);
                }
            }

            return items;
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
        public static List<T> GetRepeatLists<T, TKey>(List<T> list, Func<T, TKey> key, IEqualityComparer<TKey> comparer = null)
        {
            var rows = new List<T>();
            var repeatKeys = GetRepeatKeys(list, key, comparer);
            if (repeatKeys.Count == 0)
            {
                return rows;
            }

            foreach (var item in list)
            {
                if (repeatKeys.Contains(key.Invoke(item)))
                {
                    rows.Add(item);
                }
            }

            return rows;
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
        public static List<TKey> ToDistinct<T, TKey>(List<T> list, Func<T, TKey> key, IEqualityComparer<TKey> comparer = null)
        {
            var dict = comparer == null ? new HashSet<TKey>() : new HashSet<TKey>(comparer);
            foreach (var item in list)
            {
                dict.Add(key.Invoke(item));
            }

            return dict.ToList();
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
        public static Dictionary<TKey, TValue> ToDict<T, TKey, TValue>(List<T> list, Func<T, TKey> key, Func<T, TValue> value, bool useFirstValue, IEqualityComparer<TKey> comparer = null)
        {
            var dict = comparer == null ? new Dictionary<TKey, TValue>() : new Dictionary<TKey, TValue>(comparer);
            foreach (var item in list)
            {
                var k = key.Invoke(item);
                if (!dict.ContainsKey(k))
                {
                    dict.Add(k, value.Invoke(item));
                }
                else
                {
                    if (!useFirstValue)
                    {
                        dict[k] = value.Invoke(item);
                    }
                }
            }

            return dict;
        }

        #endregion
    }
}
