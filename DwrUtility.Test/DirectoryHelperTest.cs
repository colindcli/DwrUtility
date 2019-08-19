using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

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

            DirectoryHelper.DeleteDirectory(path);
            Assert.IsTrue(!Directory.Exists(path));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var p = $@"E:\Demo_{Guid.NewGuid()}";
            var path = $@"{p}\11\22\22";

            path.CreateDir();
            Assert.IsTrue(Directory.Exists(path));

            DirectoryHelper.DeleteDirectory(p);
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

            DirectoryHelper.DeleteDirectory(t);
            Assert.IsTrue(!Directory.Exists(t));
        }

        [TestMethod]
        public void TestMethod4()
        {
            var dir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../Html/");
            DirectoryHelper.GetDirectoryFiles(dir, out var dirs, out var files);

            Assert.IsTrue(dirs.Count == 3 && files.Count == 4);
        }
    }
}
