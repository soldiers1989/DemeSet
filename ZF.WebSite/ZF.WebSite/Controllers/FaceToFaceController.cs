using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class FaceToFaceController : BaseController
    {
        // GET: FaceToFace

        /// <summary>
        /// 面授课视图
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ActionResult Index(string courseId)
        {
            ViewBag.CourseId = courseId ?? "";
            var model = GetTitle(courseId, 0);
            if (model != null)
            {
                ViewBag.Title = model.Title;
                ViewBag.Description = model.Description;
                ViewBag.KeyWord = model.KeyWord;
            }
            return View();
        }

    }
}