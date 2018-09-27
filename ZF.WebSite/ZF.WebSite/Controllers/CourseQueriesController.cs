using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class CourseQueriesController : BaseController
    {
        // GET: CourseQueries
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexQuery()
        {
            return View();
        }
    }
}