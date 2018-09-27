using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    /// <summary>
    /// 试题分类
    /// </summary>
    public class SubjectClassController : BaseController
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
        /// <returns></returns>
        public ActionResult AddOrEdit(string id)
        {
            ViewBag.Id = id ?? "";
            return PartialView();
        }
    }
}