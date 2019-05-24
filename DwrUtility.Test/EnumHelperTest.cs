using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DwrUtility.Test
{
    [TestClass]
    public class EnumHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dict = EnumHelper.GetValueNameDict<MyEnum>();
            Assert.IsTrue(dict[1] == "QQ" && dict[2] == "WeChat");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var dict = EnumHelper.GetValueDescriptionDict<MyEnum>();
            Assert.IsTrue(dict[1] == "QQ描述" && dict[2] == "WeChat描述");
        }

        [TestMethod]
        public void TestMethod3()
        {
            try
            {
                EnumHelper.CheckEnum("DwrUtility.Test");
                Assert.IsTrue(false, "枚举有重复值");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true, ex.Message);
            }
        }

        public enum MyEnumRep
        {
            Add = 1,
            Edit = 2,
            Delete = 3,
            AddDel = 3
        }

        public enum MyEnum
        {
            [System.ComponentModel.Description("QQ描述")]
            QQ = 1,

            [System.ComponentModel.Description("WeChat描述")]
            WeChat = 2
        }
    }
}
