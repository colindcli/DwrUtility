using System.Drawing.Imaging;

namespace DwrUtility.Images
{
    /// <summary>
    /// 
    /// </summary>
    public static class FastImageExt
    {
        /// <summary>
        /// 获取ContentType
        /// </summary>
        /// <param name="format"></param>
        /// <param name="defaultContentType"></param>
        /// <returns></returns>
        public static string GetContentType(this ImgFormat format, string defaultContentType = "image/jpeg")
        {
            string contentType;
            switch (format)
            {
                case ImgFormat.Bmp:
                    {
                        contentType = ContentTypeHelper.ImageBmp;
                        break;
                    }
                case ImgFormat.Gif:
                    {
                        contentType = ContentTypeHelper.ImageGif;
                        break;
                    }
                case ImgFormat.Jpg:
                    {
                        contentType = ContentTypeHelper.ImageJpg;
                        break;
                    }
                case ImgFormat.Png:
                    {
                        contentType = ContentTypeHelper.ImagePng;
                        break;
                    }
                case ImgFormat.Tiff:
                    {
                        contentType = ContentTypeHelper.ImageTiff;
                        break;
                    }
                case ImgFormat.Svg:
                    {
                        contentType = ContentTypeHelper.ImageSvg;
                        break;
                    }
                case ImgFormat.Pdf:
                    {
                        contentType = ContentTypeHelper.ApplicationPdf;
                        break;
                    }
                case ImgFormat.Psd:
                    {
                        contentType = ContentTypeHelper.ApplicationPsd;
                        break;
                    }
                case ImgFormat.Ico:
                    {
                        contentType = ContentTypeHelper.ImageIcon;
                        break;
                    }
                case ImgFormat.Cur:
                    {
                        contentType = ContentTypeHelper.ImageCur;
                        break;
                    }
                default:
                    {
                        contentType = defaultContentType;
                        break;
                    }
            }

            return contentType;
        }

        /// <summary>
        /// 转换成Net内置的ImageFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="defaultImageFormat">转换成系统支持的图片格式</param>
        /// <returns></returns>
        public static ImageFormat GetImageFormat(this ImgFormat format, ImageFormat defaultImageFormat)
        {
            var imageFormat = defaultImageFormat;
            switch (format)
            {
                case ImgFormat.Bmp:
                    {
                        imageFormat = ImageFormat.Bmp;
                        break;
                    }
                case ImgFormat.Gif:
                    {
                        imageFormat = ImageFormat.Gif;
                        break;
                    }
                case ImgFormat.Jpg:
                    {
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    }
                case ImgFormat.Png:
                    {
                        imageFormat = ImageFormat.Png;
                        break;
                    }
                case ImgFormat.Tiff:
                    {
                        imageFormat = ImageFormat.Tiff;
                        break;
                    }
                case ImgFormat.Ico:
                    {
                        imageFormat = ImageFormat.Icon;
                        break;
                    }
            }

            return imageFormat;
        }
    }
}