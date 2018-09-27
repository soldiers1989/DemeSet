using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.WebSite.Areas.jjs.Models;

namespace ZF.WebSite.Areas.jjs.Controllers
{
    public class WxController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Code = Request.QueryString["code"];
            return View();
        }

        public ActionResult Index1()
        {
            return View();
        }
    }
}