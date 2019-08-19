using System;

namespace DwrUtility.Dates
{
    /// <summary>
    /// 日期类
    /// </summary>
    public class DateHelper
    {
        /// <summary>
        /// 获取日期段: 返回今天、本周(周一到周日)、本月、本年日期段
        /// </summary>
        /// <param name="today"></param>
        /// <param name="dateType">类型：1今天；2本周；3本月</param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public static void GetDateRange(DateTime today, DateTypeEnum dateType, out DateTime beginDate, out DateTime endDate)
        {
            //今天
            if (dateType == DateTypeEnum.Today)
            {
                beginDate = today.Date;
                endDate = today.Date.AddDays(1).AddSeconds(-1);
                return;
            }

            //本周
            if (dateType == DateTypeEnum.ThisWeek)
            {
                var wk = (int)today.DayOfWeek;
                beginDate = today.AddDays(-wk + 1);
                endDate = today.AddDays(7 - wk + 1).AddSeconds(-1);
                return;
            }

            //本月
            if (dateType == DateTypeEnum.ThisMonth)
            {
                beginDate = Convert.ToDateTime(today.ToString("yyyy-MM-01"));
                endDate = beginDate.AddMonths(1).AddSeconds(-1);
                return;
            }

            //今年
            if (dateType == DateTypeEnum.ThisYear)
            {
                beginDate = Convert.ToDateTime(today.ToString("yyyy-01-01"));
                endDate = beginDate.AddYears(1).AddSeconds(-1);
                return;
            }

            beginDate = DateTime.MinValue;
            endDate = DateTime.MaxValue;
            return;
        }

        /// <summary>
        /// 两时间段是否有交集
        /// </summary>
        /// <param name="beginRegionDate">开始区间日期</param>
        /// <param name="endRegionDate">结束区间日期</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static bool HasDateIntersection(DateTime beginRegionDate, DateTime endRegionDate, DateTime beginDate,
            DateTime endDate)
        {
            if (endDate < beginRegionDate)
            {
                return false;
            }

            if (beginDate > endRegionDate)
            {
                return false;
            }

            return true;
        }
    }
}
