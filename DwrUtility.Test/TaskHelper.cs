﻿using DwrUtility.TaskExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace DwrUtility.Test
{
    [TestClass]
    public class TaskHelper
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
    }
}
