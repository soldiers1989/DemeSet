using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;

namespace ZF.Web.ApiControllers
{
    public class UserDiscountCardController : ApiController
    {
        private readonly UserDiscountCardAppService _service;

        public UserDiscountCardController( UserDiscountCardAppService service) {
            _service = service;
        }


        [HttpPost]
        public JqGridOutPut<UserDiscountCardOutput> GetList( UserDiscountCardListInput input) {
            var count = 0;
            var list = _service.GetList( input, out count );
            return new JqGridOutPut<UserDiscountCardOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }
    }
}
