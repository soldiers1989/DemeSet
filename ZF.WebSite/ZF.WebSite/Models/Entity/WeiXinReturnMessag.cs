using System;
using System.Collections.Generic;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    [Serializable]
    public class WeiXinReturnMessag
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }

    [Serializable]
    public class WeiXinMediaReturn : WeiXinReturnMessag
    {
        public string msg_id { get; set; }
    }
}
