using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class CourseVideoController : Controller
    {
        // GET: CourseVideo
        public ActionResult Index(string courseId)
        {
            ViewBag.CourseId = courseId;
            return PartialView();
        }

        public ActionResult AddOrEdit(  string chapterId ,string id) {
            ViewBag.Id = id ?? "";
            ViewBag.ChapterId = chapterId;
            return PartialView( );
        }


        public ActionResult Choose()
        {
            return PartialView();
        }

        public ActionResult TasteLongTimeView(string chapterId)
        {
            ViewBag.ChapterId = chapterId;
            return PartialView();
        }
    }
}