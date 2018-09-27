using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs.Controllers
{

    [CheckLogin(true)]
    public class ChapterExerciseController : BaseController
    {
        // GET: jjs/ChapterExercise
        public ActionResult Index()
        {
            return View();
        }
    }
}