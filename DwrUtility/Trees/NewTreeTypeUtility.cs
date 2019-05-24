using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Trees
{
    /// <summary>
    /// 转换树Id类型（在原树上修改Id）
    /// </summary>
    sealed class NewTreeTypeUtility
    {
        /// <summary>
        /// 转换树Id类型（在原树上修改Id）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TToType"></typeparam>
        /// <param name="param"></param>
        public static void Generate<T, TType, TToType>(TreeTypeParam<T, TType, TToType> param) where T : class
        {
            if (param.TreeLists.Count == 0)
            {
                return;
            }

            if (param.TreeLists.Count > param.NewIds.Count)
            {
                throw new Exception($"新Id数量不足");
            }

            if (param.NewIds.Contains(default(TToType)))
            {
                throw new Exception($"新Id不能含有值{default(TToType)}");
            }

            var model = new TreeTypeModel<T, TType, TToType>();
            var type = typeof(T);
            model.GetId = param.IdField.Compile();
            model.SetId = type.GetProperty(param.IdField.GetPropertyName());
            model.GetParentId = param.ParentIdField.Compile();
            model.SetParentId = type.GetProperty(param.ParentIdField.GetPropertyName());
            model.GetToId = param.ToIdField.Compile();
            model.SetToId = type.GetProperty(param.ToIdField.GetPropertyName());
            model.GetToParentId = param.ToParentIdField.Compile();
            model.SetToParentId = type.GetProperty(param.ToParentIdField.GetPropertyName());

            Begin(param, model);
        }

        /// <summary>
        /// 转换树Id类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TToType"></typeparam>
        /// <param name="param"></param>
        /// <param name="model"></param>
        private static void Begin<T, TType, TToType>(TreeTypeParam<T, TType, TToType> param, TreeTypeModel<T, TType, TToType> model) where T : class
        {
            //顶级节点
            var topNodes = param.TreeLists.GetTopNodes(model.GetId, model.GetParentId);

            foreach (var item in topNodes)
            {
                var childs = param.TreeLists.GetChildNodes(model.GetId.Invoke(item), model.GetId, model.GetParentId, false);
                Recursive(childs, item, default(TToType), param, model);
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
        private static void Recursive<T, TType, TToType>(List<T> list, T item, TToType newParentValue, TreeTypeParam<T, TType, TToType> param, TreeTypeModel<T, TType, TToType> model) where T : class
        {
            var childs = list.Where(p => model.GetParentId.Invoke(p).Equals(model.GetId.Invoke(item))).ToList();

            var newId = param.GetNewId();

            model.SetToId.SetValue(item, newId);
            model.SetToParentId.SetValue(item, newParentValue);

            foreach (var child in childs)
            {
                Recursive(list, child, newId, param, model);
            }
        }
    }
}
