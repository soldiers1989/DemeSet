using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class DanyeController : Controller
    {
        /// <summary>
        /// 联系我们  注册协议  
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string code,string name)
        {
            ViewBag.Code = code;
            ViewBag.Name = name;
            return PartialView();
        }

    }
}