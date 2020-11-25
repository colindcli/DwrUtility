using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DwrUtility.Web.Controllers
{
    public class RequestController : Controller
    {
        public ActionResult TimeOut(int time)
        {
            Thread.Sleep(time);
            return Content("OK");
        }

        /// <summary>
        /// Chunked输出
        /// </summary>
        [HttpPost]
        public void PostChunked(Guid id)
        {
            GetChunked(id);
        }

        /// <summary>
        /// Chunked输出
        /// </summary>
        [HttpGet]
        public void GetChunked(Guid id)
        {
            var path = $"{AppDomain.CurrentDomain.BaseDirectory.TrimSlash()}/Logs/{id}.txt";
            path.CreateDirByFilePath();

            var sb = new StringBuilder("START----------");
            for (var i = 0; i < 4000; i++)
            {
                sb.Append(Guid.NewGuid());
            }
            sb.Append("----------END");

            System.IO.File.WriteAllText(path, sb.ToString());
            DoResponseFile(HttpContext.Response, path);
        }

        /// <summary>
        /// 以文件形式响应
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="localfile">需要下载的文件</param>
        public static void DoResponseFile(HttpResponseBase resp, string localfile)
        {
            try
            {
                using (FileStream fs = System.IO.File.OpenRead(localfile))
                {
                    resp.ClearContent();
                    resp.ContentType = "text/plain";
                    resp.Headers.Add("Transfer-Encoding", "chunked");
                    var bts = new byte[4096];
                    using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
                    {
                        int n;
                        while ((n = br.Read(bts, 0, bts.Length)) > 0)
                        {
                            var bbss = Encoding.UTF8.GetBytes(n.ToString("x") + "\r\n");
                            resp.OutputStream.Write(bbss, 0, bbss.Length);
                            resp.OutputStream.Write(bts, 0, n);
                            resp.OutputStream.Write(new byte[] { 13, 10 }, 0, 2);
                            resp.Flush();

                            Thread.Sleep(100);
                        }

                        var bbss2 = Encoding.UTF8.GetBytes("0\r\n\r\n");
                        resp.OutputStream.Write(bbss2, 0, bbss2.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.ClearContent();
                resp.Write(ex);
            }
            resp.End();
        }
    }
}