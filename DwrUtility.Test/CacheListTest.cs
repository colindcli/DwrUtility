#if NETFRAMEWORK
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DwrUtility.Test
{
    [TestClass]
    public class CacheListTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var biz = new ClassA();
            var list = biz.GetCache();
            Assert.IsTrue(list.Count == 2 &&
                          list[0].Key == "A1" && list[0].Value == "Av1" &&
                          list[1].Key == "A2" && list[1].Value == "Av2"
            );


            //
            list[0].Value = "Av11";

            var res = biz.GetCache();
            Assert.IsTrue(res.Count == 2 &&
                          res[0].Key == "A1" && res[0].Value == "Av11" &&
                          res[1].Key == "A2" && res[1].Value == "Av2"
                          );
        }

        [TestMethod]
        public void TestMethod2()
        {
            var biz = new ClassB();
            var list = biz.GetCache();
            Assert.IsTrue(list.Count == 2 && list[0] == 1 && list[1] == 2);


            //
            list[0] = 10;

            var res = biz.GetCache();
            Assert.IsTrue(res.Count == 2 && list[0] == 10 && list[1] == 2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var biz = new ClassC();
            var list = biz.GetCache();
            Assert.IsTrue(list.Count == 2 && list[0] == "1" && list[1] == "2");


            //
            list[0] = "10";

            var res = biz.GetCache();
            Assert.IsTrue(res.Count == 2 && list[0] == "10" && list[1] == "2");
        }

        public class ClassA : CacheList<KeyValue>
        {
            internal ClassA() : base("ListClassA")
            {
            }

            public override List<KeyValue> GetData()
            {
                return new List<KeyValue>
                {
                    new KeyValue() { Key = "A1", Value = "Av1" },
                    new KeyValue() { Key = "A2", Value = "Av2" },
                };
            }
        }

        public class ClassB : CacheList<int>
        {
            internal ClassB() : base("ListClassB")
            {
            }

            public override List<int> GetData()
            {
                return new List<int>
                {
                    1,2
                };
            }
        }

        public class ClassC : CacheList<string>
        {
            internal ClassC() : base("ListClassC")
            {
            }

            public override List<string> GetData()
            {
                return new List<string>
                {
                    "1",
                    "2"
                };
            }
        }
    }
}

#endif
