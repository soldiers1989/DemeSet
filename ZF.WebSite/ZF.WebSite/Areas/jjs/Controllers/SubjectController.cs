using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    //[CheckLogin(true)]
    public class SubjectController : BaseController
    {
        // GET: jjs/Subject

        /// <summary>
        /// 课程选择
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 科目分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Class()
        {
            return View();
        }

        /// <summary>
        /// 课程更多
        /// </summary>
        /// <returns></returns>
        public ActionResult Course()
        {
            return View();
        }

        /// <summary>
        /// 题库更多
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseTk()
        {
            return View();
        }
    }
}