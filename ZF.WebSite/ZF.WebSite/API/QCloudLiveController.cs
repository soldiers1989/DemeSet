using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ZF.WebSite.API
{
    public class QCloudLiveController : ApiController
    {
        private readonly string QCloudApiKey = ConfigurationManager.AppSettings["QCloudLiveApiKey"];


        [HttpPost]
        public bool validApiKey( string data) {

            return true;
        }
    }
}
