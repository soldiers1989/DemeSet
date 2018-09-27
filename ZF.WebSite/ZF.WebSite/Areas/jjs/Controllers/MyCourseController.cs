using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    [CheckLogin(true)]
    public class MyCourseController : BaseController
    {
        // GET: jjs/MyCourse
        public ActionResult Index(string SecurityCode)
        {
            ViewBag.SecurityCode = SecurityCode;
            return View();
        }


        /// <summary>
        /// 我的题库
        /// </summary>
        /// <returns></returns>
        public ActionResult MyQuestion()
        {
            return View();
        }
    }
}