using System.Collections.Generic;

namespace DwrUtility.Lists
{
    /// <summary>
    /// 差异数据结果
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <typeparam name="TEquals"></typeparam>
    public class ListRepeatResult<TLeft, TRight, TEquals>
    {
        /// <summary>
        /// 有差异对象
        /// </summary>
        public TEquals Object { get; set; }
        /// <summary>
        /// 左边对象集合
        /// </summary>
        public List<TLeft> Lefts { get; set; }
        /// <summary>
        /// 右边对象集合
        /// </summary>
        public List<TRight> Rights { get; set; }
    }
}
