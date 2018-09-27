using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.WebSite.Models.Entity
{
    public class TemplateMessage
    {
        /// <summary>
        /// 普通用户openid
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 模板消息ID
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 模板颜色
        /// </summary>
        public string topcolor { get; set; }

        public TemplateData data { get; set; }
    }

    //public class TemplateData
    //{
    //    public string code { get; set; }
    //    public string title { get; set; }
    //    public string statusremark { get; set; }
    //    public string date { get; set; }
    //    public string limitdate { get; set; }
    //    public string remark { get; set; }
    //}
    public class TemplateData
    {
        public TpData first { get; set; }
        public TpData keyword1 { get; set; }
        public TpData keyword2 { get; set; }
        public TpData keyword3 { get; set; }
        public TpData keyword4 { get; set; }
        public TpData keyword5 { get; set; }
        public TpData remark { get; set; }
    }

    public class TpData
    {
        public string value { get; set; }
        public string color { get; set; }
    }
}
