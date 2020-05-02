using Newtonsoft.Json;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DwrUtility.Web.Controllers
{
    public class ProductController : Controller
    {
        /// <summary>
        /// /Product/PostJson
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostJson(RequestModel request)
        {
            return Content(request.Guid.ToString());
        }

        /// <summary>
        /// /Product/PostPayload
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostPayload()
        {
            var content = Request.GetPayloadData();
            var m = JsonConvert.DeserializeObject<RequestModel>(content);
            return Content(m.Guid.ToString());
        }

        /// <summary>
        /// /Product/PostForm
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostForm()
        {
            var guid = Request.Form["Guid"];
            var name = Request.Form["Name"];

            return Content($"Guid：{guid}；Name：{name}");
        }

        /// <summary>
        /// /Product/PostFile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostFile()
        {
            var files = Request.Files;
            var content = $"{Request.Form["Name"]}；{Request.Form["Content"]}";

            var stream = Request.InputStream;
            stream.Position = 0;
            var bt = new byte[stream.Length];
            stream.Read(bt, 0, bt.Length);
            var str = Encoding.UTF8.GetString(bt);
            
            return Content($"文件数：{files.Count}；内容：{content}；内容2：{str}");
        }
    }

    public class RequestModel
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }
    }
}