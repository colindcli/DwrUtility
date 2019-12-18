using DwrUtility.Randoms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Test
{
    [TestClass]
    public class RandomHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var flag = false;
            for (var i = 0; i < 1000; i++)
            {
                var str = "ab".GetRandomString(2);
                var s = str.Select(p => p).Distinct().ToList();
                if (s.Count > 0)
                {
                    flag = true;
                }
            }

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var b1 = RandomHelper.CapitalChar.Select(p => p).Distinct().Count() == 26;
            var b2 = RandomHelper.LowerCaseChar.Select(p => p).Distinct().Count() == 26;
            var b3 = RandomHelper.NumberChar.Select(p => p).Distinct().Count() == 10;

            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var b1 = "".GetRandomString(5) == null;
            var b2 = ((string)null).GetRandomString(5) == null;

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var ids = new List<int>();
            for (var i = 0; i < 100000; i++)
            {
                ids.Add(RandomHelper.New(5));
            }

            var min = ids.Min();
            var max = ids.Max();

            Assert.IsTrue(min == 0 && max == 4);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var ids = new List<int>();
            for (var i = 0; i < 100000; i++)
            {
                ids.Add(RandomHelper.New(2, 5));
            }

            var min = ids.Min();
            var max = ids.Max();

            Assert.IsTrue(min == 2 && max == 4);
        }
    }
}
