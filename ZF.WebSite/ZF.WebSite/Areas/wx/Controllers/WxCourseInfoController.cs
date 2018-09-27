using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.wx.Controllers
{

    [CheckLogin(false)]
    public class WxCourseInfoController : Controller
    {
        // GET: wx/WxCourseInfo
        public ActionResult ChapterVideoPlay(string code)
        {
            return RedirectToAction("Index", "CourseInfo", new { area = "jjs", code = code });
        }
    }
}