using System.Reflection;
using System.Web;
using System.Web.Http;
using log4net;
using ZF.Core.Entity;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.Json;

namespace ZF.Web.ApiControllers
{
    public class BaseController : ApiController
    {
        private ILog _logger;
        private readonly object _lockObj = new object();

        public string ApplicationRoot
        {
            get
            {
                return HttpContext.Current.Request.ApplicationPath;
            }
        }

        /// <summary>
        /// 用户类
        /// </summary>
        protected User UserObject
        {
            get
            {
                //读取cookies
                string cook = "";
                if (HttpContext.Current.Request.Cookies["UserInfo"] != null)
                {
                    var httpCookie = HttpContext.Current.Request.Cookies["UserInfo"]["User"];
                    if (httpCookie != null)
                        cook = httpCookie;
                }
                return (User)JsonHelper.jsonDes<User>(Des3Cryption.Decrypt3DES(cook));
            }
        }


        protected ILog Logger
        {
            get
            {
                if (this._logger == null)
                {
                    object lockObj = this._lockObj;
                    lock (lockObj)
                    {
                        if (this._logger == null)
                        {
                            this._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                        }
                    }
                }
                return this._logger;
            }
        }
    }
}
