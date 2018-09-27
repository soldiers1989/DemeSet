using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class SubjectBookController : Controller
    {
        // GET: SubjectBook
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit(string Id,string SubjectId)
        {
            ViewBag.Id = Id;
            ViewBag.SubjectId = SubjectId;
            return View();
        }
    }
}