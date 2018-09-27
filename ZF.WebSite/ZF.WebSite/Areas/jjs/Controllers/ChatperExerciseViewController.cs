using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    [CheckLogin(true)]
    public class ChatperExerciseViewController : BaseController
    {
        // GET: jjs/ChatperExerciseView
        public ActionResult Index( string chapterId, string chapterQuestionsId )
        {
            ViewBag.ChapterId = chapterId;
            ViewBag.ChapterQuestionsId = chapterQuestionsId;
            return View();
        }
    }
}