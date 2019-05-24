using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Trees
{
    internal sealed class DeleteNodeUtility
    {
        /// <summary>
        /// 删除树节点（下级树接驳上来）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        public static List<T> DeleteNodes<T, TPk>(DeleteNodeParam<T, TPk> param) where T : class
        {
            if (param.TreeLists == null || param.TreeLists.Count == 0 || param.RemoveIds == null || param.RemoveIds.Count == 0)
            {
                return param.TreeLists;
            }

            if (param.IdField == null)
            {
                throw new Exception("Id字段没有赋值");
            }

            if (param.ParentIdField == null)
            {
                throw new Exception("父Id字段没有赋值");
            }

            var getParentId = param.ParentIdField.Compile();
            var setParentId = typeof(T).GetProperty(param.ParentIdField.GetPropertyName());

            foreach (var id in param.RemoveIds)
            {
                var item = param.TreeLists.Find(p => param.IdField.Invoke(p).Equals(id));
                if (item != null)
                {
                    var rows = param.TreeLists.Where(p => getParentId.Invoke(p).Equals(param.IdField.Invoke(item))).ToList();
                    foreach (var row in rows)
                    {
                        setParentId?.SetValue(row, getParentId(item));
                    }
                }

                param.TreeLists.RemoveAll(p => param.IdField.Invoke(p).Equals(id));
            }

            return param.TreeLists;
        }
    }
}
