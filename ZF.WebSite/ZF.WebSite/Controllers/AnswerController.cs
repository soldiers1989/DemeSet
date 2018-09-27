using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class AnswerController : Controller
    {
        /// <summary>
        /// 返回试卷作答视图
        /// </summary>
        /// <param name="paperId">试卷编码</param>
        /// <param name="paperRecordsId">作答记录编码</param>
        /// <returns></returns>
        public ActionResult Index(string paperId,string paperRecordsId)
        {
            ViewBag.PaperId = string.IsNullOrWhiteSpace(paperId) ? "77d5520b-0fab-44e8-8cca-e81190aeb65b" : paperId;
            ViewBag.PaperRecordsId = string.IsNullOrWhiteSpace(paperRecordsId) ? "77d5520b-0fab-44e8-8cca-e81190aeb65b" : paperRecordsId;
            return View();
        }



        /// <summary>
        /// 返回试卷作答查看试题
        /// </summary>
        /// <param name="paperId">试卷编码</param>
        /// <param name="paperRecordsId">作答记录编码</param>
        /// <returns></returns>
        public ActionResult IndexView(string paperId, string paperRecordsId)
        {
            ViewBag.PaperId = string.IsNullOrWhiteSpace(paperId) ? "77d5520b-0fab-44e8-8cca-e81190aeb65b" : paperId;
            ViewBag.PaperRecordsId = string.IsNullOrWhiteSpace(paperRecordsId) ? "77d5520b-0fab-44e8-8cca-e81190aeb65b" : paperRecordsId;
            return View();
        }

        /// <summary>
        /// 试卷提交成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Success()
        {
            return View();
        }

        /// <summary>
        /// 课程章节练习
        /// </summary>
        /// <param name="chapterId">章节编号</param>
        ///   /// <param name="chapterQuestionsId">练习编号</param>
        /// <returns></returns>
        public ActionResult ChapterPractice(string chapterId,string chapterQuestionsId)
        {
            ViewBag.ChapterId = chapterId;
            ViewBag.ChapterQuestionsId = chapterQuestionsId;
            return View();
        }


        /// <summary>
        /// 课程章节练习查看
        /// </summary>
        /// <param name="chapterQuestionsId">章节编号</param>
        /// <returns></returns>
        public ActionResult ChapterPracticeView(string chapterQuestionsId)
        {
            ViewBag.ChapterQuestionsId = chapterQuestionsId;
            return View();
        }

        /// <summary>
        /// 课程章节练习记录查看
        /// </summary>
        /// <param name="chapterQuestionsId">章节编号</param>
        /// <returns></returns>
        public ActionResult ChapterPracticeList(string chapterQuestionsId)
        {
            ViewBag.ChapterQuestionsId = chapterQuestionsId;
            return PartialView();
        }
    }
}