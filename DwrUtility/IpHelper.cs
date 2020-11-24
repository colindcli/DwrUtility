#if NETFULL
using System;
using System.Linq;
using System.Web;

namespace DwrUtility
{
    /// <summary>
    /// Ip地址类
    /// </summary>
    public class IpHelper
    {
        /// <summary>
        /// 获取Web客户端IP地址(有反向代理是跳过代理获取客户端IP)
        /// Nginx http节点下配置：
        /// proxy_set_header Host $host;
        /// proxy_set_header X-Real-IP $remote_addr;
        /// proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIpWithProxy()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            var request = HttpContext.Current.Request;
            var xForwardedFor = request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrWhiteSpace(xForwardedFor))
            {
                var ip = xForwardedFor.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    return ip;
                }
            }

            var xRealIp = request.Headers["X-Real-IP"];
            if (!string.IsNullOrWhiteSpace(xRealIp))
            {
                return xRealIp;
            }

            return GetWebClientIp();
        }

        /// <summary>
        /// 获取Web客户端IP地址(直接获取REMOTE_ADDR的IP地址)
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIp()
        {
            if (HttpContext.Current?.Request.ServerVariables == null)
            {
                return null;
            }

            var request = HttpContext.Current.Request;
            return request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress;
        }

        //ip 获取地址：http://int.dpool.sina.com.cn/iplookup/iplookup.php?ip=
    }
}

#endif
