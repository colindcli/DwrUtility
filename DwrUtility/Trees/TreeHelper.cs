using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DwrUtility.Trees
{
    /// <summary>
    /// 树形帮助类
    /// </summary>
    public class TreeHelper
    {
        /// <summary>
        /// 删除树节点（下级树接驳上来）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        public static List<T> DeleteNodes<T, TPk>(DeleteNodeParam<T, TPk> param) where T : class
        {
            return DeleteNodeUtility.DeleteNodes(param);
        }

        /// <summary>
        /// 生成新的树Id（在原树上修改Id）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<T> ToTreeNewId<T, TPk>(TreeIdParam<T, TPk> param) where T : class
        {
            NewTreeUtility.Generate(param);
            return param.TreeLists;
        }

        /// <summary>
        /// 转换树Id类型（在原树上修改Id）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TToType"></typeparam>
        /// <param name="param"></param>
        public static List<T> ToTreeNewIdType<T, TType, TToType>(TreeTypeParam<T, TType, TToType> param) where T : class
        {
            NewTreeTypeUtility.Generate(param);
            return param.TreeLists;
        }

        /// <summary>
        /// 设置树父集合和深度（在原树上修改Id）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="param"></param>
        public static List<T> ToParentsDepth<T, TPk>(TreeParentsDepthParam<T, TPk> param) where T : class
        {
            NewTreeParentsDepthUtility.Generate(param);
            return param.TreeLists;
        }

        /// <summary>
        /// 遍历树节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="treeView"></param>
        /// <param name="childField"></param>
        /// <param name="predicate"></param>
        public static void ForEachTreeView<T>(List<T> treeView, Func<T, List<T>> childField, Action<T> predicate)
        {
            foreach (var item in treeView)
            {
                predicate.Invoke(item);
                ForEachTreeView<T>(childField.Invoke(item), childField, predicate);
            }
        }

        /// <summary>
        /// List转生成树结构
        /// </summary>
        /// <typeparam name="T">列表对象类型</typeparam>
        /// <typeparam name="TIdType">Id字段类型</typeparam>
        /// <param name="treeLists">列表集合</param>
        /// <param name="idField">Id字段</param>
        /// <param name="parentIdField">父Id字段</param>
        /// <param name="childField">子孙字段</param>
        /// <returns></returns>
        public static List<T> ToTreeView<T, TIdType>(List<T> treeLists, Func<T, TIdType> idField, Func<T, TIdType> parentIdField, Expression<Func<T, List<T>>> childField) where T : new()
        {
            if (treeLists == null)
            {
                return null;
            }

            if (idField == null)
            {
                throw new Exception("没指定idField字段");
            }

            if (parentIdField == null)
            {
                throw new Exception("没指定parentIdField字段");
            }

            if (childField == null)
            {
                throw new Exception("没指定childField字段");
            }

            var setChild = typeof(T).GetProperty(childField.GetPropertyName());

            treeLists.ForEach(row => setChild?.SetValue(row, treeLists.Where(item => parentIdField.Invoke(item).Equals(idField.Invoke(row))).ToList()));
            return treeLists.Where(j => !treeLists.Exists(i => idField.Invoke(i).Equals(parentIdField.Invoke(j)))).ToList();
        }

        /// <summary>
        /// 树结构转List
        /// </summary>
        /// <typeparam name="T">列表对象类型</typeparam>
        /// <param name="treeView">树结构列表集合</param>
        /// <param name="childField">子孙字段</param>
        /// <returns></returns>
        public static List<T> ToTreeList<T>(List<T> treeView, Expression<Func<T, List<T>>> childField) where T : new()
        {
            if (treeView == null)
            {
                return null;
            }

            if (childField == null)
            {
                throw new Exception("没指定childField字段");
            }

            var getChild = childField.Compile();
            var setChild = typeof(T).GetProperty(childField.GetPropertyName());

            var list = new List<T>();
            RecursiveNode(treeView, ref list, getChild, setChild);
            return list;
        }

        private static void RecursiveNode<T>(List<T> rows, ref List<T> list, Func<T, List<T>> getChild, PropertyInfo setChild)
        {
            foreach (var row in rows)
            {
                var items = getChild.Invoke(row);
                setChild.SetValue(row, null);
                list.Add(row);
                RecursiveNode(items, ref list, getChild, setChild);
            }
        }

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
        public static List<T> GetParentNodes<T, TPk>(List<T> list, TPk idValue, Func<T, TPk> idField, Func<T, TPk> parentIdField, bool includeSelf)
        {
            var obj = list.Find(li => idField.Invoke(li).Equals(idValue));
            if (obj == null)
            {
                return null;
            }

            var rows = new List<T>();
            bool Predicate(T li, T o) => idField.Invoke(li).Equals(parentIdField.Invoke(o));
            RecursiveNode(list, obj, Predicate, ref rows);
            rows.Reverse();
            if (includeSelf)
            {
                rows.Add(obj);
            }

            return rows;
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
        public static List<T> GetChildNodes<T, TPk>(List<T> list, TPk idValue, Func<T, TPk> idField, Func<T, TPk> parentIdField, bool includeSelf)
        {
            var obj = list.Find(li => idField.Invoke(li).Equals(idValue));
            if (obj == null)
            {
                return null;
            }

            var rows = new List<T>();
            if (includeSelf)
            {
                rows.Add(obj);
            }
            bool Predicate(T li, T o) => parentIdField.Invoke(li).Equals(idField.Invoke(o));
            RecursiveNode(list, obj, Predicate, ref rows);
            return rows;
        }

        /// <summary>
        /// 递归节点
        /// </summary>
        private static void RecursiveNode<T>(List<T> list, T obj, Func<T, T, bool> predicate, ref List<T> child)
        {
            var items = list.Where(li => predicate.Invoke(li, obj)).ToList();
            if (items.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                child.Add(item);
                RecursiveNode(list, item, predicate, ref child);
            }
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
        public static List<T> GetTopNodes<T, TPk>(List<T> treeLists, Func<T, TPk> idField, Func<T, TPk> parentIdField) where T : class
        {
            return (from left in treeLists
                    join right in treeLists on parentIdField.Invoke(left) equals idField.Invoke(right) into temps
                    from temp in temps.DefaultIfEmpty()
                    where temp is null
                    select left).ToList();
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
        public static List<T> GetLeafNodes<T, TPk>(List<T> treeLists, Func<T, TPk> idField, Func<T, TPk> parentIdField) where T : class
        {
            return (from left in treeLists
                    join right in treeLists on idField.Invoke(left) equals parentIdField.Invoke(right) into temps
                    from temp in temps.DefaultIfEmpty()
                    where temp is null
                    select left).ToList();
        }

        /// <summary>
        /// 产生新Guid集合
        /// </summary>
        /// <param name="number">产生集合数量</param>
        /// <returns></returns>
        public static List<Guid> NewGuid(int number)
        {
            var guids = new List<Guid>();
            if (number <= 0)
            {
                return guids;
            }

            for (var i = 0; i < number; i++)
            {
                guids.Add(Guid.NewGuid());
            }

            return guids;
        }

        /// <summary>
        /// 产生新Guid?集合
        /// </summary>
        /// <param name="number">产生集合数量</param>
        /// <returns></returns>
        public static List<Guid?> NewGuidNull(int number)
        {
            var guids = new List<Guid?>();
            if (number <= 0)
            {
                return guids;
            }

            for (var i = 0; i < number; i++)
            {
                guids.Add(Guid.NewGuid());
            }

            return guids;
        }
    }
}
