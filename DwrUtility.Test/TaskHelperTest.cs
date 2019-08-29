using DwrUtility.TaskExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DwrUtility.Test
{
    [TestClass]
    public class TaskHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var t1 = Task.Run(() =>
            {
                Thread.Sleep(200);
                return 1;
            }).SetTimeout(100);
            var b1 = t1.Result == 0;

            var t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 1;
            }).SetTimeout(200);
            var b2 = t2.Result == 1;

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var t1 = Task.Run(() =>
            {
                Thread.Sleep(200);
                return 1;
            }).SetTimeoutResult(100);
            var b1 = t1.Result.Value == 0 && t1.Result.IsTimeout;

            var t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 1;
            }).SetTimeoutResult(200);
            var b2 = t2.Result.Value == 1 && !t2.Result.IsTimeout;

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var t1 = Task.Run(() =>
            {
                Thread.Sleep(200);
                var i = int.Parse("s");
                return 1;
            });

            var t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
                var i = DateTime.Parse("1");
                return 1;
            });

            try
            {
                TaskHelper.WaitAll(t1, t2);
            }
            catch (Exception ex)
            {
                var msg = ex.ToString();
                Assert.IsTrue(msg.IsContains("输入字符串的格式不正确") && msg.IsContains("该字符串未被识别为有效的 DateTime"));
            }
        }
    }
}
