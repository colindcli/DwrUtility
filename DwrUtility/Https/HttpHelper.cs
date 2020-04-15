using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DwrUtility.Https
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 获取页面内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="encoding">如果传编码就使用此编码，否则使用猜测编码</param>
        /// <returns></returns>
        public static string GetHtml(string url, string body = null, Encoding encoding = null)
        {
            if (encoding == null)
            {
                var coding = GetEncoding(url, out var buf, body);
                return coding.GetString(buf);
            }
            else
            {
                var buf = GetByte(url, out var _, body);
                return encoding.GetString(buf);
            }
        }

        /// <summary>
        /// 获取网址字节
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static byte[] GetByte(string url, string body = null)
        {
            return GetByte(url, out var _, body);
        }

        /// <summary>
        /// 获取网站编码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="buf"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(string url, out byte[] buf, string body = null)
        {
            buf = GetByte(url, out var response, body);
            return GetEncoding(buf, response.ContentType);
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(byte[] buf, string contentType)
        {
            //编码
            var code = GetEncodingByByte(buf);
            if (code == null)
            {
                code = GetEncodingByResponseContentType(contentType);
                if (code == null)
                {
                    var content = Encoding.UTF8.GetString(buf);
                    code = GetEncodingByHtml(content) ?? (content.Contains("�") ? "gb2312" : "utf-8");
                }
            }
            var coding = Encoding.UTF8;
            try
            {
                coding = Encoding.GetEncoding(code);
            }
            catch (Exception)
            {
                // ignored
            }

            return coding;
        }

        /// <summary>
        /// 获取字节
        /// </summary>
        /// <param name="url"></param>
        /// <param name="response"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private static byte[] GetByte(string url, out HttpWebResponse response, string body = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            request.ContentType = "*.*";

            if (!string.IsNullOrEmpty(body))
            {
                var requestStream = request.GetRequestStream();
                var bytes = Encoding.UTF8.GetBytes(body);
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }

            //读取字节
            response = (HttpWebResponse)request.GetResponse();
            return GetResponseBytes(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static byte[] GetResponseBytes(HttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                {
                    return null;
                }

                var count = (int)response.ContentLength;
                if (count != -1)
                {
                    var offset = 0;
                    var buf = new byte[count];
                    while (count > 0)
                    {
                        var n = responseStream.Read(buf, offset, count);
                        if (n == 0)
                        {
                            break;
                        }
                        count -= n;
                        offset += n;
                    }

                    return buf;
                }

                //Transfer-Encoding: chunked
                var list = new List<byte>();
                var bt = new byte[4096];
                int m;
                while ((m = responseStream.Read(bt, 0, bt.Length)) != 0)
                {
                    list.AddRange(bt.Take(m));
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// 从字节流判断编码（返回null是不能判断出编码）
        /// </summary>
        /// <param name="bt">输入字节流</param>
        /// <returns></returns>
        internal static string GetEncodingByByte(byte[] bt)
        {
            if (bt == null || bt.Length == 0)
            {
                return null;
            }

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
        /// 获取Header编码 [text/html; charset=UTF-8]
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        internal static string GetEncodingByResponseContentType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return null;
            }

            var list = contentType.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string encoding = null;
            foreach (var item in list)
            {
                var kv = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (kv[0].Trim().Equals("charset", StringComparison.OrdinalIgnoreCase) && kv.Count == 2)
                {
                    encoding = kv[1].Trim();
                    break;
                }
            }
            return encoding;
        }


        /// <summary>
        /// 从Html中获取编码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        internal static string GetEncodingByHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return null;
            }

            var charsets = GetCharsetMetas(html);
            if (charsets.Count == 0)
            {
                return null;
            }

            var obj = charsets.First();
            var meta = obj.Meta;
            if (meta.IndexOf("utf-8", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return "utf-8";
            }
            if (meta.IndexOf("gb2312", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return "gb2312";
            }
            if (meta.IndexOf("gbk", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return "gbk";
            }

            var codeType = obj.CodeType;
            if (codeType == CodeTypeEnum.Html4)
            {
                return GetMetaEncodingByHtml4(meta);
            }

            if (codeType == CodeTypeEnum.Html5)
            {
                return GetMetaEncodingByHtml5(meta);
            }
            return null;
        }

        private static readonly Regex RegMeta = new Regex("<meta[^>]+>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex RegMetaCharset = new Regex("charset", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 获取html代码mata编码值
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        internal static List<EncodingModel> GetCharsetMetas(string html)
        {
            var charsets = new List<EncodingModel>();
            var mcs = RegMeta.Matches(html);
            if (mcs.Count == 0)
            {
                return charsets;
            }

            charsets.AddRange(from Match item in mcs
                              let b = RegMetaCharset.IsMatch(item.Value)
                              where b
                              select new EncodingModel()
                              {
                                  Meta = item.Value,
                                  CodeType = item.Value.IndexOf("content", StringComparison.OrdinalIgnoreCase) > -1 ? CodeTypeEnum.Html4 : CodeTypeEnum.Html5
                              });
            return charsets;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meta"></param>
        /// <returns></returns>
        internal static string GetMetaEncodingByHtml4(string meta)
        {
            meta = RegMetaEq.Replace(meta, "=");
            var mc = RegMetaQu4.Match(meta);
            if (mc.Success)
            {
                return GetEncodingByResponseContentType(mc.Value);
            }

            return null;
        }

        private static readonly Regex RegMetaEq = new Regex("[ ]*=[ ]*", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        /// <summary>
        /// 单引号和双引号
        /// </summary>
        private static readonly Regex RegMetaQu4 = new Regex("(?<=content=\")[^\"]+(?=\")|(?<=content=')[^']+(?=')", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex RegMetaQu5 = new Regex("(?<=charset=\")[^\"]+(?=\")|(?<=charset=')[^']+(?=')", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        /// <summary>
        /// 不带引号的
        /// </summary>
        private static readonly Regex RegMetaQuNot = new Regex("(?<=charset=)[^\"' >/]+", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meta"></param>
        /// <returns></returns>
        internal static string GetMetaEncodingByHtml5(string meta)
        {
            meta = RegMetaEq.Replace(meta, "=");
            var mc = RegMetaQu5.Match(meta);
            if (mc.Success)
            {
                return mc.Value.Trim();
            }

            mc = RegMetaQuNot.Match(meta);
            if (mc.Success)
            {
                return mc.Value.Trim();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        internal class EncodingModel
        {
            /// <summary>
            /// Meta字符串
            /// </summary>
            internal string Meta { get; set; }
            /// <summary>
            /// H5(charset="UTF-8"):1; H4( content="text/html;charset=gb2312"):2  
            /// </summary>
            internal CodeTypeEnum CodeType { get; set; }
        }
        /// <summary>
        /// meta编码
        /// </summary>
        internal enum CodeTypeEnum
        {
            /// <summary>
            /// Html4的设置方式
            /// </summary>
            Html4 = 4,
            /// <summary>
            /// Html5的设置方式
            /// </summary>
            Html5 = 5
        }
    }
}
