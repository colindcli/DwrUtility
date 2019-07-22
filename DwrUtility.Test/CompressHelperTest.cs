using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace DwrUtility.Test
{
    [TestClass]
    public class CompressHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = "123";

            var bt = Encoding.UTF8.GetBytes(str);
            var enBt = CompressHelper.Compress(bt);

            var deBt = CompressHelper.Decompress(enBt);
            var res = Encoding.UTF8.GetString(deBt);

            Assert.IsTrue(str == res);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var str = "~!@#$%^&*()_+|\":?><";

            var bt = Encoding.UTF8.GetBytes(str);
            var enBt = CompressHelper.Compress(bt);

            var deBt = CompressHelper.Decompress(enBt);
            var res = Encoding.UTF8.GetString(deBt);

            Assert.IsTrue(str == res);
        }
    }
}
