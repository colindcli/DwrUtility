#if NETFULL
using System;
using System.Diagnostics;
using System.Linq;

namespace DwrUtility.Nets
{
    /// <summary>
    /// 网络速度
    /// </summary>
    public class NetSpeed
    {
        /// <summary>
        /// 
        /// </summary>
        private PerformanceCounter[] SentCounters { get; }
        /// <summary>
        /// 
        /// </summary>
        private PerformanceCounter[] ReceivedCounters { get; }

        /// <summary>
        /// 
        /// </summary>
        public NetSpeed()
        {
            var category = new PerformanceCounterCategory("Network Interface");
            var interfaces = category.GetInstanceNames();
            var length = interfaces.Length;
            if (length > 0)
            {
                SentCounters = new PerformanceCounter[length];
                ReceivedCounters = new PerformanceCounter[length];
            }
            for (var i = 0; i < length; i++)
            {
                ReceivedCounters[i] = new PerformanceCounter("Network Interface", "Bytes Received/sec", interfaces[i]);
                SentCounters[i] = new PerformanceCounter("Network Interface", "Bytes Sent/sec", interfaces[i]);
            }
        }

        /// <summary>
        /// 网络上传速度
        /// </summary>
        public double GetSentSpeed()
        {
            var sendSum = SentCounters.Sum(t => t.NextValue());
            return Math.Round(sendSum / 1024, 1);
        }

        /// <summary>
        /// 网络下载速度
        /// </summary>
        public double GetReceivedSpeed()
        {
            var receiveSum = ReceivedCounters.Sum(t => t.NextValue());
            return Math.Round(receiveSum / 1024, 1);
        }
    }
}

#endif
