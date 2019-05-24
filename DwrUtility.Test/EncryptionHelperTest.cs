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

            DirectoryHelper.DeleteDirectory(dir);

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
            DirectoryHelper.DeleteFile(path);
        }
    }
}
