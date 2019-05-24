namespace DwrUtility.Maths
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathExtension
    {
        /// <summary>
        /// 转汇（有效位数后各位数>0则向前进1模式，默认保留两位有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="rate"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToExchangeRoundUp(this decimal d, decimal rate, int digits = 2)
        {
            return MathHelper.ToExchangeRoundUp(d, rate, digits);
        }

        /// <summary>
        /// 转汇（有效位数后面位数丢弃模式，默认保留两位有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="rate"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToExchangeRoundDown(this decimal d, decimal rate, int digits = 2)
        {
            return MathHelper.ToExchangeRoundDown(d, rate, digits);
        }

        /// <summary>
        /// 转汇（四舍五入，默认保留两位有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="rate"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToExchange(this decimal d, decimal rate, int digits = 2)
        {
            return MathHelper.ToExchange(d, rate, digits);
        }

        /// <summary>
        /// 转百分比（四舍五入，默认不保留有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static string ToPercentage(this decimal d, int digits = 0)
        {
            return MathHelper.ToPercentage(d, digits);
        }

        /// <summary>
        /// 转字符串（四舍五入，默认保留两位有效数字）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static string ToString(this decimal d, int digits = 2)
        {
            return MathHelper.ToString(d, digits);
        }

        /// <summary>
        /// 转字符串（有效位数后各位数>0则向前进1模式）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static string ToStringRoundUp(this decimal d, int digits)
        {
            return MathHelper.ToStringRoundUp(d, digits);
        }

        /// <summary>
        /// 转字符串（有效位数后面位数丢弃模式）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static string ToStringRoundDown(this decimal d, int digits)
        {
            return MathHelper.ToStringRoundDown(d, digits);
        }

        /// <summary>
        /// 保留指定有效位数（有效位数后各位数>0则向前进1模式）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static decimal ToRoundUp(this decimal d, int digits)
        {
            return MathHelper.ToRoundUp(d, digits);
        }

        /// <summary>
        /// 保留指定有效位数（有效位数后面位数丢弃模式）
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits">保留位数</param>
        /// <returns></returns>
        public static decimal ToRoundDown(this decimal d, int digits)
        {
            return MathHelper.ToRoundDown(d, digits);
        }
    }
}
