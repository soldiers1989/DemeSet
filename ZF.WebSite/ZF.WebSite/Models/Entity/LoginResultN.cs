using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZF.WebSite.Models.Entity;

namespace ZF.WebSite.Models.Entity
{
    /// <summary>
    /// LoginResultN 的摘要说明
    /// </summary>
    public class LoginResultN
    {
        public BaseResp base_resp { get; set; }
        public string redirect_url { get; set; }
    }
}