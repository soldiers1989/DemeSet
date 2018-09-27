using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class StructureDetailController : Controller
    {
        // GET: StructureDetail
        public ActionResult Index(string pid)
        {
            ViewBag.Pid = pid ?? "";
            return View();
        }
    }
}