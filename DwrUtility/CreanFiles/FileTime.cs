using System;
using System.Collections.Generic;

namespace DwrUtility.CreanFiles
{
    /// <summary>
    /// 
    /// </summary>
    public class FileTime
    {
        /// <summary>
        /// 文件创建后超过多长时间做处理
        /// </summary>
        public TimeSpan DeleteTime { get; set; } = TimeSpan.FromDays(30);
        /// <summary>
        /// 处理的文件夹集合
        /// </summary>
        public List<string> Directories { get; set; } = new List<string>();
    }
}
