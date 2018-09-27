using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;

namespace ZF.Web.ApiControllers
{
    public class DiscountCardController : ApiController
    {
        private readonly DiscountCardAppService _discountCardAppService;

        public DiscountCardController( DiscountCardAppService discountCardAppService) {
            _discountCardAppService = discountCardAppService;
        }


        /// <summary>
        /// 抵用券列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<DiscountCardOutput> GetList( DiscountCardListInput input ) {
            var count= 0;
            var list = _discountCardAppService.GetList( input, out count );
            return new JqGridOutPut<DiscountCardOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }


        [HttpPost]
        public MessagesOutPut AddOrEdit( DiscountCardInput input ) {
            return _discountCardAppService.AddOrEdit( input );
        }

        [HttpPost]
        public MessagesOutPut Delete( IdInputIds input ) {
            var array = input.Ids.TrimEnd( ',' ).Split( ',' );
            foreach ( var item in array )
            {
                var model = _discountCardAppService.Get( item );
                if ( model != null )
                {
                    _discountCardAppService.Delete(model);
                }
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }

        [HttpPost]
        public Discount_Card GetOne( IdInput input )
        {
            return _discountCardAppService.Get( input.Id );
        }

        [HttpPost]
        public bool IfUnique( string CardCode ) {
            return _discountCardAppService.IfUnique( CardCode );
        }

    }
}
