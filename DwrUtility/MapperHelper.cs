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
        /// <typeparam name="TS">源</typeparam>
        /// <typeparam name="TT">目标</typeparam>
        /// <param name="obj">转换对象</param>
        /// <returns></returns>
        public static TT Mapper<TS, TT>(TS obj) where TT : new()
        {
            if (obj == null)
            {
                return default(TT);
            }
            var propertiesT = typeof(TT).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (propertiesT.Length == 0)
            {
                return default(TT);
            }
            var propertiesS = typeof(TS).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var setT = new TT();
            foreach (var itemL in propertiesT)
            {
                foreach (var itemT in propertiesS)
                {
                    if (itemL.Name != itemT.Name)
                        continue;
                    var value = itemT.GetValue(obj, null);
                    itemL.SetValue(setT, value, null);
                }
            }
            return setT;
        }
    }
}
