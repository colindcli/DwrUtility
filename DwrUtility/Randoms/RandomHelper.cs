using System;
using System.Text;

namespace DwrUtility.Randoms
{
    /// <summary>
    /// 随机数类
    /// </summary>
    public class RandomHelper
    {
        /// <summary>
        /// 小写26个字母
        /// </summary>
        public static readonly string LowerCaseChar = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 大写26个字母
        /// </summary>
        public static readonly string CapitalChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// 数字0到9
        /// </summary>
        public static readonly string NumberChar = "0123456789";

        /// <summary>
        /// 从枚举设定字符中随机产生指定长度len的字符串
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="rde">随机源</param>
        /// <returns></returns>
        public static string GetRandomString(int len, params RandomEnum[] rde)
        {
            if (len <= 0)
            {
                return null;
            }

            var str = new StringBuilder();
            foreach (var item in rde)
            {
                if (item == RandomEnum.LowerCaseChar)
                {
                    str.Append(LowerCaseChar);
                }

                if (item == RandomEnum.CapitalChar)
                {
                    str.Append(CapitalChar);
                }

                if (item == RandomEnum.NumberChar)
                {
                    str.Append(NumberChar);
                }
            }

            return GetRandomString(str.ToString(), len);
        }

        /// <summary>
        /// 从source字符中随机产生指定长度len的字符串
        /// </summary>
        /// <param name="source">随机源</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRandomString(string source, int len)
        {
            if (len <= 0)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            var length = source.Length;
            var rd = new Random();
            var sb = new StringBuilder();
            for (var i = 0; i < len; i++)
            {
                var index = rd.Next(0, length);
                sb.Append(source[index]);
            }

            return sb.ToString();
        }
    }
}
