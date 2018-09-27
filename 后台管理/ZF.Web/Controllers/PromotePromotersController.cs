using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class PromotePromotersController : Controller
    {
        // GET: PromotePromoters
        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult AddOrEdit(string id)
        {
            ViewBag.Id = id ?? "";
            return PartialView();
        }
    }
}