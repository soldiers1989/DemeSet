using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    [CheckLogin(true)]
    public class MyErrorSubjectController : BaseController
    {
        // GET: jjs/MyErrorSubject
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 试题纠错
        /// </summary>
        /// <returns></returns>
        public ActionResult Checker( ) {
            return View( );
        }
        /// <summary>
        /// 再次练习
        /// </summary>
        /// <returns></returns>
        public ActionResult Repractice( ) {
            return View( );
        }
        /// <summary>
        /// 练习结果
        /// </summary>
        /// <param name="practiceNo"></param>
        /// <returns></returns>
        public ActionResult PracticeResult( string practiceNo,int total,int correctNum ) {
            ViewBag.PracticeNo = practiceNo;
            ViewBag.Total = total;
            ViewBag.CorrectNum = correctNum;
            return View( );
        }

        public ActionResult PracticeDetail( string practiceNo ) {
            ViewBag.PracticeNo = practiceNo;
            return View( );
        }
    }
}