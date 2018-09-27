using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    [CheckLogin(true)]
    public class RecentStudyController : BaseController
    {
        // GET: jjs/RecentStudy
        public ActionResult Index()
        {
            return View();
        }
    }
}