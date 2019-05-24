using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Trees
{
    /// <summary>
    /// 生成新的树Id（在原树上修改Id）
    /// </summary>
    internal sealed class NewTreeUtility
    {
        /// <summary>
        /// 生成新的树Id（在原树上修改Id）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static void Generate<T, TPk>(TreeIdParam<T, TPk> param) where T : class
        {
            if (param.TreeLists.Count == 0)
            {
                return;
            }

            if (param.TreeLists.Count > param.NewIds.Count)
            {
                throw new Exception("新Id数量不足");
            }

            var model = new TreeIdModel<T, TPk>();
            var type = typeof(T);
            model.GetId = param.IdField.Compile();
            model.SetId = type.GetProperty(param.IdField.GetPropertyName());
            model.GetParentId = param.ParentIdField.Compile();
            model.SetParentId = type.GetProperty(param.ParentIdField.GetPropertyName());

            var ids = new List<TPk>();
            ids.AddRange(param.NewIds);
            ids.AddRange(param.TreeLists.Select(p => model.GetId.Invoke(p)));
            var moreOne = ids.GroupBy(p => p).Select(p => new { p, Total = p.Count() }).Where(p => p.Total > 1).Select(p => p.p.Key).Distinct().ToList();
            if (moreOne.Count > 0)
            {
                throw new Exception($"Id重复: {string.Join(";", moreOne)}");
            }

            Begin(param, model);
        }

        private static void Begin<T, TPk>(TreeIdParam<T, TPk> param, TreeIdModel<T, TPk> model) where T : class
        {
            //顶级节点
            var topNodes = param.TreeLists.GetTopNodes(model.GetId, model.GetParentId);

            foreach (var topNode in topNodes)
            {
                var childs = param.TreeLists.GetChildNodes(model.GetId.Invoke(topNode), model.GetId, model.GetParentId, false);
                Recursive(childs, topNode, default(TPk), param, model);
            }
        }

        /// <summary>
        /// 复制树
        /// </summary>
        /// <param name="list"></param>
        /// <param name="newParentValue"></param>
        /// <param name="param"></param>
        /// <param name="model"></param>
        /// <param name="item"></param>
        private static void Recursive<T, TPk>(List<T> list, T item, TPk newParentValue, TreeIdParam<T, TPk> param, TreeIdModel<T, TPk> model) where T : class
        {
            var childs = list.Where(p => model.GetParentId.Invoke(p).Equals(model.GetId.Invoke(item))).ToList();

            var newId = param.GetNewId();

            model.SetId.SetValue(item, newId);
            model.SetParentId.SetValue(item, newParentValue);

            foreach (var child in childs)
            {
                Recursive(list, child, newId, param, model);
            }
        }
    }
}
