using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace DwrUtility.Test
{
    [TestClass]
    public class StartStopwatchTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sw = new StartStopwatch();

            Thread.Sleep(100);

            var t1 = sw.GetTimeRestart();

            Thread.Sleep(200);

            var t2 = sw.GetTimeRestart();

            Thread.Sleep(1000);

            var t3 = sw.GetTime();

            var b1 = t1 >= 100 && t1 < 110;
            var b2 = t2 >= 200 && t2 < 205;
            var b3 = t3 >= 1000 && t3 < 1005;

            Assert.IsTrue(b1 && b2 && b3);
        }
    }
}
