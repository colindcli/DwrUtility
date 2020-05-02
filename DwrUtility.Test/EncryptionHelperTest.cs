using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DwrUtility.Test
{
    [TestClass]
    public class EncryptionHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var md5 = "123".ToMd5();
            Assert.IsTrue(md5 == "202cb962ac59075b964b07152d234b70");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var md5 = "".ToMd5();
            Assert.IsTrue(md5 == "d41d8cd98f00b204e9800998ecf8427e");
        }

        [TestMethod]
        public void TestMethod3()
        {
            var md5 = EncryptionHelper.Md5(null);
            Assert.IsTrue(md5 == null);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var e = EncryptionHelper.EncryptDes("123", "12345678");
            var d = EncryptionHelper.DecryptDes(e, "12345678");
            Assert.IsTrue(d == "123");
        }

        [TestMethod]
        public void TestMethod5()
        {
            var dir = $@"D:\File_{Guid.NewGuid()}";
            var path = $@"{dir}\1.txt";
            path.CreateDirByFilePath();
            File.WriteAllText(path, "123", Encoding.UTF8);

            var md5 = path.ToFileMd5();

            DirectoryHelper.DeleteDirectory(dir, false);

            Assert.IsTrue(!Directory.Exists(dir));

            Assert.IsTrue(md5 == "a6afd7bbb8d59e5bb12c9dbcc4ec2cff");
        }

        [TestMethod]
        public void TestMethod6()
        {
            var path = $@"D:\demo_{Guid.NewGuid()}";
            File.WriteAllText(path, "123");
            Parallel.For(0, 1000, p =>
            {
                var md5 = path.ToFileMd5();
                if (md5 == null)
                {
                    Assert.IsTrue(false, "并行读文件冲突");
                }
            });
            DirectoryHelper.DeleteFile(path, false);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var str = "中文en中国語중국어จีนこんにちは語言";
            var en = EncryptionHelper.EncryptDes(str, "12345678");
            var res = EncryptionHelper.DecryptDes(en, "12345678");

            Assert.IsTrue(res == str);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var str = "中文en中国語중국어จีนこんにちは語言";
            var enStr = EncryptionHelper.Sha1(str);
            Assert.IsTrue(enStr == "15439f0652d173ce9710eb0f526af83ea31e7931");
        }

        [TestMethod]
        public void TestMethod9()
        {
            var str = "中文en中国語중국어จีนこんにちは語言";
            var enStr = EncryptionHelper.Sha256(str);
            Assert.IsTrue(enStr == "130e55df557abf6781dccc482586b774556763830b6d4abfda22e00dabe42a32");
        }

        [TestMethod]
        public void TestMethod10()
        {
            var str = "中文en中国語중국어จีนこんにちは語言";
            var enStr = EncryptionHelper.Sha384(str);
            Assert.IsTrue(enStr == "54a29e0e136a6195f2313f0c08c121c8dc57ada7e71b4bd45b3a981f2327b60f3c7be53925846fdebe8693c49eb12881");
        }

        [TestMethod]
        public void TestMethod11()
        {
            var str = "中文en中国語중국어จีนこんにちは語言";
            var enStr = EncryptionHelper.Sha512(str);
            Assert.IsTrue(enStr == "0ec324ece017ede3e713517de8257119dbd2fe1d4efcdc8e76ce7753c0e90a85fd3c9448e134240c5a2af9bf97aaa48e3aebf0542f9cbe2b324814fb3648091b");
        }

        [TestMethod]
        public void TestMethod12()
        {
            var str = "中文en中国語중국어จีนこんにちは語言";
            var enStr = EncryptionHelper.EncryptBase64(str);
            Assert.IsTrue(enStr == "5Lit5paHZW7kuK3lm73oqp7spJHqta3slrTguIjguLXguJnjgZPjgpPjgavjgaHjga/oqp7oqIA=");
        }

        [TestMethod]
        public void TestMethod13()
        {
            var str = "5Lit5paHZW7kuK3lm73oqp7spJHqta3slrTguIjguLXguJnjgZPjgpPjgavjgaHjga/oqp7oqIA=";
            var enStr = EncryptionHelper.DecryptBase64(str);
            Assert.IsTrue(enStr == "中文en中国語중국어จีนこんにちは語言");
        }
    }
}
