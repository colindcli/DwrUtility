using DwrUtility.Converts;
using System.IO;
using System.Text;

namespace DwrUtility
{
    /// <summary>
    /// 文件处理
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns>文件不存在返回null</returns>
        public static string GetFileExtension(string path)
        {
            if (path.IsWhiteSpace())
            {
                return null;
            }

            if (!File.Exists(path))
            {
                return Path.GetExtension(path);
            }

            return new FileInfo(path).Extension;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long GetFileSize(string path)
        {
            return DirectoryHelper.GetFileSize(path);
        }

        /// <summary>
        /// 批量删除文件（全部删除成功true，否则false）
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>全部删除成功true，否则false</returns>
        public static bool DeleteFiles(params string[] paths)
        {
            return DirectoryHelper.DeleteFiles(paths);
        }

        /// <summary>
        /// 批量删除文件（全部删除成功true，否则false）
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>全部删除成功true，否则false</returns>
        public static bool DeleteFilesToRecycleBin(params string[] paths)
        {
            return DirectoryHelper.DeleteFilesToRecycleBin(paths);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="toRecycleBin">是否移入回收站</param>
        /// <returns></returns>
        public static bool DeleteFile(string path, bool toRecycleBin)
        {
            return DirectoryHelper.DeleteFile(path, toRecycleBin);
        }

        /// <summary>
        /// 读取txt文本内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns>文件不存在返回null</returns>
        public static string ReadText(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var bt = new byte[fs.Length];
            fs.Read(bt, 0, bt.Length);
            fs.Close();
            fs.Dispose();

            var encoding = bt.GetEncoding();
            if (encoding != null)
            {
                return File.ReadAllText(path, Encoding.GetEncoding(encoding));
            }

            //gb2312
            var txt = File.ReadAllText(path, Encoding.UTF8);
            if (txt.Contains("�"))
            {
                return File.ReadAllText(path, Encoding.GetEncoding("gb2312"));
            }

            //gb2312
            //var txt2 = File.ReadAllText(path, Encoding.BigEndianUnicode);
            //if (txt2.Contains("�"))
            //{
            //    return File.ReadAllText(path, Encoding.GetEncoding("gb2312"));
            //}

            //utf-8
            return File.ReadAllText(path, Encoding.UTF8);
        }
    }
}
