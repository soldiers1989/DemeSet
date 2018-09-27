using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;

namespace ZF.Web.Controllers
{
    public class DataTypeController : BaseController
    {
        // GET: DataType
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddOrEdit(string id)
        {
            ViewBag.Id = id ?? "";
            return PartialView();
        }

    }
}