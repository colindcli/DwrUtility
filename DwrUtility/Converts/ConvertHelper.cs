using System.IO;
using System.Text;

namespace DwrUtility.Converts
{
    /// <summary>
    /// 转换类
    /// </summary>
    public class ConvertHelper
    {
        /// <summary>
        /// 从字节流判断编码（返回null是不能判断出编码）
        /// </summary>
        /// <param name="bt">输入字节流</param>
        /// <returns></returns>
        public static string GetEncoding(byte[] bt)
        {
            //带BOM的utf-8
            var utf8 = new byte[] { 0xEF, 0xBB, 0xBF };
            if (bt[0] == utf8[0] && bt[1] == utf8[1] && bt[2] == utf8[2])
            {
                return "utf-8";
            }

            //UTF-32-BE
            var utf32Be = new byte[] { 0x00, 0x00, 0xFE, 0xFF };
            if (bt[0] == utf32Be[0] &&
                bt[1] == utf32Be[1] &&
                bt[2] == utf32Be[2] &&
                bt[3] == utf32Be[3])
            {
                return "utf-32";
            }

            //UTF-32-LE
            var utf32Le = new byte[] { 0xFF, 0xFE, 0x00, 0x00 };
            if (bt[0] == utf32Le[0] &&
                bt[1] == utf32Le[1] &&
                bt[2] == utf32Le[2] &&
                bt[3] == utf32Le[3])
            {
                return "utf-32";
            }

            //UTF-32-2143
            var utf322143 = new byte[] { 0x00, 0x00, 0xFF, 0xFE };
            if (bt[0] == utf322143[0] &&
                bt[1] == utf322143[1] &&
                bt[2] == utf322143[2] &&
                bt[3] == utf322143[3])
            {
                return "utf-32";
            }

            //UTF-32-3412
            var utf323412 = new byte[] { 0xFE, 0xFF, 0x00, 0x00 };
            if (bt[0] == utf323412[0] &&
                bt[1] == utf323412[1] &&
                bt[2] == utf323412[2] &&
                bt[3] == utf323412[3])
            {
                return "utf-32";
            }

            //UTF-16-BE
            var utf16Be = new byte[] { 0xFE, 0xFF };
            if (bt[0] == utf16Be[0] &&
                bt[1] == utf16Be[1])
            {
                return "utf-16";
            }

            //UTF-16-LE
            var utf16Le = new byte[] { 0xFF, 0xFE };
            if (bt[0] == utf16Le[0] &&
                bt[1] == utf16Le[1])
            {
                return "utf-16";
            }

            return null;
        }

        /// <summary>
        /// 将流读取到内存
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isCloseStream">读取完是否关闭流</param>
        /// <returns></returns>
        public static MemoryStream ReadAsMemoryStream(Stream input, bool isCloseStream)
        {
            if (input == null)
            {
                return null;
            }

            var buffer = new byte[16384];
            using (var ms = new MemoryStream())
            {
                int count;
                input.Position = 0;
                while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, count);
                }

                if (isCloseStream)
                {
                    input.Close();
                    input.Dispose();
                }

                return ms;
            }
        }

        /// <summary>
        /// 字符串转内存流
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(string str, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(str));
        }
    }
}
