using System;
using System.Collections.Generic;

namespace DwrUtility.Lists
{
    public class ListComparerParam<TLeft, TRight, TEquals>
    {
        public ListComparerParam(List<TLeft> leftList, List<TRight> rightList, Func<TLeft, TEquals> leftFunc, Func<TRight, TEquals> rightFunc)
        {
            LeftList = leftList;
            RightList = rightList;
            LeftFunc = leftFunc;
            RightFunc = rightFunc;
        }

        /// <summary>
        /// 左边数据
        /// </summary>
        public List<TLeft> LeftList { get; set; }
        /// <summary>
        /// 右边数据
        /// </summary>
        public List<TRight> RightList { get; set; }
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
