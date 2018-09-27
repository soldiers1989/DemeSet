using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.WebApiAppService.PublicInFormationModule;
using ZF.Application.WebApiDto.PublicInformationModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PublicInfoController : ApiController
    {
        private readonly PublicInformationAppService _publicInformationAppService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicInformationAppService"></param>
        public PublicInfoController( PublicInformationAppService publicInformationAppService ) {
            _publicInformationAppService = publicInformationAppService;
        }


        /// <summary>
        /// 获取联系我们，注册协议等信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]  
        public PublicInformationModelOutput GetContentInfo( PublicInformationModelInput input ) {
            return _publicInformationAppService.GetContentInfo( input);
        }
    }
}
