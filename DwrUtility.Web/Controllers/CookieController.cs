using System.Web.Mvc;

namespace DwrUtility.Web.Controllers
{
    public class CookieController : Controller
    {
        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestCookie()
        {
            var value = Request.Cookies["Hm_lvt_d165b0df9d8b576128f53e461359a530"]?.Value;
            return Content(string.IsNullOrWhiteSpace(value) ? "没有值" : value);
        }
    }
}