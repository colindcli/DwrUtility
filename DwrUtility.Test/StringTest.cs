using System;
using System.Collections.Generic;
using DwrUtility.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class StringTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = "小飞兔整站下载";
            var keys = new List<string>()
            {
                "小飞兔",
                "下载"
            };

            var b = str.HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false);

            Assert.IsTrue(b);
        }
    }
}
