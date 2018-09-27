using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Application.Dto;

namespace ZF.Web.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return PartialView();
        }


        public ActionResult AddOrEdit( string id ) {
            ViewBag.Id = id ?? "";
            return PartialView( );
        }
    }
}