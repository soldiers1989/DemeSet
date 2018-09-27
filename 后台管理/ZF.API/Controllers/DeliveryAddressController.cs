using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.WebApiAppService;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.DeliveryAddressModule;
using ZF.Application.WebApiAppService.DeliveryAddressService;
using ZF.Core.Entity;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 我的收货地址
    /// </summary>
    public class DeliveryAddressController : BaseApiController
    {
        private readonly DeliveryAddressAppService deliveryAddressAppService;

        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="_deliveryAddressAppService"></param>
        public DeliveryAddressController(DeliveryAddressAppService _deliveryAddressAppService)
        {
            deliveryAddressAppService = _deliveryAddressAppService;
        }

        /// <summary>
        /// 新增实体编码
        /// </summary>
        [System.Web.Http.HttpPost]
        public MessagesOutPut AddOrEdit(DeliveryAddressModelInput input)
        {
            input.UserId = UserObject.Id;
            return deliveryAddressAppService.AddOrEdit(input);
        }

        /// <summary>
        /// 查询用户所有收件地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<DeliveryAddressModuleOutPut> GetList()
        {
            DeliveryAddressModelInput input = new DeliveryAddressModelInput()
            {
                UserId = UserObject.Id
            };
            return deliveryAddressAppService.GetList(input);
        }
        /// <summary>
        /// 删除地址信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut DelDeliveryAddress(IdInput input)
        {
            return deliveryAddressAppService.DelDeliveryAddress(input);
        }

        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut EnditiDefaultAddress(DeliveryAddressModelInput input)
        {
            input.UserId = UserObject.Id;
            return deliveryAddressAppService.EnditiDefaultAddress(input);
        }
        /// <summary>
        /// 查询单个地址详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public DeliveryAddress GetOne(IdInput input)
        {
            return deliveryAddressAppService.GetOne(input);
        }

    }
}
