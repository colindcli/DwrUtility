using DwrUtility.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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

        [TestMethod]
        public void TestMethod2()
        {
            var str = "小飞兔整站下载";
            var b = str.IsContainChinese();
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var str = "小飞兔整站下载V3.0";
            var b = str.CountChinese() == 7;
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var end1 = (int)'\u9fa5';
            var end2 = (int)'\u9fbb';

            var list = new List<string>();
            for (var i = end1; i <= end2; i++)
            {
                list.Add(((char)i).ToString());
            }

            var s = string.Join("", list);
            var b = s == "龥龦龧龨龩龪龫龬龭龮龯龰龱龲龳龴龵龶龷龸龹龺龻";

            Assert.IsTrue(b);
        }
    }
}
