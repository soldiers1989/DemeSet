using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.WxpayModule;
using ZF.Application.WebApiDto.SheetDtoModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 微信
    /// </summary>
    public class WxPayController : BaseApiController
    {
        WxpayModuleAppService _wxpayModuleAppService;

        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="wxpayModuleAppService"></param>
        public WxPayController(WxpayModuleAppService wxpayModuleAppService)
        {
            _wxpayModuleAppService = wxpayModuleAppService;
        }


        /// <summary>
        /// 根据订单信息生成微信二维码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public MessagesOutPut GetPayUrl(SheetModelInput input)
        {
            return _wxpayModuleAppService.GetPayUrl(input, UserObject.Id);
        }
    }
}
