using System;
using System.Net;

namespace DwrUtility.Https
{
    /// <summary>
    /// Http请求参数
    /// </summary>
    public class HttpRequestParam
    {
        /// <summary>
        /// UserAgent默认：Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36";
        /// <summary>
        /// Accept默认：text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9
        /// </summary>
        public string Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
        /// <summary>
        /// 来路
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 设置Cookie，优先使用CookieValue，其次使用CookieContainer
        /// 获取方法：CookieHelper.ToCookieContainer(cookie, url)
        /// </summary>
        [Obsolete("已弃用，使用CookieValue字段")]
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// 设置Cookie，优先使用CookieValue，其次使用CookieContainer
        /// </summary>
        public string CookieValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private int _Timeout { get; set; } = 30000;

        /// <summary>
        /// 连接超时，默认30000（单位毫秒）
        /// </summary>
        public int Timeout
        {
            get
            {
                if (!DownloadTimeout.HasValue)
                {
                    return _Timeout;
                }

                return DownloadTimeout.Value < _Timeout ? DownloadTimeout.Value : _Timeout;
            }
            set => _Timeout = value;
        }

        /// <summary>
        /// 下载超时（单位毫秒）
        /// </summary>
        public int? DownloadTimeout { get; set; }

        /// <summary>
        /// 允许自动重定向
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = true;

        /// <summary>
        /// 处理接收数据事件 (参数：本批字节数，总字节数)
        /// </summary>
        public Action<long, long> Progress { get; set; }
    }
}
