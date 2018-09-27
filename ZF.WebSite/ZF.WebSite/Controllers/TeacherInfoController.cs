using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class TeacherInfoController : Controller
    {
        // GET: TeacherInfo
        public ActionResult Index(string teacherid)
        {
            ViewBag.TeacherId = teacherid;
            return View();
        }
    }
}