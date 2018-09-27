using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    public class WeiXinUploadMedia
    {
        public string mediaType {get;set;}
        public byte[] fileData { get; set; }
    }
}
