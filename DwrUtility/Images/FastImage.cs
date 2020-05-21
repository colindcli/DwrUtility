using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DwrUtility.Images
{
    /// <summary>
    /// https://github.com/sdsykes/fastimage
    /// 来源于Stephen Sykes的Ruby版本的Fastimage
    /// 此处并没有全部实现获取图片的宽度和高度
    /// </summary>
    internal class FastImage
    {
        internal static ImageInfo GetImageInfo(Stream stream)
        {
            var imageInfo = new ImageInfo
            {
                ImageFormat = ImgFormat.NotRecognised
            };

            stream.Position = 0;
            var c = GetBytes(stream, 12);
            if (c[0] == 'B' && c[1] == 'M')
            {
                imageInfo.ImageFormat = ImgFormat.Bmp;
                //ParseSizeForBmp(stream, imageInfo);
            }
            else if (c[0] == 'G' && c[1] == 'I' && c[2] == 'F' && c[3] == '8')
            {
                imageInfo.ImageFormat = ImgFormat.Gif;
                //ParseSizeForGif(stream, imageInfo);
            }
            else if (c[0] == 0xff && c[1] == 0xd8 && c[2] == 0xff)
            {
                imageInfo.ImageFormat = ImgFormat.Jpg;
                //ParseSizeForJpeg(stream, imageInfo);
            }
            else if (c[0] == 0x89 && c[1] == 'P' && c[2] == 'N')
            {
                imageInfo.ImageFormat = ImgFormat.Png;
                //ParseSizeForPng(stream, imageInfo);
            }
            else if ((c[0] == 0x49 && c[1] == 0x20 && c[2] == 0x49) ||
                (c[0] == 0x49 && c[1] == 0x49 && c[2] == 0x2A) ||
                (c[0] == 0x4D && c[1] == 0x4D && c[2] == 0x00 && c[3] == 0x2A) ||
                (c[0] == 0x4D && c[1] == 0x4D && c[2] == 0x2A) ||
                (c[0] == 0x4D && c[1] == 0x4D && c[2] == 0x00 && c[3] == 0x2B)
                )
            {
                imageInfo.ImageFormat = ImgFormat.Tiff;
                //ParseSizeForTiff(stream, imageInfo, false);
            }
            else if (c[0] == 0x25 && c[1] == 0x50 && c[2] == 0x44)
            {
                imageInfo.ImageFormat = ImgFormat.Pdf;
            }
            else if (c[0] == '8' && c[1] == 'B' && c[2] == 'P')
            {
                imageInfo.ImageFormat = ImgFormat.Psd;
            }
            else if (c[0] == 0x00 && c[1] == 0x00 && c[2] == 0x02 && c[3] == 0x00 && c[4] == 0x01)
            {
                imageInfo.ImageFormat = ImgFormat.Cur;
            }
            else if (c[0] == 0x00 && c[1] == 0x00 && c[2] == 0x01 && c[3] == 0x00)
            {
                imageInfo.ImageFormat = ImgFormat.Ico;
            }
            else if (c[0] == 'R' && c[1] == 'I' && c[8] == 'W' && c[9] == 'E' && c[10] == 'B' && c[11] == 'P')
            {
                imageInfo.ImageFormat = ImgFormat.Webp;
                //ParseSizeForWebp(stream, imageInfo);
            }
            else if (c[0] == '<' && c[1] == 's')
            {
                imageInfo.ImageFormat = ImgFormat.Svg;
            }
            else if (new Regex("<[?!]").IsMatch(string.Join("", c.Select(p => (char)p))))
            {
                stream.Position = 0;
                var len = (int)stream.Length;
                var bt = GetBytes(stream, len).Select(p => (char)p).ToList();
                var str = string.Join("", bt);
                if (str.IsContains("<svg") && str.IsContains("</svg>"))
                {
                    imageInfo.ImageFormat = ImgFormat.Svg;
                }
            }
            return imageInfo;
        }

        private static void ParseSizeForTiff(Stream stream, ImageInfo imageInfo, bool isBigEndian)
        {
            GetBytes(stream, 2);
            var chars = GetBytes(stream, 4);
            var offset = BitConverter.ToUInt32(chars, 0);
            if (isBigEndian)
            {
                offset = EndianConverter.SwapUInt32(offset);
            }
            Skip(stream, (int)offset - 8);
            chars = GetBytes(stream, 2);
            var tagCount = BitConverter.ToUInt16(chars, 0);
            if (isBigEndian)
            {
                tagCount = EndianConverter.SwapUInt16(tagCount);
            }
            var width = -1;
            var height = -1;
            for (uint i = tagCount; i >= 1; i--)
            {
                chars = GetBytes(stream, 2);
                var type = BitConverter.ToUInt16(chars, 0);
                if (isBigEndian)
                {
                    type = EndianConverter.SwapUInt16(type);
                }
                GetBytes(stream, 6);
                chars = GetBytes(stream, 2);
                var data = BitConverter.ToUInt16(chars, 0);
                if (isBigEndian)
                {
                    data = EndianConverter.SwapUInt16(data);
                }
                if (type == 256) //Width
                {
                    width = data;
                }
                else if (type == 257) //Height
                {
                    height = data;
                }
                if (width > 0 && height > 0)
                {
                    imageInfo.Width = width;
                    imageInfo.Height = height;
                    return;
                }
                GetBytes(stream, 2);
            }
        }

        private static void ParseSizeForPng(Stream stream, ImageInfo imageInfo)
        {
            stream.Position = 0;
            var c = GetBytes(stream, 25);
            var aw = c.Skip(15).Take(4).ToList();
            aw.Reverse();
            var ah = c.Skip(19).Take(4).ToList();
            ah.Reverse();
            imageInfo.Width = BitConverter.ToInt16(aw.ToArray(), 0);
            imageInfo.Height = BitConverter.ToInt16(ah.ToArray(), 0);
        }

        private static void ParseSizeForGif(Stream stream, ImageInfo imageInfo)
        {
            stream.Position = 0;
            var c = GetBytes(stream, 10);
            imageInfo.Width = GetByteValue(c, 6, 2);
            imageInfo.Height = GetByteValue(c, 8, 2);
        }

        private static void ParseSizeForBmp(Stream stream, ImageInfo imageInfo)
        {
            stream.Position = 0;
            var c = GetBytes(stream, 32);
            if (c[14] == 12)
            {
                imageInfo.Width = GetByteValue(c, 18, 2);
                imageInfo.Height = GetByteValue(c, 20, 2);
            }
            else
            {
                imageInfo.Width = GetByteValue(c, 18, 4);
                imageInfo.Height = GetByteValue(c, 22, 4);
            }
        }

        private static void ParseSizeForWebp(Stream stream, ImageInfo imageInfo)
        {
            stream.Position = 0;
            var str = GetPeekArray(stream, 16, 12, 15);
            if (str == "Vp8 ")
            {
                stream.Position = 0;
                var c = GetBytes(stream, 30);
                imageInfo.Width = GetByteValue(c, 26, 2);
                imageInfo.Height = GetByteValue(c, 28, 2);
            }
            else if (str == "VP8L")
            {

            }
            else if (str == "VP8X")
            {

            }
        }

        private static void ParseSizeForJpeg(Stream stream, ImageInfo imageInfo)
        {
            string state = "started";
            while (true)
            {
                byte[] c;
                if (state == "started")
                {
                    c = GetBytes(stream, 1);
                    state = (c[0] == 0xFF) ? "sof" : "started";
                }
                else if (state == "sof")
                {
                    c = GetBytes(stream, 1);
                    if (c[0] >= 0xe0 && c[0] <= 0xef)
                    {
                        state = "skipframe";
                    }
                    else if ((c[0] >= 0xC0 && c[0] <= 0xC3) || (c[0] >= 0xC5 && c[0] <= 0xC7) || (c[0] >= 0xC9 && c[0] <= 0xCB) || (c[0] >= 0xCD && c[0] <= 0xCF))
                    {
                        state = "readsize";
                    }
                    else if (c[0] == 0xFF)
                    {
                        state = "sof";
                    }
                    else
                    {
                        state = "skipframe";
                    }
                }
                else if (state == "skipframe")
                {
                    c = GetBytes(stream, 2);
                    int skip = ReadInt(c) - 2;
                    GetBytes(stream, skip);
                    state = "started";
                }
                else if (state == "readsize")
                {
                    c = GetBytes(stream, 7);
                    imageInfo.Width = ReadInt(new[] { c[5], c[6] });
                    imageInfo.Height = ReadInt(new[] { c[3], c[4] });
                    return;
                }
            }
        }

        /// <summary>
        /// 获取位置字符
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="peekNumber"></param>
        /// <param name="arrayStart"></param>
        /// <param name="arrayEnd"></param>
        /// <returns></returns>
        private static string GetPeekArray(Stream stream, int peekNumber, int arrayStart, int arrayEnd)
        {
            stream.Position = 0;
            var cs = GetBytes(stream, peekNumber);
            var ins = cs.Select(p => (char)p).ToList();
            return string.Join("", ins.Skip(arrayStart).Take(arrayEnd - arrayStart + 1).ToList());
        }

        /// <summary>
        /// 获取图片像素
        /// </summary>
        /// <returns></returns>
        private static int GetByteValue(byte[] bt, int skip, int take)
        {
            var v = BitConverter.ToInt16(bt.Skip(skip).Take(take).ToArray(), 0);
            if (v < 0)
            {
                return -v;
            }

            return v;
        }

        private static byte[] GetBytes(Stream stream, int length)
        {
            var c = new byte[length];
            stream.Read(c, 0, c.Length);
            return c;
        }

        private static void Skip(Stream stream, int length)
        {
            var i = 0;
            while (i < length)
            {
                stream.ReadByte();
                i++;
            }
        }

        private static int ReadInt(byte[] chars)
        {
            return (chars[0] << 8) + chars[1];
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EndianConverter
    {
        public static ushort SwapUInt16(ushort inValue)
        {
            var byteArray = BitConverter.GetBytes(inValue);
            Array.Reverse(byteArray);
            return BitConverter.ToUInt16(byteArray, 0);
        }

        public static uint SwapUInt32(uint inValue)
        {
            var byteArray = BitConverter.GetBytes(inValue);
            Array.Reverse(byteArray);
            return BitConverter.ToUInt32(byteArray, 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 图片格式
        /// </summary>
        public ImgFormat ImageFormat { get; set; }
        /// <summary>
        /// 图片宽度 （如果值为0是不支持取值）
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 图片高度（如果值为0是不支持取值）
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 获取ContentType
        /// </summary>
        public string ContentType => ImageFormat.GetContentType();
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ImgFormat
    {
        /// <summary>
        /// 
        /// </summary>
        Bmp,
        /// <summary>
        /// 
        /// </summary>
        Gif,
        /// <summary>
        /// 
        /// </summary>
        Jpg,
        /// <summary>
        /// 
        /// </summary>
        Png,
        /// <summary>
        /// 
        /// </summary>
        Tiff,
        /// <summary>
        /// 
        /// </summary>
        Webp,
        /// <summary>
        /// 
        /// </summary>
        Svg,
        /// <summary>
        /// 
        /// </summary>
        Psd,
        /// <summary>
        /// 
        /// </summary>
        Pdf,
        /// <summary>
        /// 
        /// </summary>
        Ico,
        /// <summary>
        /// 
        /// </summary>
        Cur,
        /// <summary>
        /// 无法识别的
        /// </summary>
        NotRecognised
    }
}
