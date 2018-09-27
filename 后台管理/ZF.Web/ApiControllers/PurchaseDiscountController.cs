using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;

namespace ZF.Web.ApiControllers
{
    public class PurchaseDiscountController : ApiController
    {
        private readonly PurchaseDiscountAppService _purchaseDiscountAppService;

        public PurchaseDiscountController( PurchaseDiscountAppService purchaseDiscountAppService )
        {
            _purchaseDiscountAppService = purchaseDiscountAppService;
        }


        /// <summary>
        /// 抵用券列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<PurchaseDiscountOutput> GetList( PurchaseDiscountListInput input )
        {
            var count = 0;
            var list = _purchaseDiscountAppService.GetList( input, out count );
            return new JqGridOutPut<PurchaseDiscountOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }


        [HttpPost]
        public MessagesOutPut AddOrEdit( PurchaseDiscountInput input )
        {
            return _purchaseDiscountAppService.AddOrEdit( input );
        }

        [HttpPost]
        public MessagesOutPut Delete( IdInputIds input )
        {
            var array = input.Ids.TrimEnd( ',' ).Split( ',' );
            foreach ( var item in array )
            {
                var model = _purchaseDiscountAppService.Get( item );
                if ( model != null )
                {
                    _purchaseDiscountAppService.Delete( model );
                }
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }

        [HttpPost]
        public PurchaseDiscount GetOne( IdInput input )
        {
            return _purchaseDiscountAppService.Get( input.Id );
        }

    }
}