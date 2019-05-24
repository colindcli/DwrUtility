using System;

namespace DwrUtility.Email
{
    public class EmailParam
    {
        /// <summary>
        /// smtp服务，如：smtp.163.com
        /// </summary>
        public string SmtpServerAddress { get; set; }
        /// <summary>
        /// smtp用户名：xxx@163.com
        /// </summary>
        public string SmtpUserName { get; set; }
        /// <summary>
        /// smtp邮箱地址：xxx@163.com
        /// </summary>
        public string FromMailAddress { get; set; }
        /// <summary>
        /// smtp密码
        /// </summary>
        public string SmtpUserPassword { get; set; }
        /// <summary>
        /// 端口，默认25
        /// </summary>
        public int SmtpServerPortNumber { get; set; } = 25;
        /// <summary>
        /// 是否启用安全套接字层，默认false
        /// </summary>
        public bool EnableSsl { get; set; } = false;

        /// <summary>
        /// 日志(可选)
        /// </summary>
        public Action<Exception> Log { get; set; }
    }
}
