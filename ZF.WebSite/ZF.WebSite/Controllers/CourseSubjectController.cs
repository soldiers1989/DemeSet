﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Controllers
{
    public class CourseSubjectController : Controller
    {
        // GET: CourseSubject
        public ActionResult SubjectCollection()
        {
            return View();
        }
    }
}