using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.WebApiAppService.PurchaseDiscountModule;
using ZF.Core.Entity;

namespace ZF.API.Controllers
{
    public class PurchaseDiscountController : ApiController
    {
        private readonly PurchaseDiscountAppService _service;

        public PurchaseDiscountController( PurchaseDiscountAppService service ) {
            _service = service;
        }


        /// <summary>
        /// 获取最优满减
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PurchaseDiscount GetGetBestDiscountNum(decimal priceSum ) {
            return _service.GetGetBestDiscountNum( priceSum );
        }

       
    }
}
