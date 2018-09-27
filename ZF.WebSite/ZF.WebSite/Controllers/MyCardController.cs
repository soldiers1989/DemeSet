using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class MyCardController : Controller
    {
        // GET: MyCard
        public ActionResult Index()
        {
            return View();
        }
    }
}