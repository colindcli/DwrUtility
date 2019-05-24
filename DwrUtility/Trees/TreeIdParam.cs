using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DwrUtility.Trees
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public class TreeIdParam<T, TPk>
    {
        /// <summary>
        /// 树列表
        /// </summary>
        public List<T> TreeLists { get; set; }
        /// <summary>
        /// 新Id集合
        /// </summary>
        public List<TPk> NewIds { get; set; }
        /// <summary>
        /// Id字段: p=>p.Id
        /// </summary>
        public Expression<Func<T, TPk>> IdField { get; set; }
        /// <summary>
        /// 父Id字段: p=>p.ParentId
        /// </summary>
        public Expression<Func<T, TPk>> ParentIdField { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private int Index { get; set; }

        public TPk GetNewId()
        {
            var m = NewIds[Index];
            Index++;
            return m;
        }
    }

    class TreeIdModel<T, TPk>
    {
        /// <summary>
        /// 
        /// </summary>
        public Func<T, TPk> GetId { get; set; }

        public PropertyInfo SetId { get; set; }
        /// <summary>
        /// 父Id字段
        /// </summary>
        public Func<T, TPk> GetParentId { get; set; }

        public PropertyInfo SetParentId { get; set; }
    }
}
