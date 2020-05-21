using DwrUtility.Images;
using DwrUtility.Test.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace DwrUtility.Test
{
    [TestClass]
    public class ImageTypeTest
    {
        private static readonly string Dir = Path.GetFullPath($"{TestConfig.TestDir}DataFiles/fixtures/");

        private static void Test(List<string> error, string pathName, int width, int height, ImgFormat format)
        {
            var img = ImageHelper.GetImageInfo($"{Dir}{pathName}");
            var b = img.Width == 0 && img.Height == 0
                ? img.ImageFormat == format
                : img.ImageFormat == format && img.Width == width && img.Height == height;
            if (!b)
            {
                error.Add($"{width}*{height};{pathName}");
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var error = new List<string>();
            Test(error, "test.bmp", 40, 27, ImgFormat.Bmp);
            Test(error, "test2.bmp", 1920, 1080, ImgFormat.Bmp);
            Test(error, "test_coreheader.bmp", 40, 27, ImgFormat.Bmp);
            Test(error, "test_v5header.bmp", 40, 27, ImgFormat.Bmp);
            Test(error, "test.gif", 17, 32, ImgFormat.Gif);
            Test(error, "test.jpg", 882, 470, ImgFormat.Jpg);
            Test(error, "test.png", 30, 20, ImgFormat.Png);
            Test(error, "test-24.png", 192, 108, ImgFormat.Png);
            Test(error, "test2.png", 52, 34, ImgFormat.Png);
            Test(error, "test2.jpg", 250, 188, ImgFormat.Jpg);
            Test(error, "test3.jpg", 630, 367, ImgFormat.Jpg);
            Test(error, "test4.jpg", 1485, 1299, ImgFormat.Jpg);
            Test(error, "test.tiff", 85, 67, ImgFormat.Tiff);
            Test(error, "test2.tiff", 333, 225, ImgFormat.Tiff);
            Test(error, "test.psd", 17, 32, ImgFormat.Psd);
            Test(error, "test2.pdf", 52, 34, ImgFormat.Pdf);
            Test(error, "exif_orientation.jpg", 600, 450, ImgFormat.Jpg);
            Test(error, "infinite.jpg", 160, 240, ImgFormat.Jpg);
            Test(error, "orient_2.jpg", 230, 408, ImgFormat.Jpg);
            Test(error, "favicon.ico", 16, 16, ImgFormat.Ico);
            Test(error, "favicon2.ico", 32, 32, ImgFormat.Ico);
            Test(error, "man.ico", 256, 256, ImgFormat.Ico);
            Test(error, "test.cur", 32, 32, ImgFormat.Cur);
            Test(error, "webp_vp8x.webp", 386, 395, ImgFormat.Webp);
            Test(error, "webp_vp8l.webp", 386, 395, ImgFormat.Webp);
            Test(error, "webp_vp8.webp", 550, 368, ImgFormat.Webp);
            Test(error, "test.svg", 200, 300, ImgFormat.Svg);
            Test(error, "test_partial_viewport.svg", 860, 400, ImgFormat.Svg);
            Test(error, "test2.svg", 366, 271, ImgFormat.Svg);
            Test(error, "test3.svg", 255, 48, ImgFormat.Svg);
            Test(error, "test4.svg", 271, 271, ImgFormat.Svg);
            Test(error, "orient_6.jpg", 1250, 2500, ImgFormat.Jpg);
            Test(error, "error.webp", 1500, 1500, ImgFormat.Webp);

            if (error.Count > 0)
            {
                Assert.IsTrue(false, string.Join("、", error));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="pathName"></param>
        /// <param name="extName"></param>
        private void CheckType(List<string> error, string pathName, string extName)
        {
            var path = $"{Dir}{pathName}";
            var b = ImageHelper.IsPicture(path, out var ext) && ext == extName;
            if (!b)
            {
                error.Add($"{pathName};{ext}");
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var error = new List<string>();
            CheckType(error, "test.bmp", ".bmp");
            CheckType(error, "test.gif", ".gif");
            CheckType(error, "test.jpg", ".jpg");
            CheckType(error, "test.png", ".png");
            CheckType(error, "test.tiff", ".tiff");
            CheckType(error, "webp_vp8x.webp", ".webp");
            CheckType(error, "test.svg", ".svg");
            CheckType(error, "test.psd", ".psd");
            CheckType(error, "test2.pdf", ".pdf");
            CheckType(error, "man.ico", ".ico");
            CheckType(error, "test.cur", ".cur");

            if (error.Count > 0)
            {
                Assert.IsTrue(false, string.Join("、", error));
            }
        }
    }
}
