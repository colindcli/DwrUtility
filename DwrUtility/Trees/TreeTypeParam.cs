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
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TToType"></typeparam>
    public class TreeTypeParam<T, TType, TToType>
    {
        /// <summary>
        /// 树列表
        /// </summary>
        public List<T> TreeLists { get; set; }
        /// <summary>
        /// 新Id集合
        /// </summary>
        public List<TToType> NewIds { get; set; }
        /// <summary>
        /// p=>p.Id
        /// </summary>
        public Expression<Func<T, TType>> IdField { get; set; }
        /// <summary>
        /// p=>p.ParentId
        /// </summary>
        public Expression<Func<T, TType>> ParentIdField { get; set; }
        /// <summary>
        /// p=>p.ToId
        /// </summary>
        public Expression<Func<T, TToType>> ToIdField { get; set; }
        /// <summary>
        /// p=>p.ToParentId
        /// </summary>
        public Expression<Func<T, TToType>> ToParentIdField { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private int Index { get; set; }

        public TToType GetNewId()
        {
            var m = NewIds[Index];
            Index++;
            return m;
        }
    }

    class TreeTypeModel<T, TType, TToType>
    {
        /// <summary>
        /// p=>p.Guid
        /// </summary>
        public Func<T, TType> GetId { get; set; }
        public PropertyInfo SetId { get; set; }
        /// <summary>
        /// p=>p.ParentGuid
        /// </summary>
        public Func<T, TType> GetParentId { get; set; }
        public PropertyInfo SetParentId { get; set; }
        /// <summary>
        /// p=>p.Id
        /// </summary>
        public Func<T, TToType> GetToId { get; set; }
        public PropertyInfo SetToId { get; set; }
        /// <summary>
        /// p=>p.ParentId
        /// </summary>
        public Func<T, TToType> GetToParentId { get; set; }
        public PropertyInfo SetToParentId { get; set; }
    }
}
