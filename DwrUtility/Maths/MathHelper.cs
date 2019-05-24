using System;
using System.Globalization;

namespace DwrUtility.Maths
{
    /// <summary>
    /// 数值帮助类
    /// </summary>
    public class MathHelper
    {
        /// <summary>
        /// 转汇（有效位数后各位数>0则向前进1模式，默认保留两位有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="rate"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToExchangeRoundUp(decimal d, decimal rate, int digits = 2)
        {
            return ToStringRoundUp(d * rate, digits);
        }

        /// <summary>
        /// 转汇（有效位数后面位数丢弃模式，默认保留两位有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="rate"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToExchangeRoundDown(decimal d, decimal rate, int digits = 2)
        {
            return ToStringRoundDown(d * rate, digits);
        }

        /// <summary>
        /// 转汇（四舍五入，默认保留两位有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="rate"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToExchange(decimal d, decimal rate, int digits = 2)
        {
            return ToString(d * rate, digits);
        }

        /// <summary>
        /// 转百分比（四舍五入，默认不保留有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static string ToPercentage(decimal d, int digits = 0)
        {
            return d.ToString($"P{digits}");
        }

        /// <summary>
        /// 转字符串（四舍五入，默认保留两位有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static string ToString(decimal d, int digits = 2)
        {
            return d.ToString($"N{digits}");
        }

        /// <summary>
        /// 转字符串（有效位数后各位数>0则向前进1模式）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static string ToStringRoundUp(decimal d, int digits)
        {
            return ToString(ToRoundUp(d, digits), digits);
        }

        /// <summary>
        /// 转字符串（有效位数后面位数丢弃模式）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static string ToStringRoundDown(decimal d, int digits)
        {
            return ToString(ToRoundDown(d, digits), digits);
        }

        /// <summary>
        /// 保留指定有效位数（有效位数后面位数丢弃模式）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static decimal ToRoundDown(decimal d, int digits)
        {
            var numStr = "1";
            for (var i = 0; i < digits; i++)
            {
                numStr += "0";
            }

            var num = Convert.ToInt32(numStr);

            var dTemp = d * num;
            var dTempInt = dTemp.ToString(CultureInfo.InvariantCulture).Split('.')[0];

            return Convert.ToDecimal(dTempInt) / num;
        }

        /// <summary>
        /// 保留指定有效位数（有效位数后各位数>0则向前进1模式）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static decimal ToRoundUp(decimal d, int digits)
        {
            var numStr = "1";
            for (var i = 0; i < digits; i++)
            {
                numStr += "0";
            }

            var num = Convert.ToInt32(numStr);

            var dTemp = d * num;
            var dTempInt = Convert.ToInt32(dTemp.ToString(CultureInfo.InvariantCulture).Split('.')[0]);
            if (dTemp > dTempInt)
            {
                dTempInt += 1;
            }

            return Convert.ToDecimal(dTempInt) / num;
        }
    }
}
