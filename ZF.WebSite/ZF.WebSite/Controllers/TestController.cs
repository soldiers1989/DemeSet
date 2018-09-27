using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CoustomComponent( ) {
            return View( );
        }

        public ActionResult Directive( ) {
            return View( );
        }

        public ActionResult VueRouter( ) {
            return View( );
        }

    }
}