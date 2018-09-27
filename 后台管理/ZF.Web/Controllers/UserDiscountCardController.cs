using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class UserDiscountCardController : Controller
    {
        // GET: UserDiscountCard
        public ActionResult Index(string id)
        {
            ViewBag.Id = id;
            return PartialView();
        }
    }
}