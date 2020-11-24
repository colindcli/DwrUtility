#if NETFRAMEWORK
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class CacheObjTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var biz = new ClassA();
            var list = biz.GetCache();
            Assert.IsTrue(list.Key == "A1" && list.Value == "Av1");

            //
            list.Value = "Av11";
            biz.SetCache(list);

            var res = biz.GetCache();
            Assert.IsTrue(res.Key == "A1" && res.Value == "Av11");
        }

        //[TestMethod]
        //public void TestMethod2()
        //{
        //    var biz = new ClassB();
        //    var list = biz.GetCache();
        //    Assert.IsTrue(list == 1);


        //    //
        //    biz.SetCache(10);

        //    var res = biz.GetCache();
        //    Assert.IsTrue(res == 10);
        //}

        [TestMethod]
        public void TestMethod3()
        {
            var biz = new ClassC();
            var list = biz.GetCache();
            Assert.IsTrue(list == "1");

            //
            biz.SetCache("10");

            var res = biz.GetCache();
            Assert.IsTrue(res == "10");
        }

        public class ClassA : CacheObj<KeyValue>
        {
            internal ClassA() : base("ObjClassA")
            {
            }

            public override KeyValue GetData()
            {
                return new KeyValue() { Key = "A1", Value = "Av1" };
            }
        }

        //public class ClassB : CacheObj<int>
        //{
        //    internal ClassB() : base("ObjClassB")
        //    {
        //    }

        //    public override int GetData()
        //    {
        //        return 1;
        //    }
        //}

        public class ClassC : CacheObj<string>
        {
            internal ClassC() : base("ObjClassC")
            {
            }

            public override string GetData()
            {
                return "1";
            }
        }
    }
}

#endif
