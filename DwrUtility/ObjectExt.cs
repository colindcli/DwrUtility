using System;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DwrUtility
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class ObjectExt
    {
        /// <summary>
        /// Unix时间戳（13位数Ticks(精确到毫秒)）转时间
        /// </summary>
        /// <param name="millisecondTicks">Unix时间戳: 13位数Ticks(精确到毫秒)，如：1557040736000</param>
        public static DateTime ToDatetime(this long millisecondTicks)
        {
            return new DateTime((millisecondTicks + 62135596800000) * 10000).ToLocalTime();
        }

        /// <summary>
        /// 转Unix时间戳 (精确到毫秒)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>精确到毫秒</returns>
        public static long ToTicks(this DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        /// <summary>
        /// Guid转SqlGuid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static SqlGuid ToSqlGuid(this Guid guid)
        {
            return new SqlGuid(guid);
        }

        /// <summary>
        /// 转Uri
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Uri ToUri(this string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            return Uri.TryCreate(url, UriKind.Absolute, out var result) ? result : null;
        }

        /// <summary>
        /// 对比是否相等
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool IsEquals(this string s1, string s2)
        {
            if (s1 == null || s2 == null)
            {
                return false;
            }

            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// str1包含str2
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool IsContains(this string str1, string str2)
        {
            if (str1 == null)
            {
                str1 = "";
            }

            if (str2 == null)
            {
                str2 = "";
            }
            return str1.IndexOf(str2, StringComparison.OrdinalIgnoreCase) > -1;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <returns></returns>
        public static void CreateDir(this string dirPath)
        {
            DirectoryHelper.CreateDir(dirPath);
        }

        /// <summary>
        /// 根据文件名创建文件夹
        /// </summary>
        /// <returns></returns>
        public static void CreateDirByFilePath(this string filePath)
        {
            DirectoryHelper.CreateDirByFilePath(filePath);
        }

        /// <summary>
        /// 转Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return default(Guid);
            }

            return Guid.TryParse(id, out var g) ? g : default(Guid);
        }

        /// <summary>
        /// 转Int
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int ToInt(this string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return default(int);
            }

            return int.TryParse(id, out var g) ? g : default(int);
        }

        /// <summary>
        /// 转Long
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static long ToLong(this string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return default(long);
            }

            return long.TryParse(id, out var g) ? g : default(long);
        }

        /// <summary>
        /// 强制转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T To<T>(this object id)
        {
            if (id == null)
            {
                return default(T);
            }

            try
            {
                return (T)id;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 是否电子邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var regex = new Regex("^(.*)@(.*)\\.(.*)$");
            return regex.IsMatch(email);
        }

        private static readonly Regex RegexHtml1 = new Regex("<[^>]+>", RegexOptions.Singleline);
        private static readonly Regex RegexHtml2 = new Regex("&[^;]+;", RegexOptions.Singleline);
        private static readonly Regex RegexHtml3 = new Regex("[ ]+", RegexOptions.Singleline);
        /// <summary>
        /// 删除内容html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ClearHtml(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return "";
            }
            var strText = RegexHtml1.Replace(html, "");
            strText = RegexHtml2.Replace(strText,  "");
            return RegexHtml3.Replace(strText,  "");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Cut(this string html, int length)
        {
            html = ClearHtml(html);

            if (length > 0 && html.Length > length)
            {
                return html.Substring(0, length) + "...";
            }

            return html;
        }

        /// <summary>
        /// 获取可执行代码的lambda表达式成员的名称
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TDelegate">可执行代码的 lambda 表达式</typeparam>
        /// <returns></returns>
        public static string GetPropertyName<TDelegate>(this Expression<TDelegate> expression)
        {
            if (expression == null)
            {
                return "";
            }

            var rtn = "";
            if (expression.Body is UnaryExpression body)
            {
                rtn = ((MemberExpression)body.Operand).Member.Name;
            }
            else if (expression.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expression.Body).Member.Name;
            }
            else if (expression.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expression.Body).Type.Name;
            }
            return rtn;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            return EncryptionHelper.Md5(str);
        }

        /// <summary>
        /// 文件MD5值
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>文件不存在或报错返回NULL</returns>
        public static string ToFileMd5(this string fileName)
        {
            return EncryptionHelper.FileMd5(fileName);
        }
    }
}
