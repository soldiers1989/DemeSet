using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    [CheckLogin(true)]
    public class ScanningController : BaseController
    {
        // GET: jjs/Scanning
        public ActionResult Index()
        {
            return View();
        }
    }
}