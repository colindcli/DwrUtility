using System;

namespace DwrUtility.Dates
{
    /// <summary>
    /// 日期扩展类
    /// </summary>
    public static class DateExtension
    {
        /// <summary>
        /// 获取日期段: 返回今天、本周(周一到周日)、本月、本年日期段
        /// </summary>
        /// <param name="today"></param>
        /// <param name="dateType">类型：1今天；2本周；3本月</param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public static void GetDateRange(this DateTime today, DateTypeEnum dateType, out DateTime beginDate,
            out DateTime endDate)
        {
            DateHelper.GetDateRange(today, dateType, out beginDate, out endDate);
        }

        /// <summary>
        /// 转日期字符串: yyyy-MM-dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 转时间字符串: yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateTime(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
