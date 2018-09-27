using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CartDtoModule
{
    /// <summary>
    /// 购物车入参
    /// </summary>
    public class CartModelInput
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string RegisterUserId { get; set; }
    }
}
