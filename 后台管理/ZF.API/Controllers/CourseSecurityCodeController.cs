using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.Dto;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 课程防伪码控制器
    /// </summary>

    [AllowAnonymous]
    public class CourseSecurityCodeController : ApiController
    {
        private readonly Application.WebApiAppService.CourseSecurityCodeModule.CourseSecurityCodeAppService _courseSecurityCodeAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="courseSecurityCodeAppService"></param>
        public CourseSecurityCodeController(Application.WebApiAppService.CourseSecurityCodeModule.CourseSecurityCodeAppService courseSecurityCodeAppService)
        {
            _courseSecurityCodeAppService = courseSecurityCodeAppService;
        }

        /// <summary>
        /// 验证防伪码是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public MessagesOutPut VerifySecurityCode(CourseSecurityCodeListInput input)
        {
            return _courseSecurityCodeAppService.VerifySecurityCode(input);
        }
    }
}
