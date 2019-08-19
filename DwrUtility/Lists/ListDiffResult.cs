using System.Collections.Generic;

namespace DwrUtility.Lists
{
    /// <summary>
    /// 差异数据结果
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <typeparam name="TEquals"></typeparam>
    public class ListDiffResult<TLeft, TRight, TEquals>
    {
        /// <summary>
        /// 差异数据（右边没有的数据）
        /// </summary>
        public List<TLeft> LeftDiffs { get; set; }
        /// <summary>
        /// 差异数据（左边没有的数据）
        /// </summary>
        public List<TRight> RightDiffs { get; set; }
        /// <summary>
        /// 两边有相同对象的数据，但两边数量不一样
        /// </summary>
        public List<ListRepeatResult<TLeft, TRight, TEquals>> Repeats { get; set; } = new List<ListRepeatResult<TLeft, TRight, TEquals>>();
    }
}
