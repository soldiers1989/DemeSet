using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CartModule;
using ZF.Application.WebApiDto.CartDtoModule;
using ZF.Core.Entity;
using ZF.Dapper.Db.Repository;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 购物车api
    /// </summary>
    public class CartController : BaseApiController
    {
        private readonly CartModuleAppService cartModuleAppService;

        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="_cartModuleAppService"></param>
        public CartController(CartModuleAppService _cartModuleAppService)
        {
            cartModuleAppService = _cartModuleAppService;
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CartModelOutput> CartInfoList()
        {
            return cartModuleAppService.CartInfoList(UserObject.Id);
        }


        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut DelCartDetail(IdInputIds input)
        {
            return cartModuleAppService.Delete(input);
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut DeleteInfo(IdInputIds input)
        {
            return cartModuleAppService.Delete(input);
        }


        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Add(OrderCartDetailInput input)
        {

            return cartModuleAppService.Add(input, UserObject.Id);
        }

        /// <summary>
        /// 未登录之前写入购物车登录后写入购物车表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public MessagesOutPut CartAdd(OrderCartDetailInput input)
        {
            return cartModuleAppService.CartAdd(input, UserObject.Id);
        }

    }
}