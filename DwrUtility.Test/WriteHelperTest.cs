using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace DwrUtility.Test
{
    [TestClass]
    public class WriteHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var t1 = WriteLog("测试");
            var t2 = WriteLog("一一");
            var t3 = WriteLog("三亿");

            Task.WaitAll(t1, t2, t3);

            var path = $"{DwrUtilitySetting.Root}/Logs/log.txt";
            var b1 = File.Exists(path);

            Assert.IsTrue(b1);
        }

        private Task WriteLog(string name)
        {
            return Task.Run(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    WriteHelper.Log($"{name}: 测试内容");
                }
            });
        }
    }
}
