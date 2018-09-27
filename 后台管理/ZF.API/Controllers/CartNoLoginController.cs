using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CartModule;
using ZF.Application.WebApiDto.CartDtoModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 购物车-未登录的情况下
    /// </summary>
    public class CartNoLoginController : ApiController
    {
        private readonly CartModuleAppService cartModuleAppService;

        /// <summary>
        /// 构造函数-创建实例
        /// </summary>
        /// <param name="_cartModuleAppService"></param>
        public CartNoLoginController(CartModuleAppService _cartModuleAppService)
        {
            cartModuleAppService = _cartModuleAppService;
        }

        /// <summary>
        /// 购物车明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public List<CartModelOutput> CartList(IdInputIds input)
        {
            return cartModuleAppService.CartList(input);
        }

    }
}
