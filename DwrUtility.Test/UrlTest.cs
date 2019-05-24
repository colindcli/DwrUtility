using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class UrlTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var url = "http://www.demo.com";
            var uri = url.ToUri();

            Assert.IsTrue(uri.AbsoluteUri == "http://www.demo.com/");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var url = "http://www.demo.com/";
            var uri = url.ToUri();

            Assert.IsTrue(uri.AbsoluteUri == "http://www.demo.com/");
        }

        [TestMethod]
        public void TestMethod3()
        {
            var url = "//www.demo.com/index.html";
            var uri = url.ToUri();

            Assert.IsTrue(uri.ToString() == "file://www.demo.com/index.html");
        }

        [TestMethod]
        public void TestMethod4()
        {
            var url = "://www.demo.com/index.html";
            var uri = url.ToUri();

            Assert.IsTrue(uri == null);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var url = "/index.html";
            var uri = url.ToUri();

            Assert.IsTrue(uri == null);
        }
    }
}
