using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace DwrUtility.Images
{
    /// <summary>
    /// 图片处理类
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 生成缩略图 (按比例缩小图片)
        /// </summary>
        /// <param name="sourcePath">源图片</param>
        /// <param name="outPath">缩略图</param>
        /// <param name="intWidth">图片宽带</param>
        /// <param name="intHeight">图片高度</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool GenerateThumbnail(string sourcePath, string outPath, int intWidth, int intHeight, ThumbnailType type)
        {
            if (type == ThumbnailType.Zoom)
            {
                return GenerateThumbnail1(sourcePath, outPath, intWidth, intHeight);
            }

            if (type == ThumbnailType.NotProportional)
            {
                return GenerateThumbnail2(sourcePath, outPath, intWidth, intHeight);
            }

            if (type == ThumbnailType.Cut)
            {
                return GenerateThumbnail3(sourcePath, outPath, intWidth, intHeight);
            }

            return false;
        }

        /// <summary>
        /// 生成缩略图 (按比例缩小图片)
        /// </summary>
        /// <param name="sourcePath">源图片</param>
        /// <param name="outPath">缩略图</param>
        /// <param name="intWidth">图片宽带</param>
        /// <param name="intHeight">图片高度</param>
        /// <returns></returns>
        private static bool GenerateThumbnail1(string sourcePath, string outPath, int intWidth, int intHeight)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(outPath))
            {
                return false;
            }

            Bitmap objPic = null;
            Bitmap objNewPic = null;
            try
            {
                objPic = new Bitmap(sourcePath);
                int width;
                int height;
                if ((objPic.Width * 1.0000) / objPic.Height > intWidth * 1.0000 / intHeight)
                {
                    width = intWidth;
                    height = intWidth * objPic.Height / objPic.Width;
                }
                else
                {
                    height = intHeight;
                    width = intHeight * objPic.Width / objPic.Height;
                }

                objNewPic = new Bitmap(objPic, width, height);
                objNewPic.Save(outPath);

                objPic.Dispose();
                objNewPic.Dispose();

                return File.Exists(outPath);
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                objPic?.Dispose();
                objNewPic?.Dispose();

                return false;
            }
        }

        /// <summary>
        /// 生成缩略图  (不按比例缩小到指定宽高)
        /// </summary>
        /// <param name="sourcePath">源图片</param>
        /// <param name="outPath">输出图片</param>
        /// <param name="width">缩略图宽</param>
        /// <param name="height">缩略图高</param>
        /// <returns></returns>
        private static bool GenerateThumbnail2(string sourcePath, string outPath, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(outPath))
            {
                return false;
            }

            Image imgSource = null;
            Bitmap bmp = null;
            Graphics gr = null;

            try
            {
                imgSource = Image.FromFile(sourcePath);
                bmp = new Bitmap(width, height);
                //从Bitmap创建一个Graphics
                gr = Graphics.FromImage(bmp);
                //设置 
                gr.SmoothingMode = SmoothingMode.HighQuality;
                //下面这个也设成高质量
                gr.CompositingQuality = CompositingQuality.HighQuality;
                //下面这个设成High
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //把原始图像绘制成上面所设置宽高的缩小图
                var rectDestination = new Rectangle(0, 0, width, height);

                gr.DrawImage(imgSource, rectDestination, 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
                bmp.Save(outPath, ImageFormat.Jpeg);

                imgSource.Dispose();
                bmp.Dispose();
                gr.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                imgSource?.Dispose();
                bmp?.Dispose();
                gr?.Dispose();

                return false;
            }
        }

        /// <summary>
        /// 生成缩略图  (从原图中间切割一张指定宽高的图片)
        /// </summary>
        /// <param name="sourcePath">源图片</param>
        /// <param name="outPath">输出图片</param>
        /// <param name="width">缩略图宽</param>
        /// <param name="height">缩略图高</param>
        /// <returns></returns>
        private static bool GenerateThumbnail3(string sourcePath, string outPath, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(outPath))
            {
                return false;
            }

            Image imgSource = null;
            Bitmap bmp = null;
            Graphics gr = null;

            try
            {
                imgSource = Image.FromFile(sourcePath);
                bmp = new Bitmap(width, height);
                //从Bitmap创建一个Graphics
                gr = Graphics.FromImage(bmp);
                //设置 
                gr.SmoothingMode = SmoothingMode.HighQuality;
                //下面这个也设成高质量
                gr.CompositingQuality = CompositingQuality.HighQuality;
                //下面这个设成High
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //把原始图像绘制成上面所设置宽高的缩小图
                var rectDestination = new Rectangle(0, 0, width, height);

                var left = width > imgSource.Width ? 0 : (imgSource.Width - width) / 2;
                var right = height > imgSource.Height ? 0 : (imgSource.Height - height) / 2;

                gr.DrawImage(imgSource, rectDestination, left, right, width, height, GraphicsUnit.Pixel);
                bmp.Save(outPath, ImageFormat.Jpeg);

                imgSource.Dispose();
                bmp.Dispose();
                gr.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                imgSource?.Dispose();
                bmp?.Dispose();
                gr?.Dispose();

                return false;
            }
        }

        /// <summary>
        /// 生成缩略图并压缩图片 (按比例缩小图片)
        /// </summary>
        /// <param name="sourcePath">源图片</param>
        /// <param name="outPath">输出图片</param>
        /// <param name="intWidth">图片宽带</param>
        /// <param name="intHeight">图片高度</param>
        /// <param name="quality">图片质量1-100</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool GenerateThumbnailAndCompression(string sourcePath, string outPath, int intWidth, int intHeight, int quality, ThumbnailType type)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(outPath))
            {
                return false;
            }

            var tempPath = $"{AppDomain.CurrentDomain.BaseDirectory}Files/Temp/{Guid.NewGuid()}";
            tempPath.CreateDirByFilePath();

            //缩略图片
            var isExists = GenerateThumbnail(sourcePath, tempPath, intWidth, intHeight, type);

            //压缩图片
            var b = CompressionImage(isExists ? tempPath : sourcePath, outPath, quality);

            if (isExists)
            {
                File.Delete(tempPath);
            }

            return b;
        }

        /// <summary> 
        /// 压缩图片质量
        /// </summary> 
        /// <param name="sourcePath">源图片</param> 
        /// <param name="outPath">压缩后图片</param> 
        /// <param name="quality">图片质量：1-100</param> 
        /// <returns></returns> 
        public static bool CompressionImage(string sourcePath, string outPath, int quality)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(outPath))
            {
                return false;
            }

            var iSource = Image.FromFile(sourcePath);
            var tFormat = iSource.RawFormat;
            var ep = new EncoderParameters();
            var qy = new long[1];
            qy[0] = quality;
            var eParam = new EncoderParameter(Encoder.Quality, qy);
            ep.Param[0] = eParam;
            bool flag;
            try
            {
                var arrayIci = ImageCodecInfo.GetImageEncoders();
                var jpegIcIinfo = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));
                if (jpegIcIinfo != null)
                {
                    iSource.Save(outPath, jpegIcIinfo, ep);
                }
                else
                {
                    iSource.Save(outPath, tFormat);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                flag = false;
            }
            finally
            {
                iSource.Dispose();
                iSource.Dispose();
            }
            return flag && File.Exists(outPath);
        }

        /// <summary>
        /// 根据文字生成图片
        /// </summary>
        /// <param name="text">文字内容</param>
        /// <param name="font">字体</param>
        /// <param name="fontColor">字体颜色</param>
        /// <param name="bgColor">背景颜色,如Brushes.Transparent(透明色)</param>
        /// <param name="outPath">输出路径</param>
        /// <returns></returns>
        public static bool GenerateTextImage(string text, Font font, Brush fontColor, Color bgColor, string outPath)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(outPath))
            {
                return false;
            }

            Graphics g = null;
            Bitmap bmp = null;
            try
            {
                var format = new StringFormat(StringFormatFlags.NoClip)
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                bmp = new Bitmap(1, 1);
                g = Graphics.FromImage(bmp);
                var sizef = g.MeasureString(text, font, PointF.Empty, format);

                var width = Convert.ToInt32(sizef.Width) + 20;
                var rect = new RectangleF(0, 0, width, width);
                bmp.Dispose();
                bmp = new Bitmap(width, width);

                g = Graphics.FromImage(bmp);
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                var bgBrush = new SolidBrush(bgColor);
                g.FillRectangle(bgBrush, rect);
                g.DrawString(text, font, fontColor, rect, format);

                outPath.CreateDirByFilePath();
                bmp.Save(outPath, ImageFormat.Jpeg);

                bmp.Dispose();
                g.Dispose();
                bgBrush.Dispose();

                return File.Exists(outPath);
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                bmp?.Dispose();
                g?.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 是否图片 (.jpg;.png;.gif;.jpeg;.bmp)
        /// </summary>
        /// <param name="fileTitle"></param>
        /// <returns></returns>
        public static bool IsImage(string fileTitle)
        {
            var array = new[] { ".jpg", ".png", ".gif", ".jpeg", ".bmp", ".svg" };
            var fileType = Path.GetExtension(fileTitle);
            return !string.IsNullOrWhiteSpace(fileType) && array.Contains(fileType, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Base64编码的字符串转为图片
        /// </summary>
        /// <param name="base64Str">base64字符串</param>
        /// <param name="toPath">输出路径</param>
        /// <returns></returns>
        public static bool Base64StringToImage(string base64Str, string toPath)
        {
            MemoryStream ms = null;
            Bitmap bmp = null;
            try
            {
                var array = base64Str.Split(',');
                var str = array.Length > 1 ? array[1].Trim() : array[0].Trim();

                var bt = Convert.FromBase64String(str);
                ms = new MemoryStream(bt);
                bmp = new Bitmap(ms);

                bmp.Save(toPath, ImageFormat.Jpeg);
                bmp.Dispose();
                ms.Close();
                ms.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                bmp?.Dispose();
                ms?.Close();
                ms?.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 图片转成base64编码
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="base64String"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        public static bool ImageToBase64String(string imagePath, out string base64String, ImageFormat imageFormat)
        {
            Bitmap bmp = null;
            MemoryStream ms = null;
            try
            {
                bmp = new Bitmap(imagePath);
                ms = new MemoryStream();
                bmp.Save(ms, imageFormat);
                bmp.Dispose();
                var contentType = "image/jpeg";
                if (Equals(imageFormat, ImageFormat.Jpeg))
                {
                    contentType = "image/jpeg";
                }
                else if (Equals(imageFormat, ImageFormat.Bmp))
                {
                    contentType = "image/bmp";
                }
                else if (Equals(imageFormat, ImageFormat.Png))
                {
                    contentType = "image/png";
                }
                else if (Equals(imageFormat, ImageFormat.Gif))
                {
                    contentType = "image/gif";
                }
                else if (Equals(imageFormat, ImageFormat.Icon))
                {
                    contentType = "image/x-icon";
                }
                else if (Equals(imageFormat, ImageFormat.Tiff))
                {
                    contentType = "image/tiff";
                }

                base64String = $"data:{contentType};base64,{Convert.ToBase64String(ms.ToArray())}";
                ms.Close();
                ms.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                bmp?.Dispose();
                ms?.Close();
                ms?.Dispose();
                base64String = null;
                return false;
            }

        }

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="sourceFilename">源图片路径</param>
        /// <param name="outFilename">目标图片路径</param>
        /// <param name="waterFilename">水印图片路径</param>
        /// <param name="position">水印位置</param>
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <returns></returns>
        public static bool AddImageLogo(string sourceFilename, string outFilename, string waterFilename, ImagePosition position, double alpha)
        {
            if (alpha < 0 || alpha > 1 || string.IsNullOrWhiteSpace(sourceFilename) || string.IsNullOrWhiteSpace(outFilename) || string.IsNullOrWhiteSpace(waterFilename))
            {
                return false;
            }

            Image imgPhoto = null;
            Image newImgPhoto = null;
            try
            {
                imgPhoto = Image.FromFile(sourceFilename);
                newImgPhoto = AddImageLogoDeal(waterFilename, position, alpha, imgPhoto);
                newImgPhoto.Save(outFilename, ImageFormat.Jpeg);
                imgPhoto.Dispose();
                newImgPhoto.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                imgPhoto?.Dispose();
                newImgPhoto?.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="memoryStream">图片流</param>
        /// <param name="waterFilename">水印图片路径</param>
        /// <param name="position">水印位置</param>
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <returns></returns>
        public static bool AddImageLogo(ref MemoryStream memoryStream, string waterFilename, ImagePosition position, double alpha)
        {
            if (alpha < 0 || alpha > 1)
            {
                return false;
            }

            Image imgPhoto = null;
            Image newImgPhoto = null;
            try
            {
                imgPhoto = Image.FromStream(memoryStream);
                newImgPhoto = AddImageLogoDeal(waterFilename, position, alpha, imgPhoto);

                memoryStream = new MemoryStream();
                newImgPhoto.Save(memoryStream, ImageFormat.Jpeg);

                imgPhoto.Dispose();
                newImgPhoto.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                imgPhoto?.Dispose();
                newImgPhoto?.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 在图片上添加水印文字
        /// </summary>
        /// <param name="sourceFilename">源图片路径</param>
        /// <param name="outFilename">目标图片路径</param>
        /// <param name="waterText">水印文字</param>
        /// <param name="position">水印位置</param>
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <returns></returns>
        public static bool AddImageText(string sourceFilename, string outFilename, string waterText, ImagePosition position, double alpha)
        {
            if (alpha < 0 || alpha > 1 || string.IsNullOrWhiteSpace(sourceFilename) || string.IsNullOrWhiteSpace(outFilename))
            {
                return false;
            }

            Image sourceImg = null;
            Image imgPhoto = null;
            try
            {
                sourceImg = Image.FromFile(sourceFilename);
                imgPhoto = AddImageTextDeal(waterText, position, alpha, sourceImg);
                imgPhoto.Save(outFilename, ImageFormat.Jpeg);
                sourceImg.Dispose();
                imgPhoto.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                sourceImg?.Dispose();
                imgPhoto?.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 在图片上添加水印文字
        /// </summary>
        /// <param name="memoryStream">图片流</param>
        /// <param name="waterText">水印文字</param>
        /// <param name="position">水印位置</param>
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <returns></returns>
        public static bool AddImageText(ref MemoryStream memoryStream, string waterText, ImagePosition position, double alpha)
        {
            if (alpha < 0 || alpha > 1 || string.IsNullOrWhiteSpace(waterText))
            {
                return false;
            }

            Image sourceImg = null;
            Image imgPhoto = null;
            try
            {
                sourceImg = Image.FromStream(memoryStream);
                imgPhoto = AddImageTextDeal(waterText, position, alpha, sourceImg);
                memoryStream = new MemoryStream();
                imgPhoto.Save(memoryStream, ImageFormat.Jpeg);
                sourceImg.Dispose();
                imgPhoto.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                sourceImg?.Dispose();
                imgPhoto?.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 处理添加图片
        /// </summary>
        /// <param name="waterPath"></param>
        /// <param name="position"></param>
        /// <param name="alpha"></param>
        /// <param name="imgPhoto"></param>
        /// <returns></returns>
        private static Image AddImageLogoDeal(string waterPath, ImagePosition position, double alpha, Image imgPhoto)
        {
            var phWidth = imgPhoto.Width;
            var phHeight = imgPhoto.Height;

            var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // 设定分辨率
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // 定义一个绘图画面用来装载位图
            var grPhoto = Graphics.FromImage(bmPhoto);

            //同样，由于水印是图片，我们也需要定义一个Image来装载它
            var imgWatermark = new Bitmap(waterPath);

            // 获取水印图片的高度和宽度
            var wmWidth = imgWatermark.Width;
            var wmHeight = imgWatermark.Height;

            // SmoothingMode    指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。
            // 成员名称         说明
            // AntiAlias        指定消除锯齿的呈现。
            // Default          指定不消除锯齿。
            // HighQuality      指定高质量、低速度呈现。
            // HighSpeed        指定高速度、低质量呈现。
            // Invalid          指定一个无效模式。
            // None             指定不消除锯齿。
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;

            // 第一次描绘，将我们的底图描绘在绘图画面上
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, phWidth, phHeight), 0, 0, phWidth, phHeight, GraphicsUnit.Pixel);

            // 与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率
            var bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // 继续，将水印图片装载到一个绘图画面grWatermark
            var grWatermark = Graphics.FromImage(bmWatermark);

            // ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。
            var imageAttributes = new ImageAttributes();

            // Colormap: 定义转换颜色的映射
            var colorMap = new ColorMap
            {

                // 我的水印图被定义成拥有绿色背景色的图片被替换成透明
                OldColor = Color.FromArgb(255, 0, 255, 0),
                NewColor = Color.FromArgb(0, 0, 0, 0)
            };

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
                  new[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},          //red红色
                  new[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},          //green绿色
                  new[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},          //blue蓝色
                  new[] {0.0f,  0.0f,  0.0f,  (float)alpha, 0.0f},  //透明度
                  new[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
            };

            // ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。
            // ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。
            var wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            //上面设置完颜色，下面开始设置位置
            int xPosOfWm;
            int yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    {
                        xPosOfWm = (phWidth - wmWidth) / 2;
                        yPosOfWm = phHeight - wmHeight - 10;
                        break;
                    }
                case ImagePosition.Center:
                    {
                        xPosOfWm = (phWidth - wmWidth) / 2;
                        yPosOfWm = (phHeight - wmHeight) / 2;
                        break;
                    }
                case ImagePosition.BottomLeft:
                    {
                        xPosOfWm = 10;
                        yPosOfWm = phHeight - wmHeight - 10;
                        break;
                    }
                case ImagePosition.TopLeft:
                    {
                        xPosOfWm = 10;
                        yPosOfWm = 10;
                        break;
                    }
                case ImagePosition.TopRight:
                    {
                        xPosOfWm = phWidth - wmWidth - 10;
                        yPosOfWm = 10;
                        break;
                    }
                case ImagePosition.BottomRigth:
                    {
                        xPosOfWm = phWidth - wmWidth - 10;
                        yPosOfWm = phHeight - wmHeight - 10;
                        break;
                    }
                case ImagePosition.TopMiddle:
                    {
                        xPosOfWm = (phWidth - wmWidth) / 2;
                        yPosOfWm = 10;
                        break;
                    }
                default:
                    {
                        xPosOfWm = 10;
                        yPosOfWm = phHeight - wmHeight - 10;
                        break;
                    }
            }

            // 第二次绘图，把水印印上去
            grWatermark.DrawImage(imgWatermark, new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight, GraphicsUnit.Pixel, imageAttributes);

            grPhoto.Dispose();
            grWatermark.Dispose();
            imgWatermark.Dispose();
            bmPhoto.Dispose();

            return bmWatermark;
        }

        /// <summary>
        /// 处理添加文字
        /// </summary>
        /// <param name="waterText"></param>
        /// <param name="position"></param>
        /// <param name="alpha"></param>
        /// <param name="imgPhoto"></param>
        /// <returns></returns>
        private static Image AddImageTextDeal(string waterText, ImagePosition position, double alpha, Image imgPhoto)
        {
            //获取图片的宽和高
            var phWidth = imgPhoto.Width;
            var phHeight = imgPhoto.Height;

            //建立一个bitmap，和我们需要加水印的图片一样大小
            var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //SetResolution：设置此 Bitmap 的分辨率
            //这里直接将我们需要添加水印的图片的分辨率赋给了bitmap
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //Graphics：封装一个 GDI+ 绘图图面。
            var grPhoto = Graphics.FromImage(bmPhoto);

            //设置图形的品质
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;

            //将我们要添加水印的图片按照原始大小描绘（复制）到图形中
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, phWidth, phHeight), 0, 0, phWidth, phHeight, GraphicsUnit.Pixel);

            //根据图片的大小我们来确定添加上去的文字的大小
            //在这里我们定义一个数组来确定
            var sizes = new[] { 16, 14, 12, 10, 8, 6, 4 };

            //字体
            Font crFont = null;

            //矩形的宽度和高度，SizeF有三个属性，分别为Height高，width宽，IsEmpty是否为空
            var crSize = new SizeF();

            //利用一个循环语句来选择我们要添加文字的型号
            //直到它的长度比图片的宽度小
            for (var i = 0; i < 7; i++)
            {
                crFont = new Font("Verdana, Tahoma, Arial, 微软雅黑, 宋体", sizes[i], FontStyle.Bold);

                //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。
                crSize = grPhoto.MeasureString(waterText, crFont);

                // ushort 关键字表示一种整数数据类型
                if ((ushort)crSize.Width < (ushort)phWidth)
                {
                    break;
                }
            }

            //定义在图片上文字的位置
            var wmHeight = crSize.Height;
            var wmWidth = crSize.Width;

            float xPosOfWm;
            float yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    {
                        xPosOfWm = (float)phWidth / 2;
                        yPosOfWm = phHeight - wmHeight - 10;
                        break;
                    }
                case ImagePosition.Center:
                    {
                        xPosOfWm = (float)phWidth / 2;
                        yPosOfWm = (float)phHeight / 2;
                        break;
                    }
                case ImagePosition.BottomLeft:
                    {
                        xPosOfWm = wmWidth;
                        yPosOfWm = phHeight - wmHeight - 10;
                        break;
                    }
                case ImagePosition.TopLeft:
                    {
                        xPosOfWm = wmWidth / 2;
                        yPosOfWm = wmHeight / 2;
                        break;
                    }
                case ImagePosition.TopRight:
                    {
                        xPosOfWm = phWidth - wmWidth - 10;
                        yPosOfWm = wmHeight;
                        break;
                    }
                case ImagePosition.BottomRigth:
                    {
                        xPosOfWm = phWidth - wmWidth - 10;
                        yPosOfWm = phHeight - wmHeight - 10;
                        break;
                    }
                case ImagePosition.TopMiddle:
                    {
                        xPosOfWm = (float)phWidth / 2;
                        yPosOfWm = wmWidth;
                        break;
                    }
                default:
                    {
                        xPosOfWm = wmWidth;
                        yPosOfWm = phHeight - wmHeight - 10;
                        break;
                    }
            }

            //封装文本布局信息（如对齐、文字方向和 Tab 停靠位），显示操作（如省略号插入和国家标准 (National) 数字替换）和 OpenType 功能。
            var strFormat = new StringFormat
            {
                //定义需要印的文字居中对齐
                Alignment = StringAlignment.Center
            };

            //SolidBrush:定义单色画笔。画笔用于填充图形形状，如矩形、椭圆、扇形、多边形和封闭路径。
            //这个画笔为描绘阴影的画笔，呈灰色
            var mAlpha = Convert.ToInt32(255 * alpha);
            var semiTransBrush2 = new SolidBrush(Color.FromArgb(mAlpha, 0, 0, 0));

            //描绘文字信息，这个图层向右和向下偏移一个像素，表示阴影效果
            //DrawString 在指定矩形并且用指定的 Brush 和 Font 对象绘制指定的文本字符串。
            grPhoto.DrawString(waterText, crFont, semiTransBrush2, new PointF(xPosOfWm + 1, yPosOfWm + 1), strFormat);

            //从四个 ARGB 分量（alpha、红色、绿色和蓝色）值创建 Color 结构，这里设置透明度为153
            //这个画笔为描绘正式文字的笔刷，呈白色
            var semiTransBrush = new SolidBrush(Color.FromArgb(mAlpha, 255, 255, 255));

            //第二次绘制这个图形，建立在第一次描绘的基础上
            grPhoto.DrawString(waterText, crFont, semiTransBrush, new PointF(xPosOfWm, yPosOfWm), strFormat);

            //imgPhoto是我们建立的用来装载最终图形的Image对象
            //bmPhoto是我们用来制作图形的容器，为Bitmap对象
            grPhoto.Dispose();

            return bmPhoto;
        }
    }
}
