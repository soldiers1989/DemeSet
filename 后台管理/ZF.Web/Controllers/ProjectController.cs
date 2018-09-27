using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class ProjectController : BaseController
    {

        /// <summary>
        /// 列表页面视图
        /// </summary>
        /// <returns></returns>
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
            ViewBag.Id = id ?? "";
            return PartialView();
        }
    }
}