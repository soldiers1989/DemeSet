using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class ParamDetailController : Controller
    {
        // GET: ParamDetail
        public ActionResult Index(string pid)
        {
            ViewBag.Pid = pid ?? "";
            return PartialView();
        }
    }
}