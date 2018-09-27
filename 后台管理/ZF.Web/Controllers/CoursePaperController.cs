using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class CoursePaperController : Controller
    {
        // GET: CoursePaper
        public ActionResult Index( string courseId)
        {
            ViewBag.CourseId = courseId;
            return PartialView( );
        }

        public ActionResult AddOrEdit( string id )
        {
            ViewBag.CourseId = id ?? "";
            return PartialView( );
        }
    }
}