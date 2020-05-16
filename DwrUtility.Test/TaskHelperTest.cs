using DwrUtility.TaskExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        public void TestMethod4()
        {
            var list = new List<int>()
            {
                6,7,8,9,
                4,5,
                1,2,3,10,11,12,13,14,15,16,17,18,19,20
            };

            //线程运行
            var tasks = list.RunTask(p =>
            {
                Thread.Sleep(p * 100);
                return p;
            });

            //返回线程状态
            var row = new List<int>();
            tasks.TaskStatus(80, (total, rs) =>
            {
                WriteHelper.Log(total.ToString() + " : " + string.Join(";", rs));
                row.AddRange(rs);
            });

            var b = row.Distinct().Count() == 20;
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var t = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "6数据";
            });

            var i = 0;
            t.TaskStatus(100, () =>
            {
                i++;
            });

            Assert.IsTrue(i > 5);
        }
    }
}
