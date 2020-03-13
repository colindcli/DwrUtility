using DwrUtility.Https;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var encoding = HttpHelper.GetEncoding(url, out var buf);

            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("gb2312")));
            var content = encoding.GetString(buf);
            Assert.IsTrue(content.Contains("陕西建工第十建设集团有限公司"));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            //EUC-KR
            var url = "http://www.bktech.co.kr/";
            var encoding = HttpHelper.GetEncoding(url, out var buf);
            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("EUC-KR")));
            var content = encoding.GetString(buf);
            Assert.IsTrue(content.Contains("(주)부광테크"));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            //BIG5 Transfer-Encoding: chunked
            var url = "http://www.wenweipo.com/";
            var encoding = HttpHelper.GetEncoding(url, out var buf);
            Assert.IsTrue(Equals(encoding, Encoding.GetEncoding("BIG5")));
            var content = encoding.GetString(buf);
            Assert.IsTrue(content.Contains("香港文匯網"));
        }
    }
}
