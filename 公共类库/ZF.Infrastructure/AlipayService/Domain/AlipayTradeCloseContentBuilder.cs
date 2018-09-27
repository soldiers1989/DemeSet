using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Infrastructure.AlipayService.Domain
{
    public class AlipayTradeCloseContentBuilder : JsonBuilder
    {
        /// <summary>
        /// 原支付请求的商户订单号,和支付宝交易号不能同时为空
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 支付宝交易号，和商户订单号不能同时为空
        /// </summary>
        public string trade_no { get; set; }

        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
