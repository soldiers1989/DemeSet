using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class SubjectSmallquestionController : Controller
    {
        // GET: Smallquestion
        public ActionResult Index(string bigQuestionId)
        {
            ViewBag.BigQuestionId = bigQuestionId;
            return PartialView();
        }

        /// <summary>
        /// 新增or修改视图
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="bigQuestionId">大题编号</param>
        /// <returns></returns>
        public ActionResult AddOrEdit(string id,string bigQuestionId)
        {
            ViewBag.BigQuestionId = bigQuestionId;
            ViewBag.Id = id ?? "";
            return PartialView();
        }
    }
}