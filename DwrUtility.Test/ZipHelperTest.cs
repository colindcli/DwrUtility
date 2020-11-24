using DwrUtility.Test.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DwrUtility.Test
{
    [TestClass]
    public class ZipHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var id = Guid.Empty;
            var dir = $"{TestConfig.TestDir}Files/{id}/Temp/";
            dir.CreateDir();

            var path1 = $"{dir}文件1.txt";
            File.WriteAllText(path1, "文件1", Encoding.UTF8);
            var path2 = $"{dir}文件2.txt";
            File.WriteAllText(path2, "文件2", Encoding.UTF8);

            //压缩文件夹
            var zipPath = $"{TestConfig.TestDir}Files/{id}/1.zip";
            DirectoryHelper.DeleteFile(zipPath, false);
            var flag = ZipHelper.Eecompression(dir, zipPath, out var msg);
            Assert.IsTrue(flag, msg);

            //解压缩文件
            var f1 = $"{TestConfig.TestDir}Files/{id}/文件1.txt";
            var f2 = $"{TestConfig.TestDir}Files/{id}/文件2.txt";
            DirectoryHelper.DeleteFiles(f1, f2);

            var flag2 = ZipHelper.Decompression(zipPath, $"{TestConfig.TestDir}Files/{id}", out msg);
            Assert.IsTrue(flag2, msg);

            var b1 = File.Exists(f1) && File.Exists(f2);
            Assert.IsTrue(b1);

            //验证文件内容
            var c1 = File.ReadAllText(f1);
            var c2 = File.ReadAllText(f2);
            var b2 = c1.IsEquals("文件1") && c2.IsEquals("文件2");
            Assert.IsTrue(b2);

            DirectoryHelper.DeleteDirectory($"{TestConfig.TestDir}Files/{id}", false);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var id = Guid.Empty;
            var dir = $"{TestConfig.TestDir}Files/{id}/Temp/";
            dir.CreateDir();

            var path1 = $"{dir}文件1.txt";
            File.WriteAllText(path1, "文件1", Encoding.UTF8);
            var path2 = $"{dir}文件2.txt";
            File.WriteAllText(path2, "文件2", Encoding.UTF8);

            //压缩文件夹
            var zipPath = $"{TestConfig.TestDir}Files/{id}/1.zip";
            DirectoryHelper.DeleteFile(zipPath, false);
            var flag = ZipHelper.Eecompression(new List<string>()
            {
                path1,
                path2
            }, zipPath, out var msg);
            Assert.IsTrue(flag, msg);

            //解压缩文件
            var f1 = $"{TestConfig.TestDir}Files/{id}/文件1.txt";
            var f2 = $"{TestConfig.TestDir}Files/{id}/文件2.txt";
            DirectoryHelper.DeleteFiles(f1, f2);

            var flag2 = ZipHelper.Decompression(zipPath, $"{TestConfig.TestDir}Files/{id}", out msg);
            Assert.IsTrue(flag2, msg);

            var b1 = File.Exists(f1) && File.Exists(f2);
            Assert.IsTrue(b1);

            //验证文件内容
            var c1 = File.ReadAllText(f1);
            var c2 = File.ReadAllText(f2);
            var b2 = c1.IsEquals("文件1") && c2.IsEquals("文件2");
            Assert.IsTrue(b2);

            DirectoryHelper.DeleteDirectory($"{TestConfig.TestDir}Files/{id}", false);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var id = Guid.Empty;
            var dir = $"{TestConfig.TestDir}Files/{id}/Temp/";
            dir.CreateDir();

            var path1 = $"{dir}文件1.txt";
            File.WriteAllText(path1, "文件1", Encoding.UTF8);
            var path2 = $"{dir}文件2.txt";
            File.WriteAllText(path2, "文件2", Encoding.UTF8);

            //压缩文件夹
            var zipPath = $"{TestConfig.TestDir}Files/{id}/1.zip";
            DirectoryHelper.DeleteFile(zipPath, false);
            var flag = ZipHelper.Zip(dir, zipPath, out var msg);
            Assert.IsTrue(flag, msg);

            //解压缩文件
            var f1 = $"{TestConfig.TestDir}Files/{id}/文件1.txt";
            var f2 = $"{TestConfig.TestDir}Files/{id}/文件2.txt";
            DirectoryHelper.DeleteFiles(f1, f2);

            var flag2 = ZipHelper.UnZip(zipPath, $"{TestConfig.TestDir}Files/{id}", out msg);
            Assert.IsTrue(flag2, msg);

            var b1 = File.Exists(f1) && File.Exists(f2);
            Assert.IsTrue(b1);

            //验证文件内容
            var c1 = File.ReadAllText(f1);
            var c2 = File.ReadAllText(f2);
            var b2 = c1.IsEquals("文件1") && c2.IsEquals("文件2");
            Assert.IsTrue(b2);

            DirectoryHelper.DeleteDirectory($"{TestConfig.TestDir}Files/{id}", false);
        }
    }
}
