using DwrUtility.Converts;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 获取编码
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(byte[] buf, string contentType)
        {
            //从字节中获取编码
            var code = buf.GetEncoding();
            if (code == null)
            {
                //从Header中获取编码
                code = GetEncodingByResponseContentType(contentType);

                if (code == null)
                {
                    //从内容中获取编码
                    code = GetEncodingByHtml(Encoding.UTF8.GetString(buf));
                    if (code == null)
                    {
                        //猜测编码
                        if (Encoding.UTF8.GetString(buf).Contains("�")/* || Encoding.BigEndianUnicode.GetString(buf).Contains("�")*/)
                        {
                            code = "gb2312";
                        }
                        else
                        {
                            code = "utf-8";
                        }
                    }
                }
            }
            var coding = Encoding.UTF8;
            try
            {
                coding = Encoding.GetEncoding(code);
            }
            catch (Exception)
            {
                // 报错默认utf-8编码
            }

            return coding;
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
