using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    public class SendMessage
    {
        public string openId { get; set; }
        public string context { get; set; }
        public string url { get; set; }
        public string mediaid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<NewsClass> newsMessage { get; set; }
    }

    public class SendNewsMessage
    {
        public List<string> openIds { get; set; }
        public string mediaId { get; set; }
        public string msgtype { get; set; }
    }
}
