using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DwrUtility
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {

        /// <summary>
        /// 检查枚举值是否重复
        /// </summary>
        /// <param name="assemblyString"></param>
        public static void CheckEnum(string assemblyString)
        {
            var enumList = Assembly.Load(assemblyString).GetTypes().Where(p => p.IsEnum).ToList();
            foreach (var el in enumList)
            {
                var vs = Enum.GetValues(el).Cast<int>().ToList();
                var repeats = vs.GetRepeats(p => p);
                if (repeats.Count > 0)
                {
                    throw new Exception($"枚举{el.FullName}重复值: {string.Join(";", repeats)}");
                }
            }
        }

        /// <summary>
        /// 根据值获取描述
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static string GetDescriptionByValue(Type enumType, int value)
        {
            if (!enumType.IsEnum)
            {
                throw new Exception("不是枚举类型");
            }

            var enumItem = Enum.GetName(enumType, value);
            if (enumItem == null)
            {
                return string.Empty;
            }
            var objs = enumType.GetField(enumItem).GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (objs.Length == 0)
            {
                return string.Empty;
            }
            var attr = objs[0] as DescriptionAttribute;
            return attr?.Description;
        }

        /// <summary>
        /// 返回值与描述键值对
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetValueDescriptionDict<TEnum>() where TEnum : struct
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new Exception("不是枚举类型");
            }

            var dict = new Dictionary<int, string>();
            var values = Enum.GetValues(enumType);
            foreach (int v in values)
            {
                var name = GetDescriptionByValue(enumType, v);
                dict.Add(v, name);
            }

            return dict;
        }

        /// <summary>
        /// 返回值与名称键值对
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetValueNameDict<TEnum>() where TEnum : struct
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new Exception("不是枚举类型");
            }

            var dict = new Dictionary<int, string>();
            var values = Enum.GetValues(enumType);
            foreach (int v in values)
            {
                var name = Enum.GetName(enumType, v);
                dict.Add(v, name);
            }

            return dict;
        }
    }

}
