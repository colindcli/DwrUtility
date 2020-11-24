using DwrUtility.CreanFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DwrUtility.Test
{
    [TestClass]
    public class CreanFileTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var root = DwrUtilitySetting.Root;
            var dir = $"{root}/testFolder";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var path = $"{dir}/1.txt";
            File.WriteAllText(path, "test");
            Assert.IsTrue(File.Exists(path));

            CreanFileHelper.Start(new CreanFileParam()
            {
                FileTimes = new List<FileTime>()
                {
                    new FileTime()
                    {
                        DeleteTime = TimeSpan.FromSeconds(1),
                        Directories = new List<string>() { dir },
                    }
                },
                Period = TimeSpan.FromMilliseconds(200),
                DueTime = TimeSpan.FromSeconds(0),
            });

            Assert.IsTrue(File.Exists(path));

            Thread.Sleep(3000);

            Assert.IsTrue(!File.Exists(path));
        }
    }
}
