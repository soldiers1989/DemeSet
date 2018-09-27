using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    public class WeiXinUploadMediaResult : WeiXinReturnMessag
    {
        public string type { get; set; }
        public string media_id { get; set; }
        public long created_at { get; set; }
    }
}
