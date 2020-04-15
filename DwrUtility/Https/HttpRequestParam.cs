using System.Net;

namespace DwrUtility.Https
{
    /// <summary>
    /// Http请求参数
    /// </summary>
    public class HttpRequestParam
    {
        /// <summary>
        /// UserAgent默认：Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
        /// <summary>
        /// Accept默认：text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3
        /// </summary>
        public string Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
        /// <summary>
        /// Cookie
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

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
    }
}
