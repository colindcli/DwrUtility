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
    public class TreeParentsDepthParam<T, TPk>
    {
        /// <summary>
        /// 树列表
        /// </summary>
        public List<T> TreeLists { get; set; }
        /// <summary>
        /// Id字段: p=>p.Id
        /// </summary>
        public Func<T, TPk> IdField { get; set; }
        /// <summary>
        /// 父Id字段: p=>p.ParentId
        /// </summary>
        public Func<T, TPk> ParentIdField { get; set; }
        /// <summary>
        /// 上级节点字段(选项): p=>p.ParentIds
        /// </summary>
        public Expression<Func<T, string>> ParentIdsField { get; set; } = null;
        /// <summary>
        /// 设置深度字段(选项)：p=>p.Depth
        /// </summary>
        public Expression<Func<T, int>> DepthField { get; set; } = null;
        /// <summary>
        /// ParentIds加前缀
        /// </summary>
        public string PrefixParentIds { get; set; } = "";
        /// <summary>
        /// Depth增加深度值
        /// </summary>
        public int PrefixDepth { get; set; } = 0;
        /// <summary>
        /// 父集合分隔符：默认“,”
        /// </summary>
        public char Split { get; set; } = ',';
    }

    class TreeParentsDepthModel<T>
    {
        /// <summary>
        /// 上级节点字段
        /// </summary>
        public Func<T, string> GetParentIds { get; set; }

        public PropertyInfo SetParentIds { get; set; }
        /// <summary>
        /// 设置深度字段
        /// </summary>
        public Func<T, int> GetDepth { get; set; }

        public PropertyInfo SetDepth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Split { get; set; }
    }
}
