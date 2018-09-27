using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class CourseAppraiseController : Controller
    {
        // GET: CourseAppraise
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reply(string Id ) {
            ViewBag.Id = Id;
            return View( );
        }
    }
}