#if NETFULL
using DwrUtility.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DwrUtility
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string cookieName, string cookieValue, DateTime expires)
        {
            var cookie = new HttpCookie(cookieName, cookieValue)
            {
                Value = cookieValue,
                Expires = expires
            };
            HttpContext.Current.Response.SetCookie(cookie);
        }

        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>无值返回null</returns>
        public static string GetCookie(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            return cookie?.Value;
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        public static void ClearCookie(string cookieName)
        {
            var responseCookie = HttpContext.Current.Response.Cookies[cookieName];
            if (responseCookie != null)
            {
                responseCookie.Expires = DateTime.Now.AddYears(-1000);
            }
        }

        /// <summary>
        /// Cookie字符串转CookieContainer
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static CookieContainer ToCookieContainer(string cookie, string domain)
        {
            if (string.IsNullOrWhiteSpace(cookie))
            {
                return null;
            }

            var kvs = ParseCookie(cookie);
            var cc = new CookieContainer();
            foreach (var kv in kvs)
            {
                try
                {
                    cc.Add(new Cookie(kv.Key, kv.Value, "/", domain));
                }
                catch (Exception)
                {
                    // 键值为非法字符时报错
                }
            }

            return cc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private static List<KeyValue> ParseCookie(string cookie)
        {
            if (string.IsNullOrWhiteSpace(cookie))
            {
                return new List<KeyValue>();
            }

            var list = cookie.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(m =>
                {
                    var str = m.Trim();
                    var index = str.IndexOf("=", 0, StringComparison.Ordinal);
                    return index > -1 ? new { Key = str.Substring(0, index), Value = str.Substring(index + 1, str.Length - index - 1) } : null;
                }).Where(p => p != null)
                //键值相同后一个覆盖前一个
                .ToDict(p => p.Key, p => p.Value, false, StringComparer.OrdinalIgnoreCase)
                .Select(p => new KeyValue()
                {
                    Key = p.Key,
                    Value = p.Value
                }).ToList();

            return list;
        }
    }
}

#endif
