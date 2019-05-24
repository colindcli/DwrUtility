using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DwrUtility.Email
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public class EmailHelper
    {
        private EmailParam Smtp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtp"></param>
        public EmailHelper(EmailParam smtp)
        {
            Smtp = smtp;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">接收邮箱地址</param>
        /// <param name="displayName">接收邮箱地址显示名称</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public bool Send(string toEmail, string displayName, string subject, string body)
        {
            var smtp = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Host = Smtp.SmtpServerAddress,
                Port = Smtp.SmtpServerPortNumber,
                Credentials = new NetworkCredential(Smtp.SmtpUserName, Smtp.SmtpUserPassword),
                EnableSsl = Smtp.EnableSsl,
            };

            try
            {
                var msg = new MailMessage(new MailAddress(Smtp.FromMailAddress, displayName),
                    new MailAddress(toEmail))
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                };
                smtp.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                Smtp.Log?.Invoke(ex);
                return false;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">接收邮箱地址</param>
        /// <param name="displayName">接收邮箱地址显示名称</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public Task<bool> SendAsynic(string toEmail, string displayName, string subject, string body)
        {
            return Task.Run(() => Send(toEmail, displayName, subject, body));
        }
    }
}
