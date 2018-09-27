using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class CourseChapterController : Controller
    {
        public ActionResult Index(string courseId ) {
            ViewBag.CourseId = courseId;
            return PartialView( );
        }

        // GET: CourseChapter
        public ActionResult AddCourseChapter(string courseId,string parentId)
        {
            ViewBag.CourseId = courseId;
            ViewBag.ParentId = parentId;
            return PartialView();
        }

        public ActionResult EditCourseChapter( string chapterid,string courseid ) {
            ViewBag.ChapterId = chapterid;
            ViewBag.CourseId = courseid;
            return PartialView( );
        }
    }
}