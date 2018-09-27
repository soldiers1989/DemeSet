using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Infrastructure.AlipayService;

namespace ZF.Web.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AddOrEdit()
        {
            var model1 = Aliyun.GetSecurityToken();
            ViewBag.AccessKeyId = model1.AccessKeyId;
            ViewBag.AccessKeySecret = model1.AccessKeySecret;
            ViewBag.SecurityToken = model1.SecurityToken;
            return View();
        }

        public ActionResult Play(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult Choose()
        {
            return View();
        }
    }
}