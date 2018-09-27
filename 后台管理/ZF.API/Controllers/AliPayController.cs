using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.WebApiAppService.AlipayModule;
using ZF.Application.WebApiAppService.SheetModule;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Infrastructure.AlipayService.Business;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 支付宝支付接口
    /// </summary>
    public class AliPayController : BaseApiController
    {
        private readonly AlipayModuleAppService alipayModuleAppService;

        private readonly SheetApiService sheetApiService;
        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="_alipayModuleAppService"></param>
        /// <param name="_sheetApiService"></param>
        public AliPayController(AlipayModuleAppService _alipayModuleAppService, SheetApiService _sheetApiService)
        {
            alipayModuleAppService = _alipayModuleAppService;
            sheetApiService = _sheetApiService;
        }

        /// <summary>
        /// 支付宝扫码支付
        /// </summary>
        /// <param name="input">支付信息</param>
        [HttpPost]
        public AlipayTradePagePayResult GetAliPayUrl(SheetModelInput input)
        {
            return alipayModuleAppService.BuildPrePageContent(input,UserObject.Id);
        }

        /// <summary>
        /// 获取用户最近一次写入的电子发票信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public SheetModelOutput GetElectronicInvoiceByUserIds()
        {
            return sheetApiService.GetElectronicInvoiceByUserId(UserObject.Id);
        }
    }
}
