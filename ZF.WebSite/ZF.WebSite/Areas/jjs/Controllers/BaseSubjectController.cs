using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    [CheckLogin(true)]
    public class BaseSubjectController : BaseController
    {
        // GET: jjs/BaseSubject
        public ActionResult Index()
        {
            return View();
        }
    }
}