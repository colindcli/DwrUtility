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
        /// 空格分隔关键词并去重复，始终返回List对象
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="comparer">不区分大小写</param>
        /// <returns>始终返回List对象，不会为null</returns>
        public static List<string> ToKeys(this string keyword, StringComparer comparer = null)
        {
            if (keyword.IsWhiteSpace())
            {
                return new List<string>();
            }

            if (comparer == null)
            {
                comparer = StringComparer.OrdinalIgnoreCase;
            }

            var keys = keyword.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return keys.Distinct(comparer).ToList();
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

        /// <summary>
        /// 从右往左找到字符则停止，返回找到的字符串
        /// 例如：string找字符i，返回ing
        /// </summary>
        /// <param name="str">查找字符串</param>
        /// <param name="cs">遇到字符停止</param>
        /// <param name="outStr">找到的字符串</param>
        /// <param name="rule">对字符串str不存在字符集cs的处理办法，true返回return true和out str，false返回return false和out null</param>
        /// <returns></returns>
        public static bool RightToLeftIndexOfChar(this string str, char[] cs, out string outStr, bool rule)
        {
            return StringUtility.RightToLeftIndexOfChar(str, cs, out outStr, rule);
        }

        /// <summary>
        /// 从左往右找到字符则停止，返回找到的字符串
        /// 例如：string找字符i，返回stri
        /// </summary>
        /// <param name="str">查找字符串</param>
        /// <param name="cs">遇到字符停止</param>
        /// <param name="outStr">找到的字符串</param>
        /// <param name="rule">对字符串str不存在字符集cs的处理办法，true返回return true和out str，false返回return false和out null</param>
        /// <returns></returns>
        public static bool LeftToRightIndexOfChar(this string str, char[] cs, out string outStr, bool rule)
        {
            return StringUtility.LeftToRightIndexOfChar(str, cs, out outStr, rule);
        }

        /// <summary>
        /// 将字符串分割成两部分 (不管它有多少个分割符，都是分割为两部分)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="c">分隔符</param>
        /// <param name="direction">查找字符串方向</param>
        /// <param name="defaultValue">不存在分隔符时默认值</param>
        /// <param name="leftStr">返回分割后左边的字符串</param>
        /// <param name="rightStr">返回分割后右边的字符串</param>
        /// <returns>返回成功失败</returns>
        public static bool SplitString(this string str, char c, Direction direction, DefaultValue defaultValue, out string leftStr, out string rightStr)
        {
            return StringUtility.SplitString(str, c, direction, defaultValue, out leftStr, out rightStr);
        }
    }
}
