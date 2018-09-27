using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class PaperGroupController : Controller
    {
        // GET: PaperGroupRelation
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增or修改视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddOrEdit(string id)
        {
            if (id.Contains("#"))
            {
                ViewBag.Id = "";
                ViewBag.subjectId = id.Split('#')[1];
            }
            else
            {
                ViewBag.Id = id;
                ViewBag.subjectId = "";
            }

            return PartialView();
        }


        /// <summary>
        /// 人员管理
        /// </summary>
        /// <param name="paperGroupId"></param>
        /// <returns></returns>
        public ActionResult PersonnelManagement(string paperGroupId)
        {
            ViewBag.PaperGroupId = paperGroupId ?? "";
            return PartialView();
        }

        /// <summary>
        /// 人员维护
        /// </summary>
        /// <param name="paperGroupId"></param>
        /// <returns></returns>
        public ActionResult PaperGroupAddOrEdit(string paperGroupId)
        {
            ViewBag.PaperGroupId = paperGroupId ?? "";
            return PartialView();
        }
    }
}