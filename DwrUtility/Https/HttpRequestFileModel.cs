using System;
using System.IO;

namespace DwrUtility.Https
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpRequestFileModel
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpRequestFileModel()
        {

        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="filePath"></param>
        public HttpRequestFileModel(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"文件不存在: {filePath}");
            }

            var fileInfo = new FileInfo(filePath);
            Name = fileInfo.Name;
            Filename = fileInfo.Name;
            Stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// input标签的name值
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 上传的文件名称（并非路径）
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// 默认：application/octet-stream
        /// </summary>
        public string ContentType { get; set; } = "application/octet-stream";
        /// <summary>
        /// 文件流
        /// </summary>
        public Stream Stream { get; set; }
    }
}
