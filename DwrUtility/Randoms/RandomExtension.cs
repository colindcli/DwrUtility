﻿namespace DwrUtility.Randoms
{
    /// <summary>
    /// 
    /// </summary>
    public static class RandomExtension
    {
        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="source">随机源</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRandomString(this string source, int len)
        {
            return RandomHelper.GetRandomString(source, len);
        }
    }
}
