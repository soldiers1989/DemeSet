using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZF.WebSite.Areas.jjs.Common.WxPayAPI
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg) : base(msg)
        {

        }
    }
}