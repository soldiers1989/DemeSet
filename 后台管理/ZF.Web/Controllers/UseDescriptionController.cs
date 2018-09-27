using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class UseDescriptionController : Controller
    {
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return PartialView();
        }

        /// <summary>
        /// 新增修改页面
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