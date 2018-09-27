using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    /// <summary>
    /// 统计模块
    /// </summary>
    public class StatisticalInformationController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}