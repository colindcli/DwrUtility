using System;
using System.IO;

namespace DwrUtility
{
    /// <summary>
    /// 相对路径与绝对路径转换
    /// </summary>
    public class RelativeHelper
    {
        /// <summary>
        /// 获取参照baseUri的相对路径的目标网址
        /// </summary>
        /// <param name="basePath">参照Uri</param>
        /// <param name="relativePath">相对路径</param>
        /// <returns>目标网址</returns>
        public static string RelativePathToFullPath(string basePath, string relativePath)
        {
            if (basePath == null)
            {
                return "";
            }

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return basePath;
            }

            try
            {
                return Path.GetFullPath($"{basePath}{relativePath}");
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                return "";
            }
        }

        /// <summary>
        /// 获取参照baseUri的uri的相对路径
        /// </summary>
        /// <param name="basePath">参照Uri</param>
        /// <param name="fullPath">目标Uri</param>
        /// <returns>目标路径</returns>
        public static string FullPathToRelativePath(string basePath, string fullPath)
        {
            if (basePath == null)
            {
                return "";
            }

            if (fullPath == null)
            {
                return "";
            }

            var basePathUri = basePath.ToUri();
            if (basePathUri == null)
            {
                return "";
            }

            var fullPathUri = fullPath.ToUri();
            if (fullPathUri == null)
            {
                return "";
            }

            return UriToRelativePath(basePathUri, fullPathUri);
        }

        /// <summary>
        /// 获取参照baseUri的相对路径的目标网址
        /// </summary>
        /// <param name="baseUri">参照Uri</param>
        /// <param name="relativePath">相对路径</param>
        /// <returns>目标网址</returns>
        public static string RelativePathToUrl(Uri baseUri, string relativePath)
        {
            if (baseUri == null)
            {
                return "";
            }

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return baseUri.OriginalString;
            }

            return Uri.TryCreate(baseUri, relativePath, out var newUri) ? newUri?.OriginalString : "";
        }

        /// <summary>
        /// 获取参照baseUri的uri的相对路径
        /// </summary>
        /// <param name="baseUri">参照Uri</param>
        /// <param name="uri">目标Uri</param>
        /// <returns>目标路径</returns>
        public static string UriToRelativePath(Uri baseUri, Uri uri)
        {
            if (baseUri == null)
            {
                return "";
            }

            if (uri == null)
            {
                return "";
            }

            try
            {
                var relativeUri = baseUri.MakeRelativeUri(uri);
                return relativeUri.OriginalString;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                return "";
            }
        }
    }
}
