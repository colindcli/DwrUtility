using System;

namespace DwrUtility
{
    /// <summary>
    /// DwrUtility全局设置
    /// </summary>
    public class DwrUtilitySetting
    {
#if DEBUG
        /// <summary>
        /// 日志输出
        /// </summary>
        public static Action<Exception> Log { get; set; } = ex => throw ex;
#else
        /// <summary>
        /// 日志输出
        /// </summary>
        public static Action<Exception> Log { get; set; }
#endif
    }
}
