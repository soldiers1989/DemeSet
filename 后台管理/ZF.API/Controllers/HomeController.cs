using System.Web.Mvc;
using ZF.Dapper.Db.Repository;

namespace ZF.API.Controllers
{
    public class HomeController : BaseApiController
    {
        /// <summary>
        /// /根据用户票据查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RegisterUserOutputDto GetUserInfoByTicket()
        {
            return UserObject;
        }
    }
}
