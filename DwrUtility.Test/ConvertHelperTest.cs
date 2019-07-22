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
            var path = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/')}/a.txt";

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
            var path = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/')}/{Guid.NewGuid()}.txt";
            File.WriteAllText(path, "测试", Encoding.UTF8);
            var md5 = path.ToFileMd5();

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            var bt = stream.ReadAsBytes(true);

            var path2 = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/')}/{Guid.NewGuid()}.txt";
            File.WriteAllBytes(path2, bt);
            var result = path2.ToFileMd5();

            File.Delete(path);
            File.Delete(path2);

            Assert.IsTrue(md5 == result);
        }
    }
}
