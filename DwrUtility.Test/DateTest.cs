using System;
using DwrUtility.Dates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class DateTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var beginDate = Convert.ToDateTime("2019-07-02");
            var endDate = Convert.ToDateTime("2019-07-30 23:59:59");

            var b1 = !DateHelper.HasDateIntersection(beginDate, endDate,
                Convert.ToDateTime("2019-07-01"),
                Convert.ToDateTime("2019-07-01")
            );
            var b2 = DateHelper.HasDateIntersection(beginDate, endDate,
                Convert.ToDateTime("2019-07-01"),
                Convert.ToDateTime("2019-07-02")
            );
            var b3 = DateHelper.HasDateIntersection(beginDate, endDate,
                Convert.ToDateTime("2019-07-02"),
                Convert.ToDateTime("2019-07-02")
            );
            var b4 = DateHelper.HasDateIntersection(beginDate, endDate,
                Convert.ToDateTime("2019-07-30"),
                Convert.ToDateTime("2019-07-30")
            );
            var b5 = DateHelper.HasDateIntersection(beginDate, endDate,
                Convert.ToDateTime("2019-07-30"),
                Convert.ToDateTime("2019-07-31")
            );
            var b6 = !DateHelper.HasDateIntersection(beginDate, endDate,
                Convert.ToDateTime("2019-07-31"),
                Convert.ToDateTime("2019-07-31")
            );

            Assert.IsTrue(
                b1 &&
                b2 &&
                b3 &&
                b4 &&
                b5 &&
                b6
            );
        }

        [TestMethod]
        public void TestMethod2()
        {
            var today = Convert.ToDateTime("2019-07-17");
            DateHelper.GetDateRange(today, DateTypeEnum.Today, out var bd, out var ed);

            var b1 = bd == Convert.ToDateTime("2019-07-17");
            var b2 = ed == Convert.ToDateTime("2019-07-17 23:59:59");

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var today = Convert.ToDateTime("2019-07-17");
            DateHelper.GetDateRange(today, DateTypeEnum.ThisWeek, out var bd, out var ed);

            var b1 = bd == Convert.ToDateTime("2019-07-15");
            var b2 = ed == Convert.ToDateTime("2019-07-21 23:59:59");

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var today = Convert.ToDateTime("2019-07-17");
            DateHelper.GetDateRange(today, DateTypeEnum.ThisMonth, out var bd, out var ed);

            var b1 = bd == Convert.ToDateTime("2019-07-01");
            var b2 = ed == Convert.ToDateTime("2019-07-31 23:59:59");

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var today = Convert.ToDateTime("2019-07-17");
            DateHelper.GetDateRange(today, DateTypeEnum.ThisYear, out var bd, out var ed);

            var b1 = bd == Convert.ToDateTime("2019-01-01");
            var b2 = ed == Convert.ToDateTime("2019-12-31 23:59:59");

            Assert.IsTrue(b1 && b2);
        }
    }
}
