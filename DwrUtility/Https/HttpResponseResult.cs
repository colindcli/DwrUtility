using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DwrUtility.Https
{
    /// <summary>
    /// 输出内容
    /// </summary>
    public class HttpResponseResult
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private string _Content { get; set; }

        /// <summary>
        /// 获取内容（输入编码）较快
        /// </summary>
        /// <param name="encoding">默认utf-8</param>
        /// <returns></returns>
        public string GetContent(Encoding encoding = null)
        {
            if (_Content != null)
            {
                return _Content;
            }

            if (RawBytes == null || RawBytes.Length == 0)
            {
                return null;
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            _Content = encoding.GetString(RawBytes);
            return _Content;
        }

        /// <summary>
        /// 获取内容（自动识别编码）较慢
        /// </summary>
        public string Content
        {
            get
            {
                if (_Content != null)
                {
                    return _Content;
                }

                if (RawBytes == null || RawBytes.Length == 0)
                {
                    return null;
                }

                var encoding = HttpHelper.GetEncoding(RawBytes, ContentType);

                _Content = encoding.GetString(RawBytes);
                return _Content;
            }
        }

        /// <summary>
        /// 是否成功，HttpStatusCode.OK，并且正常读取内容
        /// </summary>
        public bool IsSuccessful { get; internal set; }
        /// <summary>
        /// 状态
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }
        /// <summary>
        /// 字节
        /// </summary>
        public byte[] RawBytes { get; internal set; }
        /// <summary>
        /// 报错
        /// </summary>
        public Exception Exception { get; internal set; }
        /// <summary>
        /// Response Header编码
        /// </summary>
        public string ContentType { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Headers { get; internal set; }
    }

    internal class MsStreamModel : MemoryStream
    {
        /// <summary>
        /// 
        /// </summary>
        public Action<long> Progress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] array, int offset, int count)
        {
            base.Write(array, offset, count);
            Progress?.Invoke(count);
        }
    }
}
