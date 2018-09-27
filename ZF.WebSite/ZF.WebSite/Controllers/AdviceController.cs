using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class AdviceController : Controller
    {
        // GET: Advice
        public ActionResult Index(string aboutType)
        {
            ViewBag.aboutType = aboutType;
            return View();
        }

        // GET: Advice
        public ActionResult Indexbak(string aboutType)
        {
            ViewBag.aboutType = aboutType;
            return View();
        }

        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>

        public ActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// 联系我们
        /// </summary>
        /// <returns></returns>

        public ActionResult ContactUs()
        {
            return View();
        }

        /// <summary>
        /// 法律条款
        /// </summary>
        /// <returns></returns>

        public ActionResult LegalProvisions()
        {
            return View();
        }

        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <returns></returns>

        public ActionResult Feedback()
        {
            return View();
        }

        /// <summary>
        /// 服务条款
        /// </summary>
        /// <returns></returns>
        public ActionResult TermsService()
        {
            return View();
        }
    }
}