using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class SubjectBigQuestionController : BaseController
    {
        /// <summary>
        /// 列表页面视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return PartialView();
        }

        /// <summary>
        /// 新增or修改视图
        /// </summary>
        /// <param name="id"></param>
        /// <param name="subjectId">科目编号</param>
        /// <param name="knowledgePointId">知识点编号</param>
        /// <returns></returns>
        public ActionResult AddOrEdit(string id,string subjectId,string knowledgePointId)
        {
            ViewBag.Id = id ?? "";
            ViewBag.SubjectId = subjectId;
            ViewBag.KnowledgePointId = knowledgePointId;
            return PartialView();
        }

        /// <summary>
        /// 错题反馈
        /// </summary>
        /// <returns></returns>
        public ActionResult ErrorFeedback( ) {
            return PartialView( );
        }
    }
}