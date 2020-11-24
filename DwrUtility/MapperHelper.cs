using System.Collections.Generic;
using System.Reflection;

namespace DwrUtility
{
    /// <summary>
    /// 对象映射
    /// </summary>
    public class MapperHelper
    {
        /// <summary>
        /// 对象转换
        /// </summary>
        /// <typeparam name="TSource">源</typeparam>
        /// <typeparam name="TResult">目标</typeparam>
        /// <param name="obj">转换对象</param>
        /// <returns></returns>
        public static TResult Mapper<TSource, TResult>(TSource obj) where TResult : new()
        {
            if (obj == null)
            {
                return default(TResult);
            }

            var propertiesT = typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (propertiesT.Length == 0)
            {
                return default(TResult);
            }

            var propertiesS = typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var setT = new TResult();
            foreach (var itemL in propertiesT)
            {
                foreach (var itemT in propertiesS)
                {
                    if (itemL.Name != itemT.Name)
                    {
                        continue;
                    }

                    if (!itemT.CanRead)
                    {
                        continue;
                    }

                    if (!itemL.CanWrite)
                    {
                        continue;
                    }

                    var value = itemT.GetValue(obj, null);
                    itemL.SetValue(setT, value, null);
                }
            }
            return setT;
        }

        /// <summary>
        /// 对象转换
        /// </summary>
        /// <typeparam name="TSource">源</typeparam>
        /// <typeparam name="TResult">目标</typeparam>
        /// <param name="list">转换对象</param>
        /// <returns></returns>
        public static List<TResult> Mapper<TSource, TResult>(List<TSource> list) where TResult : new()
        {
            if (list == null)
            {
                return null;
            }

            var propertiesT = typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (propertiesT.Length == 0)
            {
                return null;
            }

            var propertiesS = typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var row = new List<TResult>();
            foreach (var li in list)
            {
                var setT = new TResult();
                foreach (var itemL in propertiesT)
                {
                    foreach (var itemT in propertiesS)
                    {
                        if (itemL.Name != itemT.Name)
                        {
                            continue;
                        }

                        if (!itemT.CanRead)
                        {
                            continue;
                        }

                        if (!itemL.CanWrite)
                        {
                            continue;
                        }

                        var value = itemT.GetValue(li, null);
                        itemL.SetValue(setT, value, null);
                    }
                }
                row.Add(setT);
            }

            return row;
        }
    }
}
