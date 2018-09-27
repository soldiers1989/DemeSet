using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class TestVueController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit( string id ,string subjectId) {
            ViewBag.Id = id ?? "";
            ViewBag.SubjectId = subjectId;
            return PartialView( );
        }

        public ActionResult CourseContent( /*string id,string SubjectName,string CourseName,string TeachersName */) {
            //ViewBag.Id = id ?? "";
            //ViewBag.SubjectName = SubjectName;
            //ViewBag.CourseName = CourseName;
            //ViewBag.TeachersName = TeachersName;
            return View( );
        }

        public ActionResult AddCourseResource( string CourseId) {
            ViewBag.CourseId = CourseId;
            return PartialView( );
        }

        public ActionResult page( ) {
            return View( );
        }
    }
}