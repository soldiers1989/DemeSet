using System.Web;

namespace ZF.Web.Ashx
{
    /// <summary>
    /// ThumbImage 的摘要说明
    /// </summary>
    public class GetFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // new Topevery.FMP.ObjectModel.Web.Handlers.GetFileHandler().ProcessRequest(context);
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