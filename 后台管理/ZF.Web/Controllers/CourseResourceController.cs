using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class CourseResourceController : Controller
    {
        // GET: CourseResource
        public ActionResult Index(string courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        public ActionResult AddOrEdit( string courseId,string id ) {
            ViewBag.CourseId = courseId;
            ViewBag.Id = id??"";
            return PartialView( );
        }
    }
}