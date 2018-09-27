using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    [CheckLogin(true)]
    public class MyPaperTestController : BaseController
    {
        // GET: jjs/MyPaperTest
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 进入测评
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        public ActionResult EnterPaper(string paperId, string paperRecordsId)
        {
            ViewBag.PaperId = paperId;
            ViewBag.PaperRecordsId = paperRecordsId;
            return View();
        }

        /// <summary>
        /// 考试在线测评
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        public ActionResult PaperAnalyzes(string paperId,string paperRecordsId)
        {
            ViewBag.PaperId = paperId;
            ViewBag.PaperRecordsId = paperRecordsId;
            return View();
        }

        /// <summary>
        /// 考试查看
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>

        public ActionResult PaperPractice(string paperId, string paperRecordsId)
        {
            ViewBag.PaperId = paperId;
            ViewBag.PaperRecordsId = paperRecordsId;
            return View();
        }


        /// <summary>
        /// 测评提交
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        public ActionResult EndPaper(string paperId, string paperRecordsId)
        {
            ViewBag.PaperId = paperId;
            ViewBag.PaperRecordsId = paperRecordsId;
            return View();
        }
        public ActionResult PaperTestResult( string paperId, string paperRecordsId ) {
            ViewBag.PaperId = paperId;
            ViewBag.PaperRecordsId = paperRecordsId;
            return View( );
        }

    }
}