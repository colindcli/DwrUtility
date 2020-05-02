using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace DwrUtility
{
    /// <summary>
    /// 压缩和解压缩
    /// </summary>
    public class ZipHelper
    {
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="zipPath">压缩文件</param>
        /// <param name="dir">解压目录</param>
        /// <param name="msg"></param>
        public static bool Decompression(string zipPath, string dir, out string msg)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipPath, dir);
                msg = null;
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dir">被压缩文件夹</param>
        /// <param name="zipPath">生成压缩包</param>
        /// <param name="msg">失败时输出消息</param>
        /// <returns></returns>
        public static bool Eecompression(string dir, string zipPath, out string msg)
        {
            //创建文件夹
            zipPath.CreateDirByFilePath();

            if (File.Exists(zipPath))
            {
                msg = "压缩文件已存在，压缩失败";
                return false;
            }

            if (!Directory.Exists(dir))
            {
                msg = "压缩文件夹不存在，压缩失败";
                return false;
            }

            ZipFile.CreateFromDirectory(dir, zipPath);

            var flag = File.Exists(zipPath);
            if (flag)
            {
                msg = null;
                return true;
            }

            msg = "压缩包不存在，压缩失败";
            return false;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filePaths">压缩文件集合</param>
        /// <param name="zipPath">生成压缩包</param>
        /// <param name="msg">失败时输出消息</param>
        public static bool Eecompression(List<string> filePaths, string zipPath, out string msg)
        {
            //创建文件夹
            zipPath.CreateDirByFilePath();

            if (File.Exists(zipPath))
            {
                msg = "压缩文件已存在，压缩失败";
                return false;
            }

            //检查文件是否都存在
            foreach (var filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    continue;
                }

                msg = $"被压缩文件[{new FileInfo(filePath).Name}]不存在，压缩失败";
                return false;
            }

            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (var filePath in filePaths)
                {
                    archive.CreateEntryFromFile(filePath, new FileInfo(filePath).Name);
                }
            }

            var flag = File.Exists(zipPath);
            if (flag)
            {
                msg = null;
                return true;
            }

            msg = "压缩包不存在，压缩失败";
            return false;
        }
    }
}
