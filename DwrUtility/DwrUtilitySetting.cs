using System;

namespace DwrUtility
{
    /// <summary>
    /// DwrUtility全局设置
    /// </summary>
    public class DwrUtilitySetting
    {
        private static string _root { get; set; }
        /// <summary>
        /// 根目录，不包含"/"
        /// </summary>
        public static string Root
        {
            // ReSharper disable once ConvertToNullCoalescingCompoundAssignment
            get => _root ?? (_root = AppDomain.CurrentDomain.BaseDirectory.TrimEndSlash());
            set => _root = value.TrimEndSlash();
        }

#if NETCOREAPP3_0
        /// <summary>
        /// wwwroot目录，不包含"/"
        /// </summary>
        public static string RootWwwroot
        {
            // ReSharper disable once ConvertToNullCoalescingCompoundAssignment
            get => _root ?? (_root = AppDomain.CurrentDomain.BaseDirectory.TrimEndSlash() + "/wwwroot");
            set => _root = value.TrimEndSlash();
        }
#endif

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
