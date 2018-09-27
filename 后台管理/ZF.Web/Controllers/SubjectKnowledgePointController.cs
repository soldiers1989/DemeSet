using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class SubjectKnowledgePointController : BaseController
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
        /// <param name="id">主键编号</param>
        /// <param name="parentId">父级编号</param>
        /// <param name="parentName">父级名称</param>
        /// <param name="subjectId">科目编号</param>
        /// <returns></returns>
        public ActionResult AddOrEdit(string id,string parentId,string parentName,string subjectId)
        {
            ViewBag.SubjectId = subjectId;
            ViewBag.ParentId = parentId;
            ViewBag.ParentName = parentName;
            ViewBag.Id = id ?? "";
            return PartialView();
        }
    }
}