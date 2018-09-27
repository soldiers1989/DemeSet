using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{
    [CheckLogin(true)]
    public class ValueAddedController : BaseController
    {
        // GET: jjs/ValueAdded
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Message()
        {
            return View();
        }

        /// <summary>
        /// 投诉
        /// </summary>
        /// <returns></returns>
        public ActionResult Complaints(string code)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}