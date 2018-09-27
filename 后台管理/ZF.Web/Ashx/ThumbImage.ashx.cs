using System.Web;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.QiniuYun;

namespace ZF.Web.Ashx
{
    /// <summary>
    /// ThumbImage 的摘要说明
    /// </summary>
    public class ThumbImage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var FID = context.Request.QueryString["FID"];
            var url = AliyunFileUpdata.DownloadUrl(FID);
            context.Response.WriteFile(url);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}