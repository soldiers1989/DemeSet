﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Controllers
{
    public class PurchaseDiscountController : Controller
    {
        // GET: PurchaseDiscount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit( string id) {
            ViewBag.Id = id;
            return PartialView( );
        }
    }
}