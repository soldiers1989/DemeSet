using System.Configuration;

namespace ZF.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountController : BaseController
    {
        /// <summary>
        /// 退出
        /// </summary>
        public void Logout()
        {
           // CurrentUser.Logout();
            Response.Redirect(ConfigurationManager.AppSettings["loginUrl"]);
        }
    }
}