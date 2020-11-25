using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DwrUtility.Strings
{
    internal class StringUtility
    {
        /// <summary>
        /// 在字符串前后插入字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="key">搜索关键词</param>
        /// <param name="preStr">关键词前插入字符串</param>
        /// <param name="endStr">关键词后插入字符串</param>
        /// <param name="comparison">搜索规则</param>
        /// <returns></returns>
        internal static string InsertString(string input, string key, string preStr, string endStr, StringComparison comparison)
        {
            var start = 0;
            int index;
            var keyLen = key.Length;
            var lhKeyLen = $"{preStr}{key}{endStr}".Length;
            var preHasValue = !string.IsNullOrEmpty(preStr);
            var endHasValue = !string.IsNullOrEmpty(endStr);
            while ((index = input.IndexOf(key, start, comparison)) > -1)
            {
                if (endHasValue)
                {
                    input = input.Insert(index + keyLen, endStr);
                }
                if (preHasValue)
                {
                    input = input.Insert(index, preStr);
                }

                start = index + lhKeyLen;
            }

            return input;
        }

        /// <summary>
        /// 关键词添加高亮
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="keys">搜索关键词</param>
        /// <param name="comparison">搜索规则</param>
        /// <param name="className">html标签类名</param>
        /// <returns></returns>
        internal static string ToKeywordHighlighting(string input, List<string> keys, StringComparison comparison, string className)
        {
            foreach (var key in keys)
            {
                input = InsertString(input, key, $"<span class=\"{className}\">", "</span>", comparison);
            }

            return input;
        }

        /// <summary>
        /// 是否包含搜索关键词
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keys"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        internal static bool HasSearchKeys(string input, List<string> keys, StringComparison comparison)
        {
            var flag = true;
            foreach (var key in keys)
            {
                if (input.IndexOf(key, comparison) == -1)
                {
                    flag = false;
                }
            }
            return flag;
        }

        private static readonly Regex Reg = new Regex("[\u4e00-\u9fbb]", RegexOptions.Singleline);

        /// <summary>
        /// 是否包含汉语
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static bool IsContainChinese(string input)
        {
            return Reg.IsMatch(input);
        }

        /// <summary>
        /// 汉语数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static int CountChinese(string input)
        {
            return Reg.Matches(input).Count;
        }
    }
}
