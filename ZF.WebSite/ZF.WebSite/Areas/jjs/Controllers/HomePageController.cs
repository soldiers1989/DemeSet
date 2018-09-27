using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{
    //[CheckLogin( false )]
    public class HomePageController : BaseController
    {

        // GET: jjs/HomePage
        public ActionResult Index()
        {
            return View();
        }
    }
}