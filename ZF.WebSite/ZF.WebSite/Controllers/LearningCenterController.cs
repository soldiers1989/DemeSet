using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class LearningCenterController : Controller
    {
        // GET: LearningCenter
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 增值服务
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCourse()
        {
            return View();
        }

        /// <summary>
        /// 机考仿真练习
        /// </summary>
        /// <returns></returns>
        public ActionResult MyQuestion()
        {
            return View();
        }


        /// <summary>
        /// 收藏课程
        /// </summary>
        /// <returns></returns>
        public ActionResult CollectionCourse()
        {
            return View();
        }

        /// <summary>
        /// 收藏试题
        /// </summary>
        /// <returns></returns>
        public ActionResult CollectionItem()
        {
            return View();
        }

        /// <summary>
        /// 我的错题
        /// </summary>
        /// <returns></returns>
        public ActionResult MyErrors()
        {
            return View();
        }

        /// <summary>
        /// 练习记录
        /// </summary>
        /// <returns></returns>
        public ActionResult PracticeRecords()
        {
            return View();
        }

        /// <summary>
        /// 测评记录
        /// </summary>
        /// <returns></returns>
        public ActionResult AssessmentRecords()
        {
            return View();
        }

        /// <summary>
        /// 我的足迹
        /// </summary>
        /// <returns></returns>
        public ActionResult MyFootprint()
        {
            return View();
        }

        /// <summary>
        /// 最近学习
        /// </summary>
        /// <returns></returns>
        public ActionResult RecentStudy()
        {
            return View();
        }

        public ActionResult QuesetionRelateVideo(string videoId)
        {
            ViewBag.videoId = videoId;
            return View();
        }
    }
}