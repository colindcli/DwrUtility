using DwrUtility.Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DwrUtility.Test
{
    [TestClass]
    public class ImageTest
    {
        private static readonly string FileDir = Path.GetFullPath($"{DwrUtilitySetting.Root}/../../../DataFiles/");

        [TestMethod]
        public void TestMethod1()
        {
            var path = $"{FileDir}xft.jpg";
            var toPath = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.jpg";
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
            var toPath = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.jpg";
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
            var toPath = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.jpg";
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
            var toPath = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.jpg";
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
            var toPath = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.jpg";
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
            var toPath = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.jpg";
            var toPath2 = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.jpg";
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

        [TestMethod]
        public void TestMethod7()
        {
            var path = $"{FileDir}base64.jpg";
            var b = ImageHelper.ImageToBase64String(path, out var str);
            Assert.IsTrue(b);

            var res = "data:image/jpeg;base64,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";
            Assert.IsTrue(str == res);

            var toPath = $"{DwrUtilitySetting.Root}/{Guid.NewGuid()}.jpg";
            var b2 = ImageHelper.Base64StringToImage(str, toPath);
            Assert.IsTrue(b2);

            var md1 = EncryptionHelper.FileMd5(path);
            var md2 = EncryptionHelper.FileMd5(toPath);
            FileHelper.DeleteFile(toPath, false);

            var b3 = md1.IsEquals(md2);
            if (!b3)
            {
                Assert.Inconclusive("两文件的md5值不一样");
            }

            Assert.IsTrue(b3);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var path = $"{FileDir}images/error.jpg";
            var img = ImageHelper.GetImageInfo(path);

            if (img.ImageFormat == ImgFormat.NotRecognised)
            {
                Assert.Inconclusive("无法识别图片");
            }
        }

        [TestMethod]
        public void TestMethod9()
        {
            var path = $"{FileDir}images/ok.png";
            var img = ImageHelper.GetImageInfo(path);

            Assert.IsTrue(img.ImageFormat == ImgFormat.Png);

            var width = 500;
            var height = 500;
            if (img.Width != width || img.Height != height)
            {
                Assert.Inconclusive($"真实为{width}*{height}；Width: {img.Width}；Height：{img.Height}");
            }
        }

        [TestMethod]
        public void TestMethod10()
        {
            var path = $"{FileDir}images/ok.jpg";
            var img = ImageHelper.GetImageInfo(path);

            Assert.IsTrue(img.ImageFormat == ImgFormat.Png);

            var width = 500;
            var height = 500;
            if (img.Width != width || img.Height != height)
            {
                Assert.Inconclusive($"真实为{width}*{height}；Width: {img.Width}；Height：{img.Height}");
            }
        }

        [TestMethod]
        public void TestMethod11()
        {
            var path = $"{FileDir}images/xftbg.png";
            var img = ImageHelper.GetImageInfo(path);

            Assert.IsTrue(img.ImageFormat == ImgFormat.Jpg);

            var width = 1920;
            var height = 1080;
            if (img.Width != width || img.Height != height)
            {
                Assert.Inconclusive($"真实为{width}*{height}；Width: {img.Width}；Height：{img.Height}");
            }
        }

        [TestMethod]
        public void TestMethod12()
        {
            var path = $"{FileDir}images/timg.gif";
            var img = ImageHelper.GetImageInfo(path);

            Assert.IsTrue(img.ImageFormat == ImgFormat.Gif);

            var width = 658;
            var height = 494;
            if (img.Width != width || img.Height != height)
            {
                Assert.Inconclusive($"真实为{width}*{height}；Width: {img.Width}；Height：{img.Height}");
            }
        }
    }
}
