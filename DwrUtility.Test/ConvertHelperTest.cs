using DwrUtility.Converts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace DwrUtility.Test
{
    [TestClass]
    public class ConvertHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var path = $"{DwrUtilitySetting.Root}/a.txt";

            var v1 = IsUtf8(path);

            var v2 = IsUtf32Le(path);
            var v3 = IsUtf32Le2(path);
            var v4 = IsUtf32Be(path);

            var v5 = IsUtf16Be(path);
            var v6 = IsUtf16Le(path);

            File.Delete(path);

            Assert.IsTrue(v1 && v2 && v3 && v4 && v5 && v6);
        }

        /// <summary>
        /// Utf8
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsUtf8(string path)
        {
            File.WriteAllText(path, "text", Encoding.UTF8);
            var bt = File.ReadAllBytes(path);
            var c = bt.GetEncoding();
            return c == "utf-8";
        }

        /// <summary>
        /// UTF-32-LE
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsUtf32Le(string path)
        {
            File.WriteAllText(path, "text", new UTF32Encoding());
            var bt = File.ReadAllBytes(path);
            var c = bt.GetEncoding();
            return c == "utf-32";
        }
        /// <summary>
        /// UTF-32-LE
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsUtf32Le2(string path)
        {
            File.WriteAllText(path, "text", new UTF32Encoding(false, true));
            var bt = File.ReadAllBytes(path);
            var c = bt.GetEncoding();
            return c == "utf-32";
        }
        /// <summary>
        /// UTF-32-BE
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsUtf32Be(string path)
        {
            File.WriteAllText(path, "text", new UTF32Encoding(true, true));
            var bt = File.ReadAllBytes(path);
            var c = bt.GetEncoding();
            return c == "utf-32";
        }

        /// <summary>
        /// UTF-16-BE
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsUtf16Be(string path)
        {
            File.WriteAllText(path, "text", Encoding.BigEndianUnicode);
            var bt = File.ReadAllBytes(path);
            var c = bt.GetEncoding();
            return c == "utf-16";
        }
        /// <summary>
        /// UTF-16-LE
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsUtf16Le(string path)
        {
            File.WriteAllText(path, "text", new UnicodeEncoding(false, true));
            var bt = File.ReadAllBytes(path);
            var c = bt.GetEncoding();
            return c == "utf-16";
        }

        [TestMethod]
        public void TestMethod2()
        {
            var path = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.txt";
            File.WriteAllText(path, "测试", Encoding.UTF8);
            var md5 = path.ToFileMd5();

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            var bt = stream.ReadAsBytes(true);

            var path2 = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.txt";
            File.WriteAllBytes(path2, bt);
            var result = path2.ToFileMd5();

            File.Delete(path);
            File.Delete(path2);

            Assert.IsTrue(md5 == result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var py = "中华人民共和国".ToPingYin();
            Assert.IsTrue(py == "ZhongHuaRenMinGongHeGuo");
        }

        [TestMethod]
        public void TestMethod4()
        {
            //转二进制
            Assert.IsTrue(ConvertHelper.ToBinary(2, false) == "10");
            Assert.IsTrue(ConvertHelper.ToBinary(2, true) == "00000010");
            //转二进制
            Assert.IsTrue(ConvertHelper.ToBinary(2) == "10");
            Assert.IsTrue(ConvertHelper.ToBinary(999) == "1111100111");

            //转十六进制
            Assert.IsTrue(ConvertHelper.ToHexadecimal(255, false) == "ff");
            Assert.IsTrue(ConvertHelper.ToHexadecimal(255, true) == "FF");
            //转十六进制
            Assert.IsTrue(ConvertHelper.ToHexadecimal(255) == "ff");
            Assert.IsTrue(ConvertHelper.ToHexadecimal(999) == "3e7");

        }

        [TestMethod]
        public void TestMethod6()
        {
            var str = "我是中国人";
            var unicode = ConvertHelper.StringToUnicode(str);
            Assert.IsTrue(unicode == "\\u6211\\u662f\\u4e2d\\u56fd\\u4eba");
        }

        [TestMethod]
        public void TestMethod7()
        {
            var unicode = "\\u6211\\u662f\\u4e2d\\u56fd\\u4eba";
            var str = ConvertHelper.UnicodeToString(unicode);
            Assert.IsTrue(str == "我是中国人");
        }
    }
}
