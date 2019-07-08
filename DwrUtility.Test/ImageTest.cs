using DwrUtility.Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DwrUtility.Test
{
    [TestClass]
    public class ImageTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            var path = $"{FileDir}xft.jpg";
            var toPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Guid.NewGuid()}.jpg";
            var water = $"{FileDir}logo.jpg";
            var b = ImageHelper.AddImageLogo(path, toPath, water, ImagePosition.Center, 0.4);

            Assert.IsTrue(b);

            try
            {
                File.Delete(toPath);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "删除失败");
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var path = $"{FileDir}xft.jpg";
            var toPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Guid.NewGuid()}.jpg";
            var b = ImageHelper.GenerateThumbnail(path, toPath, 200, 200, ThumbnailType.Cut);

            Assert.IsTrue(b);

            try
            {
                File.Delete(toPath);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "删除失败");
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            var path = $"{FileDir}xft.jpg";
            var toPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Guid.NewGuid()}.jpg";
            var b = ImageHelper.GenerateThumbnail(path, toPath, 200, 200, ThumbnailType.NotProportional);

            Assert.IsTrue(b);

            try
            {
                File.Delete(toPath);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "删除失败");
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            var path = $"{FileDir}xft.jpg";
            var toPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Guid.NewGuid()}.jpg";
            var b = ImageHelper.GenerateThumbnail(path, toPath, 200, 200, ThumbnailType.Zoom);

            Assert.IsTrue(b);

            try
            {
                File.Delete(toPath);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "删除失败");
            }
        }

        [TestMethod]
        public void TestMethod5()
        {
            var path = $"{FileDir}xft.jpg";
            var toPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Guid.NewGuid()}.jpg";
            var b = ImageHelper.GenerateThumbnailAndCompression(path, toPath, 200, 200, 40, ThumbnailType.Zoom);

            Assert.IsTrue(b);

            try
            {
                File.Delete(toPath);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "删除失败");
            }
        }

        [TestMethod]
        public void TestMethod6()
        {
            var path = $"{FileDir}xft.jpg";
            var toPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{Guid.NewGuid()}.jpg";
            var toPath2 = $"{AppDomain.CurrentDomain.BaseDirectory}/{Guid.NewGuid()}.jpg";
            var water = $"{FileDir}logo.jpg";
            var b1 = ImageHelper.GenerateThumbnail(path, toPath, 200, 200, ThumbnailType.Zoom);
            Assert.IsTrue(b1);

            var b2 = ImageHelper.AddImageLogo(toPath, toPath2, water, ImagePosition.Center, 0.4);
            Assert.IsTrue(b2);

            try
            {
                File.Delete(toPath);
                File.Delete(toPath2);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "删除失败");
            }
        }
    }
}
