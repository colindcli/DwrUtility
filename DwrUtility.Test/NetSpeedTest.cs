using System.Diagnostics;
using System.Threading;
using DwrUtility.Nets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class NetSpeedTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sw = new Stopwatch();
            sw.Start();
            var ns = new NetSpeed();
            sw.Stop();
            WriteHelper.Log($"初始化耗时：{sw.ElapsedMilliseconds}ms");

            for (var i = 0; i < 5; i++)
            {
                sw.Restart();
                var sent = ns.GetSentSpeed();
                var revice = ns.GetReceivedSpeed();
                sw.Stop();

                WriteHelper.Log($"接收：{revice}kb；发送：{sent}kb；耗时：{sw.ElapsedMilliseconds}ms");
                Thread.Sleep(1000);
            }
        }
    }
}
