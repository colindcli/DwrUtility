using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DwrUtility.Trees
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public class DeleteNodeParam<T, TPk>
    {
        /// <summary>
        /// 树列表
        /// </summary>
        public List<T> TreeLists { get; set; }
        /// <summary>
        /// 要移除行的Id值集合
        /// </summary>
        public List<TPk> RemoveIds { get; set; }
        /// <summary>
        /// Id字段：p=>p.Id
        /// </summary>
        public Func<T, TPk> IdField { get; set; }
        /// <summary>
        /// ParentId字段：p=>p.ParentId
        /// </summary>
        public Expression<Func<T, TPk>> ParentIdField { get; set; }
    }
}
