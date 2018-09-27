using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using ZF.WebSite.Models;
using ZF.WebSite.Models.Entity;

namespace ZF.WebSite.API
{
    public class WeiXinUserController : ApiController
    {

    }

    public class ResultMsg
    {
        public bool isSuccess { get; set; }
        public string errorMsg { get; set; }
        public dynamic data { get; set; }
    }
}