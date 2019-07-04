using System;
using System.Collections.Generic;

namespace DwrUtility.Lists
{
    /// <summary>
    /// 比较两边集合差异
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <typeparam name="TEquals"></typeparam>
    public class ListComparerParam<TLeft, TRight, TEquals>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <param name="leftFunc"></param>
        /// <param name="rightFunc"></param>
        public ListComparerParam(IEnumerable<TLeft> leftList, IEnumerable<TRight> rightList, Func<TLeft, TEquals> leftFunc, Func<TRight, TEquals> rightFunc)
        {
            LeftList = leftList;
            RightList = rightList;
            LeftFunc = leftFunc;
            RightFunc = rightFunc;
        }

        /// <summary>
        /// 左边数据
        /// </summary>
        public IEnumerable<TLeft> LeftList { get; set; }
        /// <summary>
        /// 右边数据
        /// </summary>
        public IEnumerable<TRight> RightList { get; set; }
        /// <summary>
        /// 对比规则
        /// </summary>
        public Func<TLeft, TEquals> LeftFunc { get; set; }
        /// <summary>
        /// 对比规则
        /// </summary>
        public Func<TRight, TEquals> RightFunc { get; set; }
    }
}
