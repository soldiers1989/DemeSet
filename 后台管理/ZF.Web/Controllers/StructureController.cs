using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class StructureController : Controller
    {
        // GET: Structure
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit(string id)
        {
            if (id.Contains("#"))
            {
                ViewBag.Id = "";
                ViewBag.subjectId = id.Split('#')[1];
            }
            else
            {
                ViewBag.Id = id ;
                ViewBag.subjectId = "";
            }
            
            return PartialView();
        }
    }
}