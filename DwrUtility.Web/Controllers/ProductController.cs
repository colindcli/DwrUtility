using Newtonsoft.Json;
using System;
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
    }

    public class RequestModel
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }
    }
}