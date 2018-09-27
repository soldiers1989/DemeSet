using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{
    /// <summary>
    /// 试题  试卷练习相关管理
    /// </summary>

    [CheckLogin(true)]
    public class PracticeController : BaseController
    {
        // GET: jjs/Practice
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 试题列表
        /// </summary>
        /// <returns></returns>

        public ActionResult ItemList(string chapterQuestionsId)
        {
            ViewBag.ChapterQuestionsId = chapterQuestionsId;
            return View();
        }

        /// <summary>
        /// 课程章节练习
        /// </summary>
        /// <param name="chapterId">章节编号</param>
        ///   /// <param name="chapterQuestionsId">练习编号</param>
        /// <returns></returns>
        public ActionResult ChapterPractice(string chapterId, string chapterQuestionsId)
        {
            ViewBag.ChapterId = chapterId;
            ViewBag.ChapterQuestionsId = chapterQuestionsId;
            return View();
        }
    }
}