using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{
    [CheckLogin(true)]
    public class CourseInfoController : BaseController
    {
        // GET: jjs/CourseInfo
        public ActionResult Index(string id, string code,int? courseType)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.CourseId = GetCourseId(code);
            }
            else
            {
                ViewBag.CourseId = id;
            }
            ViewBag.code = code;
            if (courseType == 1)
            {
                Response.Redirect("/jjs/CourseInfo/CoursePackInfo?courseId=" + id);
                return View();
            }
            return View();
        }

        /// <summary>
        /// 打包课程页面
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ActionResult CoursePackInfo(string courseId)
        {
            ViewBag.CourseId = courseId ?? "";
            return View();
        }

        public ActionResult CoursePackContent()
        {
            return View();
        }


        public ActionResult CourseLsit()
        {
            return View();
        }
        


        public ActionResult CourseChapter(string code)
        {
            return View();
        }

        public ActionResult CourseContent()
        {
            return View();
        }

        /// <summary>
        /// 模拟试卷
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseTestPaper()
        {
            return View();
        }

        /// <summary>
        /// 试题精炼
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseLLztPaper()
        {
            return View();
        }

        

        public ActionResult CourseAppraise()
        {
            return View();
        }

        public ActionResult VideoPlay (string courseId,string videoId ,string chapterId){
            ViewBag.CourseId = courseId;
            ViewBag.VideoId = videoId;
            ViewBag.ChapterId = chapterId;
            return View( );
        }

        public ActionResult TestPaperInfo( string paperId,string paperRecordsId ) {
            ViewBag.PaperId = paperId;
            ViewBag.PaperRecordsId = paperRecordsId;
            return View( );
        }

        public ActionResult Advertising()
        {
            return View();
        }
    }
}