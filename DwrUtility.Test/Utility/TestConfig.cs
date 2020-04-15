using System;
using System.IO;

namespace DwrUtility.Test.Utility
{
    public class TestConfig
    {
        /// <summary>
        /// 请求地址: http://localhost:7000
        /// </summary>
        public static readonly string WebUrl = "http://localhost:7000";

        /// <summary>
        /// Test/Debug/bin/Debug/目录，包含“/”
        /// </summary>
        public static readonly string BinDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEndSlash()}/";

        /// <summary>
        /// 到Test目录，包含“/”
        /// </summary>
        public static readonly string TestDir = Path.GetFullPath($"{BinDir}../../");
        /// <summary>
        /// 到Web目录，包含“/”
        /// </summary>
        public static readonly string WebDir = Path.GetFullPath($"{BinDir}../../../DwrUtility.Web/");
    }
}
