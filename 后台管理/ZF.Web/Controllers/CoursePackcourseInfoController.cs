using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class CoursePackcourseInfoController : Controller
    {
        // GET: CoursePackcourseInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit( string id ) {
            ViewBag.Id = id ?? "";
            return PartialView( );
        }

        public ActionResult CourseDetail( string packCourseId ) {
            ViewBag.PackCourseId = packCourseId;
            return PartialView ();
        }

        public ActionResult AddSubCourse( string packCourseId ) {
            ViewBag.PackCourseId = packCourseId;
            return PartialView( );
        }

        public ActionResult EditSubCourse( string id ) {
            ViewBag.Id = id;
            return PartialView( );
        }
    }
}