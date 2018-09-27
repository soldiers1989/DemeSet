using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ZF.Web.Controllers
{
    public class CourseInfoController : Controller
    {
        // GET: CourseInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit(string id, string subjectId)
        {
            ViewBag.Id = id ?? "";
            ViewBag.SubjectId = subjectId;
            return PartialView();
        }

        public ActionResult CourseContent( /*string id,string SubjectName,string CourseName,string TeachersName */)
        {
            //ViewBag.Id = id ?? "";
            //ViewBag.SubjectName = SubjectName;
            //ViewBag.CourseName = CourseName;
            //ViewBag.TeachersName = TeachersName;
            return View();
        }

        public ActionResult AddCourseResource(string CourseId)
        {
            ViewBag.CourseId = CourseId;
            return PartialView();
        }

        public ActionResult EditContent(string courseid)
        {
            ViewBag.CourseId = courseid;
            return PartialView();
        }

        public ActionResult CourseDimensionalCodeInfo(CourseDimensional courseDimensional)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(courseDimensional);//序列化
            ViewBag.jsonData = jsonData;
            return PartialView();
        }

        /// <summary>
        /// 防伪码使用情况
        /// </summary>
        /// <returns></returns>

        // GET: CourseInfo
        public ActionResult SecurityCodeUsage()
        {
            return View();
        }

        /// <summary>
        /// 课程防伪码统计情况
        /// </summary>
        /// <returns></returns>

        // GET: CourseInfo
        public ActionResult SecurityCodeStatistics()
        {
            return View();
        }
    }

    [Serializable]
    public class CourseDimensional
    {
        public string Id { get; set; }

        /// <summary>
        /// 生成二维码数量
        /// </summary>
        public int CourseWareCount { get; set; }

        /// <summary>
        /// 生成二维码方式
        /// </summary>
        public int EmailNotes { get; set; }

    }
}