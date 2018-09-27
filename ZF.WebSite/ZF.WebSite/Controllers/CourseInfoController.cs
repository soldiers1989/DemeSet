using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZF.Infrastructure.Json;
using ZF.WebSite.Models;

namespace ZF.WebSite.Controllers
{

    public class CourseInfoController : BaseController
    {
        /// <summary>
        /// 课程页面
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="courseType"></param>
        /// <returns></returns>
        public ActionResult CourseInfoIfarm(string courseId, int? courseType)
        {
            var model = GetTitle(courseId, 0);
            if (model != null)
            {
                ViewBag.Title = string.IsNullOrEmpty(model.Title) ? "课程详情" : model.Title;
                ViewBag.Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description;
                ViewBag.KeyWord = string.IsNullOrEmpty(model.KeyWord) ? "" : model.KeyWord;
            }
            ViewBag.CourseId = courseId ?? "";
            if (courseType == 1)
            {
                Response.Redirect("/CourseInfo/CoursePackInfo?courseId=" + courseId);
                return View();
            }
            if (courseType == 2)
            {
                Response.Redirect("/FaceToFace/Index?courseId=" + courseId);
            }
            return View();
        }

        /// <summary>
        /// 课程页面
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="courseType"></param>
        /// <returns></returns>
        public ActionResult CourseInfoPack(string courseId, int? courseType)
        {
            var model = GetTitle(courseId, 0);
            if (model != null)
            {
                ViewBag.Title = string.IsNullOrEmpty(model.Title) ? "课程详情" : model.Title;
                ViewBag.Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description;
                ViewBag.KeyWord = string.IsNullOrEmpty(model.KeyWord) ? "" : model.KeyWord;
            }
            ViewBag.CourseId = courseId ?? "";
            if (courseType == 1)
            {
                Response.Redirect("/CourseInfo/CoursePackInfo?courseId=" + courseId);
                return View();
            }
            if (courseType == 2)
            {
                Response.Redirect("/FaceToFace/Index?courseId=" + courseId);
            }
            return View();
        }

        /// <summary>
        /// 课程页面
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="courseType"></param>
        /// <returns></returns>
        public ActionResult CourseInfo(string courseId, int? courseType)
        {
            var model = GetTitle(courseId, 0);
            if (model != null)
            {
                ViewBag.Title = model.Title;
                ViewBag.Description = model.Description;
                ViewBag.KeyWord = model.KeyWord;
            }
            ViewBag.CourseId = courseId ?? "";
            if (courseType == 1)
            {
                Response.Redirect("/CourseInfo/CoursePackInfo?courseId=" + courseId);
                return View();
            }
            if (courseType == 2)
            {
                Response.Redirect("/FaceToFace/Index?courseId=" + courseId);
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
            var model = GetTitle(courseId, 0);
            if (model != null)
            {
                ViewBag.Title = model.Title;
                ViewBag.Description = model.Description;
                ViewBag.KeyWord = model.KeyWord;
            }
            ViewBag.CourseId = courseId ?? "";
            return View();
        }


        public ActionResult MyStudy()
        {
            return View();
        }

        /// <summary>
        /// 课程搜索
        /// </summary>
        /// <param name="searchinput"></param>
        /// <returns></returns>

        public ActionResult SearchCourse(string searchinput)
        {
            return View();
        }

        public ActionResult ChapterVideoPlay(string courseId, string chapterId, string chapterName, string videoId, string code)
        {
            ViewBag.CourseId = courseId;
            ViewBag.ChapterId = chapterId;
            ViewBag.ChapterName = chapterName;
            ViewBag.VideoId = videoId;
            ViewBag.Code = code ?? "";
            return View();
        }

        /// <summary>
        /// 试卷练习记录
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        public ActionResult ChapterPracticeList(string paperId)
        {
            ViewBag.PaperId = paperId;
            return View();
        }

        /// <summary>
        /// 视频播放
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoPlay()
        {
            return View();
        }

    }
}