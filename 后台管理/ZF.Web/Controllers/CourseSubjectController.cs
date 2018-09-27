using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class CourseSubjectController : Controller
    {
        // GET: CourseSubject
        public ActionResult Index(string courseId,string courseName,string subjectId,string projectId)
        {
            ViewBag.CourseId = courseId;
            ViewBag.CourseName = courseName;
            ViewBag.SubjectId = subjectId;
            ViewBag.ProjectId = projectId;
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">项目</param>
        /// <param name="subjectId">科目</param>
        /// <param name="courseId">课程</param>
        /// <param name="chapterId">视频节点Id</param>
        /// <returns></returns>
        public ActionResult Add(string projectId,string subjectId, string courseId, string chapterId ) {
            ViewBag.ProjectId = projectId;
            ViewBag.SubjectId = subjectId;
            ViewBag.CourseId = courseId;
            ViewBag.ChapterId = chapterId;
            return PartialView( );
        }
    }
}