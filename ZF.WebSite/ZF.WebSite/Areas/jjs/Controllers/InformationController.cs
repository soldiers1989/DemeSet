using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    public class InformationController : BaseController
    {
        // GET: jjs/Information
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string Id ) {
            ViewBag.Id = Id;
            return View( );
        }
    }
}