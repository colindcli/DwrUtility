using DwrUtility.Lists;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Strings
{
    /// <summary>
    /// 字符串处理
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 在字符串前后插入字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="key">搜索关键词</param>
        /// <param name="preStr">关键词前插入字符串</param>
        /// <param name="endStr">关键词后插入字符串</param>
        /// <param name="ignoreCase">不区分大小写</param>
        /// <returns></returns>
        public static string InsertString(this string input, string key, string preStr, string endStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(key))
            {
                return input;
            }

            return StringUtility.InsertString(input, key, preStr, endStr, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        /// <summary>
        /// 空格分隔关键词并去重复
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToKeys(this string keyword, StringComparer comparer)
        {
            var keys = keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return keys.ToDist(p => p.Trim(), comparer);
        }

        /// <summary>
        /// 给关键词添加高亮标签
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="keys">搜索关键词</param>
        /// <param name="ignoreCase">不区分大小写</param>
        /// <param name="className">html标签类名</param>
        /// <param name="isClearHtml">是否删除html标签</param>
        /// <returns></returns>
        public static string ToKeywordHighlighting(this string input, List<string> keys, bool ignoreCase, string className, bool isClearHtml)
        {
            if (isClearHtml)
            {
                input = input.ClearHtml();
            }

            if (keys == null || keys.Count == 0 || string.IsNullOrEmpty(input))
            {
                return input;
            }

            return StringUtility.ToKeywordHighlighting(input, keys, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal, className);
        }

        /// <summary>
        /// 是否包含搜索关键词
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="keys">搜索关键词</param>
        /// <param name="comparison"></param>
        /// <param name="isClearHtml">是否删除html标签</param>
        /// <returns></returns>
        public static bool HasSearchKeys(this string input, List<string> keys, StringComparison comparison, bool isClearHtml)
        {
            if (isClearHtml)
            {
                input = input.ClearHtml();
            }

            if (keys == null || keys.Count == 0 || string.IsNullOrEmpty(input))
            {
                return false;
            }

            return StringUtility.HasSearchKeys(input, keys, comparison);
        }

        /// <summary>
        /// 是否包含汉语
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsContainChinese(this string input)
        {
            return StringUtility.IsContainChinese(input);
        }

        /// <summary>
        /// 汉语数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int CountChinese(this string input)
        {
            return StringUtility.CountChinese(input);
        }
    }
}
