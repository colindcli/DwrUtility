using System;
using System.Collections.Generic;

namespace DwrUtility.CreanFiles
{
    /// <summary>
    /// 清理文件参数
    /// </summary>
    public class CreanFileParam
    {
        /// <summary>
        /// 文件夹文件保留时长
        /// </summary>
        public List<FileTime> FileTimes { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        public Action<Exception> Log { get; set; }
        /// <summary>
        /// 定时器Timer参数：延时启动，默认TimeSpan.Zero立即启动
        /// </summary>
        public TimeSpan DueTime { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// 定时器Timer参数：时间间隔，默认一小时
        /// </summary>
        public TimeSpan Period { get; set; } = TimeSpan.FromHours(1);
    }
}
