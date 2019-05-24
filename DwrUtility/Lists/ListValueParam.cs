using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DwrUtility.Lists
{
    /// <summary>
    /// Source.Value赋值给List.Value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TIdType"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public class ListValueById<T, TSource, TIdType, TValueType>
    {
        /// <summary>
        /// 被设置Value字段值的列表
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// Id和Value对应的数据源
        /// </summary>
        public List<TSource> Source { get; set; }
        /// <summary>
        /// List.Id字段
        /// </summary>
        public Func<T, TIdType> ListIdField { get; set; }
        /// <summary>
        /// List.Value字段
        /// </summary>
        public Expression<Func<T, TValueType>> ListValueField { get; set; }
        /// <summary>
        /// Source.Id字段
        /// </summary>
        public Func<TSource, TIdType> SourceIdField { get; set; }
        /// <summary>
        /// Source.Value字段
        /// </summary>
        public Func<TSource, TValueType> SourceValueField { get; set; }
        /// <summary>
        /// 一对多时（TSource有多个值）取值规则：true使用Source第一个值，false使用最后一个值
        /// </summary>
        public bool UseSourceFirstValue { get; set; }
    }

    /// <summary>
    /// Source.Value赋值给List.Values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSourceIdType"></typeparam>
    public class ListValueByIdsString<T, TSource, TSourceIdType>
    {
        /// <summary>
        /// 被设置Value字段值的列表
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// Id和Value对应的数据源
        /// </summary>
        public List<TSource> Source { get; set; }
        /// <summary>
        /// List.Id字段，字符串集合（多个用split分割）
        /// </summary>
        public Func<T, string> ListIdField { get; set; }
        /// <summary>
        /// List.Value被设置值字段
        /// </summary>
        public Expression<Func<T, string>> ListValueField { get; set; }
        /// <summary>
        /// Source.Id字段 (此字段值唯一)
        /// </summary>
        public Func<TSource, TSourceIdType> SourceIdField { get; set; }
        /// <summary>
        /// Source.Value字段
        /// </summary>
        public Func<TSource, string> SourceValueField { get; set; }
        /// <summary>
        /// 字符串分割符
        /// </summary>
        public char Split { get; set; }
    }

    /// <summary>
    /// Source集合赋值给List.Value集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TIdType"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public class ListValueByIds<T, TSource, TIdType, TValueType>
    {
        /// <summary>
        /// 被设置Value字段值的列表
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// Id和Value对应的数据源
        /// </summary>
        public List<TSource> Source { get; set; }
        /// <summary>
        /// List.Id字段
        /// </summary>
        public Func<T, List<TIdType>> ListIdField { get; set; }
        /// <summary>
        /// List.Value被设置值字段
        /// </summary>
        public Expression<Func<T, List<TValueType>>> ListValueField { get; set; }
        /// <summary>
        /// Source.Id字段 (此字段值唯一)
        /// </summary>
        public Func<TSource, TIdType> SourceIdField { get; set; }
        /// <summary>
        /// Source.Value字段
        /// </summary>
        public Func<TSource, TValueType> SourceValueField { get; set; }
    }

    internal class ListValueRow<T, TIdType>
    {
        public TIdType Id { get; set; }
        public T Item { get; set; }
    }
}
