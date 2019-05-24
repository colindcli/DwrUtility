using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DwrUtility.Trees
{
    /// <summary>
    /// 
    /// </summary>
    public static class TreeExtension
    {
        /// <summary>
        /// 获取所有父节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk">Id字段数据类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <param name="idValue">当前Id值</param>
        /// <param name="idField">Id字段</param>
        /// <param name="parentIdField">ParentId字段</param>
        /// <param name="includeSelf">是否包含自己节点</param>
        /// <returns></returns>
        public static List<T> GetParentNodes<T, TPk>(this List<T> list, TPk idValue, Func<T, TPk> idField, Func<T, TPk> parentIdField, bool includeSelf)
        {
            return TreeHelper.GetParentNodes(list, idValue, idField, parentIdField, includeSelf);
        }

        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk">Id字段数据类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <param name="idValue">从list中找此Id及以下节点</param>
        /// <param name="idField">Id字段</param>
        /// <param name="parentIdField">ParentId字段</param>
        /// <param name="includeSelf">是否包含自己节点</param>
        /// <returns></returns>
        public static List<T> GetChildNodes<T, TPk>(this List<T> list, TPk idValue, Func<T, TPk> idField, Func<T, TPk> parentIdField, bool includeSelf)
        {
            return TreeHelper.GetChildNodes(list, idValue, idField, parentIdField, includeSelf);
        }

        /// <summary>
        /// List转生成树结构
        /// </summary>
        /// <typeparam name="T">列表对象</typeparam>
        /// <typeparam name="TIdType">Id字段类型</typeparam>
        /// <param name="treeLists">列表集合</param>
        /// <param name="idField">Id字段</param>
        /// <param name="parentIdField">父Id字段</param>
        /// <param name="childField">子孙字段</param>
        /// <returns></returns>
        public static List<T> ToTreeView<T, TIdType>(this List<T> treeLists, Func<T, TIdType> idField, Func<T, TIdType> parentIdField, Expression<Func<T, List<T>>> childField) where T : new()
        {
            return TreeHelper.ToTreeView(treeLists, idField, parentIdField, childField);
        }

        /// <summary>
        /// 树结构转List
        /// </summary>
        /// <typeparam name="T">列表对象类型</typeparam>
        /// <param name="treeView">树结构列表集合</param>
        /// <param name="childField">子孙字段</param>
        /// <returns></returns>
        public static List<T> ToTreeList<T>(this List<T> treeView, Expression<Func<T, List<T>>> childField) where T : new()
        {
            return TreeHelper.ToTreeList(treeView, childField);
        }

        /// <summary>
        /// 生成新的树Id（在原树上修改Id）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="treeLists"></param>
        /// <param name="newIds"></param>
        /// <param name="idField"></param>
        /// <param name="parentIdField"></param>
        /// <returns></returns>
        public static List<T> ToTreeNewId<T, TPk>(this List<T> treeLists, List<TPk> newIds, Expression<Func<T, TPk>> idField, Expression<Func<T, TPk>> parentIdField) where T : class
        {
            return TreeHelper.ToTreeNewId(new TreeIdParam<T, TPk>()
            {
                TreeLists = treeLists,
                NewIds = newIds,
                IdField = idField,
                ParentIdField = parentIdField
            });
        }

        /// <summary>
        /// 转换树Id类型（在原树上修改Id）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TToType"></typeparam>
        /// <param name="treeLists"></param>
        /// <param name="newIds"></param>
        /// <param name="idField"></param>
        /// <param name="parentIdField"></param>
        /// <param name="toIdField"></param>
        /// <param name="toParentIdField"></param>
        /// <returns></returns>
        public static List<T> ToTreeNewIdType<T, TType, TToType>(this List<T> treeLists, List<TToType> newIds, Expression<Func<T, TType>> idField, Expression<Func<T, TType>> parentIdField, Expression<Func<T, TToType>> toIdField, Expression<Func<T, TToType>> toParentIdField) where T : class
        {
            return TreeHelper.ToTreeNewIdType(new TreeTypeParam<T, TType, TToType>()
            {
                TreeLists = treeLists,
                NewIds = newIds,
                IdField = idField,
                ParentIdField = parentIdField,
                ToIdField = toIdField,
                ToParentIdField = toParentIdField
            });
        }

        /// <summary>
        /// 删除树节点（下级树接驳上来）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="treeLists"></param>
        /// <param name="removeIds"></param>
        /// <param name="idField"></param>
        /// <param name="parentIdField"></param>
        /// <returns></returns>
        public static List<T> DeleteNodes<T, TPk>(this List<T> treeLists, List<TPk> removeIds, Func<T, TPk> idField, Expression<Func<T, TPk>> parentIdField) where T : class
        {
            return TreeHelper.DeleteNodes(new DeleteNodeParam<T, TPk>()
            {
                TreeLists = treeLists,
                RemoveIds = removeIds,
                IdField = idField,
                ParentIdField = parentIdField
            });
        }

        /// <summary>
        /// 获取树的顶级节点集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="treeLists">树列表</param>
        /// <param name="idField">Id字段: p=>p.Id</param>
        /// <param name="parentIdField">父Id字段: p=>p.ParentId</param>
        /// <returns></returns>
        public static List<T> GetTopNodes<T, TPk>(this List<T> treeLists, Func<T, TPk> idField, Func<T, TPk> parentIdField)
            where T : class
        {
            return TreeHelper.GetTopNodes(treeLists, idField, parentIdField);
        }

        /// <summary>
        /// 获取树的叶子节点集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="treeLists">树列表</param>
        /// <param name="idField">Id字段: p=>p.Id</param>
        /// <param name="parentIdField">父Id字段: p=>p.ParentId</param>
        /// <returns></returns>
        public static List<T> GetLeafNodes<T, TPk>(this List<T> treeLists, Func<T, TPk> idField, Func<T, TPk> parentIdField)
            where T : class
        {
            return TreeHelper.GetLeafNodes(treeLists, idField, parentIdField);
        }
    }
}
