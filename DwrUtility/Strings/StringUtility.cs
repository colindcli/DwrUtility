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

        /// <summary>
        /// 从右往左找到字符则停止，返回找到的字符串
        /// 例如：string找字符i，返回ing
        /// </summary>
        /// <param name="str">查找字符串</param>
        /// <param name="cs">遇到字符停止</param>
        /// <param name="outStr">找到的字符串</param>
        /// <param name="rule">对字符串str不存在字符集cs的处理办法，true返回return true和out str，false返回return false和out null</param>
        /// <returns></returns>
        internal static bool RightToLeftIndexOfChar(string str, char[] cs, out string outStr, bool rule)
        {
            outStr = null;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            if (cs == null || cs.Length == 0)
            {
                return false;
            }

            var index = str.LastIndexOfAny(cs);
            if (index == -1)
            {
                if (rule)
                {
                    outStr = str;
                    return true;
                }

                return false;
            }

            outStr = str.Substring(index, str.Length - index);
            return true;
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
        internal static bool LeftToRightIndexOfChar(string str, char[] cs, out string outStr, bool rule)
        {
            outStr = null;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            if (cs == null || cs.Length == 0)
            {
                return false;
            }

            var index = str.IndexOfAny(cs);
            if (index == -1)
            {
                if (rule)
                {
                    outStr = str;
                    return true;
                }

                return false;
            }

            outStr = str.Substring(0, index + 1);
            return true;
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
        internal static bool SplitString(string str, char c, Direction direction, DefaultValue defaultValue, out string leftStr, out string rightStr)
        {
            leftStr = rightStr = null;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            var index = direction == Direction.LeftRight ? str.IndexOf(c) : str.LastIndexOf(c);
            if (index == -1)
            {
                if (defaultValue == DefaultValue.TrueAndValue)
                {
                    if (direction == Direction.LeftRight)
                    {
                        leftStr = str;
                        rightStr = "";
                        return true;
                    }
                    leftStr = "";
                    rightStr = str;
                    return true;
                }

                return false;
            }

            leftStr = str.Substring(0, index);
            rightStr = str.Substring(index + 1, str.Length - index - 1);
            return true;
        }
    }
}
