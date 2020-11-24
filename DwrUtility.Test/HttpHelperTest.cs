using System.Net;
using System.Text;
using DwrUtility.Https;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class HttpHelperTest
    {
        public HttpHelperTest()
        {
#if NETSTANDARD
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }

        [TestMethod]
        public void TestMethod1()
        {
            var url = "http://www.sjsgs.com/";
            var res = HttpRequestHelper.GetData(url);
            Assert.IsTrue(res.IsSuccessful && res.StatusCode == HttpStatusCode.OK);

            var encoding = HttpHelper.GetEncoding(res.RawBytes, res.ContentType);
            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("utf-8")));
            var content = encoding.GetString(res.RawBytes);
            Assert.IsTrue(content.Contains("陕西建工第十建设集团有限公司"));
        }

        /// <summary>
        /// EUC-KR
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            //EUC-KR
            var url = "http://www.bktech.co.kr/";
            var res = HttpRequestHelper.GetData(url);
            Assert.IsTrue(res.IsSuccessful && res.StatusCode == HttpStatusCode.OK);

            var encoding = HttpHelper.GetEncoding(res.RawBytes, res.ContentType);

#if NETSTANDARD
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("EUC-KR")));
            var content = encoding.GetString(res.RawBytes);
            Assert.IsTrue(content.Contains("(주)부광테크"));
        }

        /// <summary>
        /// BIG5，改成utf-8
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            //BIG5 Transfer-Encoding: chunked，改成utf-8
            var url = "http://www.wenweipo.com/";
            var res = HttpRequestHelper.GetData(url);
            Assert.IsTrue(res.IsSuccessful && res.StatusCode == HttpStatusCode.OK);

            var encoding = HttpHelper.GetEncoding(res.RawBytes, res.ContentType);
            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("utf-8")));

            var content = encoding.GetString(res.RawBytes);
            Assert.IsTrue(content.Contains("香港文匯網"));
        }
    }
}
