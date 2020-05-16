using DwrUtility.Test.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DwrUtility.Test
{
    [TestClass]
    public class DirectoryHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var path = $@"E:\Demo_{Guid.NewGuid()}";

            path.CreateDir();
            Assert.IsTrue(Directory.Exists(path));

            DirectoryHelper.DeleteDirectory(path, false);
            Assert.IsTrue(!Directory.Exists(path));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var p = $@"E:\Demo_{Guid.NewGuid()}";
            var path = $@"{p}\11\22\22";

            path.CreateDir();
            Assert.IsTrue(Directory.Exists(path));

            DirectoryHelper.DeleteDirectory(p, false);
            Assert.IsTrue(!Directory.Exists(p));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var t = $@"E:\Demo_{Guid.NewGuid()}";
            var p = $@"{t}\11\22";
            var path = $@"{p}\22.txt";

            path.CreateDirByFilePath();
            Assert.IsTrue(Directory.Exists(p));

            File.WriteAllText(path, "test");
            Assert.IsTrue(File.Exists(path));

            DirectoryHelper.DeleteDirectory(t, false);
            Assert.IsTrue(!Directory.Exists(t));
        }

        [TestMethod]
        public void TestMethod4()
        {
            var dir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../Html/");
            DirectoryHelper.GetDirectoryFiles(dir, out var dirs, out var files);

            Assert.IsTrue(dirs.Count == 3 && files.Count == 4);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var path = $@"E:\Demo_{Guid.NewGuid()}";

            path.CreateDir();
            Assert.IsTrue(Directory.Exists(path));

            DirectoryHelper.DeleteDirectory(path, true);
            Assert.IsTrue(!Directory.Exists(path));
        }

        [TestMethod]
        public void TestMethod6()
        {
            var p = $@"E:\Demo_{Guid.NewGuid()}";
            var path = $@"{p}\11\22\22";

            path.CreateDir();
            Assert.IsTrue(Directory.Exists(path));

            DirectoryHelper.DeleteDirectory(p, true);
            Assert.IsTrue(!Directory.Exists(p));
        }

        [TestMethod]
        public void TestMethod7()
        {
            var t = $@"E:\Demo_{Guid.NewGuid()}";
            var p = $@"{t}\11\22";
            var path = $@"{p}\22.txt";

            path.CreateDirByFilePath();
            Assert.IsTrue(Directory.Exists(p));

            File.WriteAllText(path, "test");
            Assert.IsTrue(File.Exists(path));

            DirectoryHelper.DeleteDirectory(t, true);
            Assert.IsTrue(!Directory.Exists(t));
        }

        /// <summary>
        /// 获取不存在的文件夹报错
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            var dir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimSlash()}/{Guid.NewGuid()}";
            DirectoryHelper.GetDirectoryFiles(dir, out var dirs, out var files);
            Assert.IsTrue(dirs.Count == 0 && files.Count == 0);

            var ds1 = DirectoryHelper.GetDirectorys(dir);
            var ds2 = DirectoryHelper.GetDirectorys(null);
            Assert.IsTrue(ds1.Count == 0 && ds2.Count == 0);
        }

        /// <summary>
        /// 没有system.webServer
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            var dir = $"{TestConfig.BinDir}Files/config/";
            dir.CreateDir();

            DirectoryHelper.AddDirectoryReadAuth(dir);

            var path = $"{dir}web.config";
            Assert.IsTrue(File.Exists(path));

            //删除文件
            FileHelper.DeleteFile(path, false);
        }
    }
}
