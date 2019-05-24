using System;
using System.Linq;

namespace DwrUtility.Trees
{
    /// <summary>
    /// 设置树父集合和深度（在原树上修改Id）
    /// </summary>
    internal sealed class NewTreeParentsDepthUtility
    {
        /// <summary>
        /// 设置树父集合和深度（在原树上修改Id）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <param name="param"></param>
        public static void Generate<T, TPk>(TreeParentsDepthParam<T, TPk> param) where T : class
        {
            if (param.TreeLists.Count == 0)
            {
                return;
            }

            if (param.ParentIdsField == null && param.DepthField == null)
            {
                return;
            }

            var model = new TreeParentsDepthModel<T>();
            var type = typeof(T);
            if (param.ParentIdsField != null)
            {
                model.GetParentIds = param.ParentIdsField.Compile();
                model.SetParentIds = type.GetProperty(param.ParentIdsField.GetPropertyName());
                model.Split = param.Split.ToString();
            }

            if (param.DepthField != null)
            {
                model.GetDepth = param.DepthField.Compile();
                model.SetDepth = type.GetProperty(param.DepthField.GetPropertyName());
            }

            Begin(param, model);

            foreach (var item in param.TreeLists)
            {
                if (!string.IsNullOrWhiteSpace(param.PrefixParentIds))
                {
                    var pidsStr = $"{param.PrefixParentIds}{model.GetParentIds.Invoke(item)}";
                    var array = pidsStr.Split(new[] { param.Split }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
                    model.SetParentIds?.SetValue(item, string.Join(model.Split, array));
                }

                if (param.PrefixDepth != 0)
                {
                    model.SetDepth?.SetValue(item, param.PrefixDepth + model.GetDepth.Invoke(item));
                }
            }
        }

        private static void Begin<T, TPk>(TreeParentsDepthParam<T, TPk> param, TreeParentsDepthModel<T> model) where T : class
        {
            //顶级节点
            var topNodes = param.TreeLists.GetTopNodes(param.IdField, param.ParentIdField);

            foreach (var item in topNodes)
            {
                Recursive(param, model, item, $"{model.Split}{param.ParentIdField.Invoke(item)}{model.Split}", 1);
            }
        }

        private static void Recursive<T, TPk>(TreeParentsDepthParam<T, TPk> param, TreeParentsDepthModel<T> model, T item, string parentIds, int depth)
        {
            var row = param.TreeLists.Find(p => param.IdField.Invoke(p).Equals(param.IdField.Invoke(item)));
            if (row == null)
            {
                return;
            }

            model.SetParentIds?.SetValue(row, parentIds);
            model.SetDepth?.SetValue(row, depth);

            var childs = param.TreeLists.Where(p => param.ParentIdField.Invoke(p).Equals(param.IdField.Invoke(row))).ToList();
            depth++;
            foreach (var child in childs)
            {
                Recursive(param, model, child, $"{parentIds}{param.IdField.Invoke(row)}{model.Split}", depth);
            }
        }
    }
}
