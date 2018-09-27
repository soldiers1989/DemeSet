using System.Web.Mvc;
using log4net;
using ZF.Core.Entity;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.Json;
using System.Collections.Generic;

namespace ZF.Web.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController : Controller
    {


        /// <summary>
        /// 应用程序根目录
        /// </summary>
        public string ApplicationRoot
        {
            get { return System.Web.HttpContext.Current.Request.ApplicationPath; }
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
                if (System.Web.HttpContext.Current.Request.Cookies["UserInfo"] != null)
                {
                    var httpCookie = System.Web.HttpContext.Current.Request.Cookies["UserInfo"]["User"];
                    if (httpCookie != null)
                        cook = httpCookie;
                }
                return (User)JsonHelper.jsonDes<User>(Des3Cryption.Decrypt3DES(cook));
            }
        }


        /// <summary>
        /// 日志帮助类
        /// </summary>
        private ILog _logger;
        private readonly object _lockObj = new object();
        protected ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    lock (_lockObj)
                    {
                        if (_logger == null)
                        {
                            _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); ;
                        }
                    }
                }
                return _logger;
            }
        }

    }
}