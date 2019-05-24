using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class RelativeUrlTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var baseUrl = "http://www.demo.com/index.html";
            var relativeUrl = "default.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/default.html"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var baseUrl = "http://www.demo.com/index.html";
            var relativeUrl = "";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/index.html"));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var baseUrl = "http://www.demo.com/index.html";
            var relativeUrl = "Home/Index";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/Home/Index"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            var baseUrl = "http://www.demo.com/index.html";
            string relativeUrl = null;

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/index.html"));
        }

        [TestMethod]
        public void TestMethod5()
        {
            var baseUrl = "http://www.demo.com/index.html";
            var relativeUrl = "/default.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/default.html"));
        }

        [TestMethod]
        public void TestMethod7()
        {
            var baseUrl = "http://www.demo.com/test/index.html";
            var relativeUrl = "";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/test/index.html"));
        }


        [TestMethod]
        public void TestMethod8()
        {
            var baseUrl = "http://www.demo.com/test/index.html";
            var relativeUrl = "/default.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/default.html"));
        }

        [TestMethod]
        public void TestMethod9()
        {
            var baseUrl = "http://www.demo.com/test/index.html";
            var relativeUrl = "/Next/default.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/Next/default.html"));
        }

        [TestMethod]
        public void TestMethod10()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/demo/test/index.html"));
        }

        [TestMethod]
        public void TestMethod11()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/demo/test/def.html"));
        }

        [TestMethod]
        public void TestMethod12()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/def.html"));
        }

        [TestMethod]
        public void TestMethod13()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "/td/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/td/def.html"));
        }

        [TestMethod]
        public void TestMethod14()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "/td/rr/ww/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/td/rr/ww/def.html"));
        }

        [TestMethod]
        public void TestMethod15()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "td/rr/ww/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/demo/test/td/rr/ww/def.html"));
        }

        [TestMethod]
        public void TestMethod16()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "../td/rr/ww/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/demo/td/rr/ww/def.html"));
        }

        [TestMethod]
        public void TestMethod17()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "../../td/rr/ww/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/td/rr/ww/def.html"));
        }

        [TestMethod]
        public void TestMethod18()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "../../../td/rr/ww/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/td/rr/ww/def.html"));
        }

        [TestMethod]
        public void TestMethod19()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "//td/rr/ww/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://td/rr/ww/def.html"));
        }

        [TestMethod]
        public void TestMethod20()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var relativeUrl = "/td/rr//ww/def.html";

            var baseUri = baseUrl.ToUri();

            var url = RelativeHelper.RelativePathToUrl(baseUri, relativeUrl);

            Assert.IsTrue(url.IsEquals("http://www.demo.com/td/rr//ww/def.html"));
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// ToRelative
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>

        [TestMethod]
        public void TestMethod21()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var url = "http://www.demo.com/td/rr//ww/def.html";

            var baseUri = baseUrl.ToUri();
            var uri = url.ToUri();

            var toUrl = RelativeHelper.UriToRelativePath(baseUri, uri);

            Assert.IsTrue(toUrl.IsEquals("../../td/rr//ww/def.html"));
        }

        [TestMethod]
        public void TestMethod22()
        {
            var baseUrl = "http://www.demo.com/demo/test/index.html";
            var url = "http://www.demo.com/demo/rr//ww/def.html";

            var baseUri = baseUrl.ToUri();
            var uri = url.ToUri();

            var toUrl = RelativeHelper.UriToRelativePath(baseUri, uri);

            Assert.IsTrue(toUrl.IsEquals("../rr//ww/def.html"));
        }
    }
}
