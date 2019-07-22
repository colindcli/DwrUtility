using System.IO;

namespace DwrUtility.Converts
{
    /// <summary>
    /// 转换类
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// 将流读取为字节
        /// </summary>
        /// <param name="input">数据流</param>
        /// <param name="isCloseStream">读取完是否关闭流</param>
        /// <returns></returns>
        public static byte[] ReadAsBytes(this Stream input, bool isCloseStream)
        {
            return ConvertHelper.ReadAsMemoryStream(input, isCloseStream)?.ToArray();
        }

        /// <summary>
        /// 将流读取到内存
        /// </summary>
        /// <param name="input">数据流</param>
        /// <param name="isCloseStream">读取完是否关闭流</param>
        /// <returns></returns>
        public static MemoryStream ReadAsMemoryStream(this Stream input, bool isCloseStream)
        {
            return ConvertHelper.ReadAsMemoryStream(input, isCloseStream);
        }
    }
}
