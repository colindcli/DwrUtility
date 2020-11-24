using DwrUtility.Lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Test
{
    [TestClass]
    public class EqualityComparerTest
    {
        /// <summary>
        /// 对象去重
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            var list = new List<Source>()
            {
                new Source()
                {
                    Id = 1,
                    Value = "Value1"
                },
                new Source()
                {
                    Id = 1,
                    Value = "value1"
                },
                new Source()
                {
                    Id = 1,
                    Value = "Value2"
                },
            };

            var rows = list.Distinct(new EqualityComparer<Source>((p, q) => p.Id == q.Id));
            Assert.IsTrue(rows.Count() == 1);

            var rows1 = list.ToDist((p, q) => p.Id == q.Id);
            Assert.IsTrue(rows1.Count() == 1);

            var rows2 = list.Distinct(new EqualityComparer<Source>((p, q) => p.Id == q.Id && p.Value == q.Value));
            Assert.IsTrue(rows2.Count() == 3);

            var rows3 = list.Distinct(new EqualityComparer<Source>((p, q) => p.Id == q.Id && p.Value.ToLower() == q.Value.ToLower()));
            Assert.IsTrue(rows3.Count() == 2);
        }
        /// <summary>
        /// 对象去重
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            var list = new List<Source>()
            {
                new Source()
                {
                    Id = 1,
                    Value = "Value1"
                },
                new Source()
                {
                    Id = 1,
                    Value = "value1"
                },
                new Source()
                {
                    Id = 1,
                    Value = "Value2"
                },
            };

            var rows = list.ToDist((p, q) => p.Id == q.Id).ToList();
            Assert.IsTrue(rows.Count() == 1);

            var rows2 = list.ToDist((p, q) => p.Value == q.Value).ToList();
            Assert.IsTrue(rows2.Count() == 3);

            var rows3 = list.ToDist((p, q) => string.Equals(p.Value, q.Value, StringComparison.OrdinalIgnoreCase)).ToList();
            Assert.IsTrue(rows3.Count() == 2);

            var rows4 = list.ToDist((p, q) => string.Equals(p.Value, q.Value, StringComparison.Ordinal)).ToList();
            Assert.IsTrue(rows4.Count() == 3);
        }

        public class Source
        {
            public int Id { get; set; }

            public string Value { get; set; }
        }
    }
}
