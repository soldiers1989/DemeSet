using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class RoleController : BaseController
    {
        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit(string id)
        {
            ViewBag.Id = id ?? "";
            return PartialView();
        }

        /// <summary>
        /// 分配资源
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult AllocatingResources(string roleId)
        {
            ViewBag.RoleId = roleId ?? "";
            return PartialView();
        }


        /// <summary>
        /// 人员管理
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult PersonnelManagement(string roleId)
        {
            ViewBag.RoleId = roleId ?? "";
            return PartialView();
        }

        /// <summary>
        /// 人员维护
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult RoleUserAddOrEdit(string roleId)
        {
            ViewBag.RoleId = roleId ?? "";
            return PartialView();
        }
        

    }
}