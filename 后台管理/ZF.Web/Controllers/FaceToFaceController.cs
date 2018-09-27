using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class FaceToFaceController : Controller
    {
        // GET: FaceToFace
        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult AddOrEdit(string id, string subjectId)
        {
            ViewBag.Id = id ?? "";
            ViewBag.SubjectId = subjectId;
            return PartialView();
        }

    }
}