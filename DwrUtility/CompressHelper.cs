using System.IO;
using System.IO.Compression;

namespace DwrUtility
{
    /// <summary>
    /// 压缩与解压缩
    /// </summary>
    public class CompressHelper
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                using (var outStream = new MemoryStream())
                {
                    var gzip = new GZipStream(outStream, CompressionMode.Compress, true);
                    var buf = new byte[10240];
                    int len;
                    stream.Position = 0;
                    while ((len = stream.Read(buf, 0, buf.Length)) > 0)
                    {
                        gzip.Write(buf, 0, len);
                    }
                    gzip.Close();
                    return outStream.ToArray();
                }
            }
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] buffer)
        {
            using (var mem = new MemoryStream(buffer) { Position = 0 })
            {
                using (var outStream = new MemoryStream())
                {
                    var gzip = new GZipStream(mem, CompressionMode.Decompress, true);
                    int len;
                    var buf = new byte[10240];
                    while ((len = gzip.Read(buf, 0, buf.Length)) > 0)
                    {
                        outStream.Write(buf, 0, len);
                    }
                    gzip.Close();
                    return outStream.ToArray();
                }
            }
        }
    }
}
