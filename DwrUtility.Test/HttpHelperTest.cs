using DwrUtility.Https;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;

namespace DwrUtility.Test
{
    [TestClass]
    public class HttpHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var url = "http://www.sjsgs.com/";
            var res = HttpRequestHelper.GetData(url);
            Assert.IsTrue(res.IsSuccessful && res.StatusCode == HttpStatusCode.OK);

            var encoding = HttpHelper.GetEncoding(res.RawBytes, res.ContentType);
            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("gb2312")));
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
            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("EUC-KR")));
            var content = encoding.GetString(res.RawBytes);
            Assert.IsTrue(content.Contains("(주)부광테크"));
        }

        /// <summary>
        /// BIG5
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            //BIG5 Transfer-Encoding: chunked
            var url = "http://www.wenweipo.com/";
            var res = HttpRequestHelper.GetData(url);
            Assert.IsTrue(res.IsSuccessful && res.StatusCode == HttpStatusCode.OK);

            var encoding = HttpHelper.GetEncoding(res.RawBytes, res.ContentType);
            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("BIG5")));

            var content = encoding.GetString(res.RawBytes);
            Assert.IsTrue(content.Contains("香港文匯網"));
        }
    }
}
