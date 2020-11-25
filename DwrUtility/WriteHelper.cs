using System;
using System.IO;
using System.Text;

namespace DwrUtility
{
    /// <summary>
    /// 写文件
    /// </summary>
    public class WriteHelper
    {
        private static readonly object Obj = new object();

        /// <summary>
        /// 当前的Ticks值
        /// </summary>
        private static long Ticks { get; set; }

        /// <summary>
        /// 追加日志（每次启动写新日志）
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="isBak">是否需要备份</param>
        public static void Log(string content, bool isBak = false)
        {
            if (content == null)
            {
                content = "";
            }
            var path = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEndSlash()}/Logs/log.txt";
            path.CreateDirByFilePath();

            lock (Obj)
            {
                //文件存在
                if (File.Exists(path) && Ticks == 0)
                {
                    //首次启动记录
                    Ticks = DateTime.Now.ToTicks();

                    //备份
                    if (isBak)
                    {
                        var newPath = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEndSlash()}/Logs/log_{Ticks}.txt";
                        if (File.Exists(newPath))
                        {
                            File.Delete(newPath);
                        }
                        File.Move(path, newPath);
                    }
                    //不备份
                    else
                    {
                        File.Delete(path);
                    }
                }

                File.AppendAllText(path, $"{DateTime.Now:HH:mm:ss}：{content}\r\n\r\n", Encoding.GetEncoding("gb2312"));
            }
        }
    }
}
