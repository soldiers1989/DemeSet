using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CartModule;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Infrastructure.AppWikiService;

namespace ZF.API.Controllers
{
    /// <summary>
    /// app购物车与订单服务类
    /// </summary>
    public class AppCartController : BaseApiController
    {
        private readonly CartModuleAppService cartModuleAppService;
        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="_cartModuleAppService"></param>
        public AppCartController(CartModuleAppService _cartModuleAppService)
        {
            cartModuleAppService = _cartModuleAppService;
        }
        /// <summary>
        /// app微信支付-获取签名
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public MessagesOutPut GetWikiPaySign(SheetModelInput input)
        {
            return cartModuleAppService.GetWikiPaySign(input);
        }

        /// <summary>
        /// 获得appid以及AppSecret
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [AllowAnonymous]
        public MessagesOutPut GetWikiLoginConfig()
        {
            return cartModuleAppService.GetWikiLoginConfig();
        }
    }
}
