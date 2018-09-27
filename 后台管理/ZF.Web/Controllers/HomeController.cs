using System;
using System.Web.Mvc;
using ZF.Infrastructure.AlipayService;

namespace ZF.Web.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.UserName = UserObject.UserName;
            return View();
        }

        public JsonResult Test()
        {
            return Json(new { name = 1 });
        }

        public JsonResult Logout()
        {
            /*****写入cookie******/
            var httpCookie = Response.Cookies["UserInfo"];
            if (httpCookie != null)
            {
                httpCookie.Value = "";
                httpCookie.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(httpCookie);
            }
            return Json(new { Success = true, data = "", Message = "退出成功!" });
        }
    }
}